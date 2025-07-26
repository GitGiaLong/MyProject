using Core.Libraries.PythonNet.PythonTypes;
using Core.Libraries.PythonNet.Utils;
using System.Reflection;
using System.Reflection.Emit;

namespace Core.Libraries.PythonNet
{
    /// <summary>
    /// The DelegateManager class manages the creation of true managed
    /// delegate instances that dispatch calls to Python methods.
    /// </summary>
    internal class DelegateManager
    {
        private readonly Dictionary<Type, Type> cache = new();
        private readonly Type basetype = typeof(Dispatcher);
        private readonly Type arrayType = typeof(object[]);
        private readonly Type voidtype = typeof(void);
        private readonly Type typetype = typeof(Type);
        private readonly Type pyobjType = typeof(PyObject);
        private readonly CodeGenerator codeGenerator = new();
        private readonly ConstructorInfo arrayCtor;
        private readonly MethodInfo dispatch;

        public DelegateManager()
        {
            arrayCtor = arrayType.GetConstructor(new[] { typeof(int) });
            dispatch = basetype.GetMethod("Dispatch");
        }

        /// <summary>
        /// GetDispatcher is responsible for creating a class that provides
        /// an appropriate managed callback method for a given delegate type.
        /// </summary>
        private Type GetDispatcher(Type dtype)
        {
            /* 
             * If a dispatcher type for the given delegate type has already 
             * been generated, get it from the cache. The cache maps delegate 
             * types to generated dispatcher types. A possible optimization 
             * for the future would be to generate dispatcher types based on 
             * unique signatures rather than delegate types, since multiple 
             * delegate types with the same sig could use the same dispatcher. 
             */
            if (cache.TryGetValue(dtype, out Type item)) { return item; }

            string name = $"__{dtype.FullName}Dispatcher";
            name = name.Replace('.', '_');
            name = name.Replace('+', '_');
            TypeBuilder tb = codeGenerator.DefineType(name, basetype);

            /* 
             * Generate a constructor for the generated type that calls the 
             * appropriate constructor of the Dispatcher base type. 
             */
            MethodAttributes ma = MethodAttributes.Public | MethodAttributes.HideBySig |
                                  MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
            var cc = CallingConventions.Standard;
            Type[] args = { pyobjType, typetype };
            ConstructorBuilder cb = tb.DefineConstructor(ma, cc, args);
            ConstructorInfo ci = basetype.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, args, null);
            ILGenerator il = cb.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Call, ci);
            il.Emit(OpCodes.Ret);

            /* 
             * Method generation: we generate a method named "Invoke" on the 
             * dispatcher type, whose signature matches the delegate type for 
             * which it is generated. The method body simply packages the 
             * arguments and hands them to the Dispatch() method, which deals 
             * with converting the arguments, calling the Python method and 
             * converting the result of the call. 
             */
            MethodInfo method = dtype.GetMethod("Invoke");
            ParameterInfo[] pi = method.GetParameters();

            var signature = new Type[pi.Length];
            for (var i = 0; i < pi.Length; i++) { signature[i] = pi[i].ParameterType; }

            MethodBuilder mb = tb.DefineMethod("Invoke", MethodAttributes.Public, method.ReturnType, signature);

            il = mb.GetILGenerator();
            // loc_0 = new object[pi.Length]
            il.DeclareLocal(arrayType);
            il.Emit(OpCodes.Ldc_I4, pi.Length);
            il.Emit(OpCodes.Newobj, arrayCtor);
            il.Emit(OpCodes.Stloc_0);

            bool anyByRef = false;

            for (var c = 0; c < signature.Length; c++)
            {
                Type t = signature[c];
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ldc_I4, c);
                il.Emit(OpCodes.Ldarg_S, (byte)(c + 1));

                if (t.IsByRef)
                {
                    // The argument is a pointer.  We must dereference the pointer to get the value or object it points to.
                    t = t.GetElementType();
                    if (t.IsValueType) { il.Emit(OpCodes.Ldobj, t); }
                    else { il.Emit(OpCodes.Ldind_Ref); }
                    anyByRef = true;
                }

                if (t.IsValueType) { il.Emit(OpCodes.Box, t); }

                // args[c] = arg
                il.Emit(OpCodes.Stelem_Ref);
            }

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, dispatch);

            if (anyByRef)
            {
                // Dispatch() will have modified elements of the args list that correspond to out parameters.
                CodeGenerator.GenerateMarshalByRefsBack(il, signature);
            }

            if (method.ReturnType == voidtype) { il.Emit(OpCodes.Pop); }
            else if (method.ReturnType.IsValueType) { il.Emit(OpCodes.Unbox_Any, method.ReturnType); }

            il.Emit(OpCodes.Ret);

            Type disp = tb.CreateType();
            cache[dtype] = disp;
            return disp;
        }

        /// <summary>
        /// Given a delegate type and a callable Python object, GetDelegate
        /// returns an instance of the delegate type. The delegate instance
        /// returned will dispatch calls to the given Python object.
        /// </summary>
        internal Delegate GetDelegate(Type dtype, PyObject callable)
        {
            Type dispatcher = GetDispatcher(dtype);
            object[] args = { callable, dtype };
            object o = Activator.CreateInstance(dispatcher, args);
            return Delegate.CreateDelegate(dtype, o, "Invoke");
        }
    }
}
