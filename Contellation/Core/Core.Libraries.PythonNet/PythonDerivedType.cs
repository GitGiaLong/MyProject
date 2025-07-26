using Core.Libraries.Enums.PythonNet;
using Core.Libraries.Interfaces.PythonNet;
using Core.Libraries.PythonNet.PY;
using Core.Libraries.PythonNet.PythonTypes;
using Core.Libraries.PythonNet.References;
using Core.Libraries.PythonNet.Types;
using Core.Libraries.PythonNet.Utils;
using Core.Libraries.Structs.PythonNet.References;
using Core.Libraries.Structs.PythonNet.Types;
using System.ComponentModel;
using System.Reflection;

namespace Core.Libraries.PythonNet
{
    /// <summary>
    /// PythonDerivedType contains static methods used by the dynamically created
    /// derived type that allow it to call back into python from overridden virtual
    /// methods, and also handle the construction and destruction of the python
    /// object.
    /// </summary>
    /// <remarks>
    /// This has to be public as it's called from methods on dynamically built classes
    /// potentially in other assemblies.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(Util.InternalUseOnly)]
    public class PythonDerivedType
    {
        internal const string PyObjName = "__pyobj__";
        internal const BindingFlags PyObjFlags = BindingFlags.Instance | BindingFlags.NonPublic;

        /// <summary>
        /// This is the implementation of the overridden methods in the derived
        /// type. It looks for a python method with the same name as the method
        /// on the managed base class and if it exists and isn't the managed
        /// method binding (i.e. it has been overridden in the derived python
        /// class) it calls it, otherwise it calls the base method.
        /// </summary>
        public static T? InvokeMethod<T>(IPythonDerivedType obj, string methodName, string origMethodName,
            object[] args, RuntimeMethodHandle methodHandle, RuntimeTypeHandle declaringTypeHandle)
        {
            var self = GetPyObj(obj);

            if (null != self.Ref)
            {
                var disposeList = new List<PyObject>();
                PyGILState gs = PyGILState_Ensure();
                try
                {
                    using var pyself = new PyObject(self.CheckRun());
                    using PyObject method = pyself.GetAttr(methodName, None);
                    if (method.Reference != PyNone)
                    {
                        // if the method hasn't been overridden then it will be a managed object
                        ManagedType? managedMethod = ManagedType.GetManagedObject(method.Reference);
                        if (null == managedMethod)
                        {
                            var pyargs = new PyObject[args.Length];
                            for (var i = 0; i < args.Length; ++i)
                            {
                                pyargs[i] = Converter.ToPythonImplicit(args[i]).MoveToPyObject();
                                disposeList.Add(pyargs[i]);
                            }

                            PyObject py_result = method.Invoke(pyargs);
                            var clrMethod = methodHandle != default
                                ? MethodBase.GetMethodFromHandle(methodHandle, declaringTypeHandle) : null;
                            PyTuple? result_tuple = MarshalByRefsBack(args, clrMethod, py_result, outsOffset: 1);
                            return result_tuple is not null ? result_tuple[0].As<T>() : py_result.As<T>();
                        }
                    }
                }
                finally
                {
                    foreach (PyObject x in disposeList) { x?.Dispose(); }
                    PyGILState_Release(gs);
                }
            }

            if (origMethodName == null)
            {
                throw new NotImplementedException("Python object does not have a '" + methodName + "' method");
            }

            return (T)obj.GetType().InvokeMember(origMethodName, BindingFlags.InvokeMethod, null, obj, args);
        }

        public static void InvokeMethodVoid(IPythonDerivedType obj, string methodName, string origMethodName,
            object?[] args, RuntimeMethodHandle methodHandle, RuntimeTypeHandle declaringTypeHandle)
        {
            var self = GetPyObj(obj);
            if (null != self.Ref)
            {
                var disposeList = new List<PyObject>();
                PyGILState gs = PyGILState_Ensure();
                try
                {
                    using var pyself = new PyObject(self.CheckRun());
                    using PyObject method = pyself.GetAttr(methodName, None);
                    if (method.Reference != None)
                    {
                        // if the method hasn't been overridden then it will be a managed object
                        ManagedType? managedMethod = ManagedType.GetManagedObject(method);
                        if (null == managedMethod)
                        {
                            var pyargs = new PyObject[args.Length];
                            for (var i = 0; i < args.Length; ++i)
                            {
                                pyargs[i] = Converter.ToPythonImplicit(args[i]).MoveToPyObject();
                                disposeList.Add(pyargs[i]);
                            }

                            PyObject py_result = method.Invoke(pyargs);
                            var clrMethod = methodHandle != default
                                ? MethodBase.GetMethodFromHandle(methodHandle, declaringTypeHandle) : null;
                            MarshalByRefsBack(args, clrMethod, py_result, outsOffset: 0);
                            return;
                        }
                    }
                }
                finally
                {
                    foreach (PyObject x in disposeList) { x?.Dispose(); }
                    PyGILState_Release(gs);
                }
            }

            if (origMethodName == null)
            {
                throw new NotImplementedException($"Python object does not have a '{methodName}' method");
            }

            obj.GetType().InvokeMember(origMethodName, BindingFlags.InvokeMethod, null, obj, args);
        }

        /// <summary>
        /// If the method has byref arguments, reinterprets Python return value
        /// as a tuple of new values for those arguments, and updates corresponding
        /// elements of <paramref name="args"/> array.
        /// </summary>
        private static PyTuple? MarshalByRefsBack(object?[] args, MethodBase? method, PyObject pyResult, int outsOffset)
        {
            if (method is null) { return null; }

            var parameters = method.GetParameters();
            PyTuple? outs = null;
            int byrefIndex = 0;
            for (int i = 0; i < parameters.Length; ++i)
            {
                Type type = parameters[i].ParameterType;
                if (!type.IsByRef) { continue; }

                type = type.GetElementType();

                if (outs is null)
                {
                    outs = new PyTuple(pyResult);
                    pyResult.Dispose();
                }

                args[i] = outs[byrefIndex + outsOffset].AsManagedObject(type);
                byrefIndex++;
            }
            if (byrefIndex > 0 && outs!.Length() > byrefIndex + outsOffset) { throw new ArgumentException("Too many output parameters"); }

            return outs;
        }

        public static T? InvokeGetProperty<T>(IPythonDerivedType obj, string propertyName)
        {
            var self = GetPyObj(obj);

            if (null == self.Ref)
            {
                throw new NullReferenceException("Instance must be specified when getting a property");
            }

            PyGILState gs = PyGILState_Ensure();
            try
            {
                using var pyself = new PyObject(self.CheckRun());
                using var pyvalue = pyself.GetAttr(propertyName);
                return pyvalue.As<T>();
            }
            finally
            {
                PyGILState_Release(gs);
            }
        }

        public static void InvokeSetProperty<T>(IPythonDerivedType obj, string propertyName, T value)
        {
            var self = GetPyObj(obj);

            if (null == self.Ref)
            {
                throw new NullReferenceException("Instance must be specified when setting a property");
            }

            PyGILState gs = PyGILState_Ensure();
            try
            {
                using var pyself = new PyObject(self.CheckRun());
                using var pyvalue = Converter.ToPythonImplicit(value).MoveToPyObject();
                pyself.SetAttr(propertyName, pyvalue);
            }
            finally
            {
                PyGILState_Release(gs);
            }
        }

        public static void InvokeCtor(IPythonDerivedType obj, string origCtorName, object[] args)
        {
            var selfRef = GetPyObj(obj);
            if (selfRef.Ref == null)
            {
                // this might happen when the object is created from .NET
                using var _ = Py.GIL();
                /* 
                 * In the end we decrement the python object's reference count. 
                 * This doesn't actually destroy the object, it just sets the reference to this object 
                 * to be a weak reference and it will be destroyed when the C# object is destroyed. 
                 */
                using var self = CLRObject.GetReference(obj, obj.GetType());
                SetPyObj(obj, self.Borrow());
            }

            // call the base constructor
            obj.GetType().InvokeMember(origCtorName, BindingFlags.InvokeMethod, null, obj, args);
        }

        public static void PyFinalize(IPythonDerivedType obj)
        {
            /* 
             * the C# object is being destroyed which must mean there are no more 
             * references to the Python object as well 
             */
            var self = GetPyObj(obj);
            Finalizer.Instance.AddDerivedFinalizedObject(ref self.RawObj, self.Run);
        }

        internal static void Finalize(IntPtr derived)
        {
            var @ref = NewReference.DangerousFromPointer(derived);

            ClassBase.tp_clear(@ref.Borrow());

            var type = PyObject_TYPE(@ref.Borrow());

            if (!HostedInPython || TypeManagerInitialized)
            {
                /* 
                 * rare case when it's needed 
                 * matches correspdonging PyObject_GC_UnTrack 
                 * in ClassDerivedObject.tp_dealloc 
                 */
                PyObject_GC_Del(@ref.Steal());

                // must decref our type
                XDecref(StolenReference.DangerousFromPointer(type.DangerousGetAddress()));
            }
        }

        internal static FieldInfo? GetPyObjField(Type type) => type.GetField(PyObjName, PyObjFlags);

        internal static UnsafeReferenceWithRun GetPyObj(IPythonDerivedType obj)
        {
            FieldInfo fi = GetPyObjField(obj.GetType())!;
            return (UnsafeReferenceWithRun)fi.GetValue(obj);
        }

        internal static void SetPyObj(IPythonDerivedType obj, BorrowedReference pyObj)
        {
            FieldInfo fi = GetPyObjField(obj.GetType())!;
            fi.SetValue(obj, new UnsafeReferenceWithRun(pyObj));
        }
    }
}
