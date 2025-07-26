using Core.Libraries.Enums.PythonNet;
using Core.Libraries.PythonNet.Python;
using Core.Libraries.PythonNet.PythonTypes;
using Core.Libraries.PythonNet.References;
using Core.Libraries.Structs.PythonNet.References;
using System.Reflection;

namespace Core.Libraries.PythonNet
{
    /* 
     * When a delegate instance is created that has a Python implementation, 
     * the delegate manager generates a custom subclass of Dispatcher and 
     * instantiates it, passing the IntPtr of the Python callable.
     *  
     * The "real" delegate is created using CreateDelegate, passing the 
     * instance of the generated type and the name of the (generated) 
     * implementing method (Invoke). 
     * 
     * The true delegate instance holds the only reference to the dispatcher 
     * instance, which ensures that when the delegate dies, the finalizer 
     * of the referenced instance will be able to decref the Python callable. 
     * 
     * A possible alternate strategy would be to create custom subclasses 
     * of the required delegate type, storing the IntPtr in it directly. 
     * This would be slightly cleaner, but I'm not sure if delegates are 
     * too "special" for this to work. It would be more work, so for now 
     * the 80/20 rule applies :) 
     */

    public class Dispatcher
    {
        readonly PyObject target;
        readonly Type dtype;

        protected Dispatcher(PyObject target, Type dtype)
        {
            this.target = target;
            this.dtype = dtype;
        }

        public object? Dispatch(object?[] args)
        {
            PyGILState gs = PythonEngine.AcquireLock();

            try { return TrueDispatch(args); }
            finally { PythonEngine.ReleaseLock(gs); }
        }

        private object? TrueDispatch(object?[] args)
        {
            MethodInfo method = dtype.GetMethod("Invoke");
            ParameterInfo[] pi = method.GetParameters();
            Type rtype = method.ReturnType;

            NewReference callResult;
            using (var pyargs = PyTuple_New(pi.Length))
            {
                for (var i = 0; i < pi.Length; i++)
                {
                    /* 
                     * Here we own the reference to the Python value, 
                     * and give the ownership to the arg tuple. 
                     */
                    using var arg = Converter.ToPython(args[i], pi[i].ParameterType);
                    int res = PyTuple_SetItem(pyargs.Borrow(), i, arg.StealOrThrow());
                    if (res != 0) { throw PythonException.ThrowLastAsClrException(); }
                }

                callResult = PyObject_Call(target, pyargs.Borrow(), null);
            }

            if (callResult.IsNull()) { throw PythonException.ThrowLastAsClrException(); }

            using (callResult)
            {
                BorrowedReference op = callResult.Borrow();
                int byRefCount = pi.Count(parameterInfo => parameterInfo.ParameterType.IsByRef);
                if (byRefCount > 0)
                {
                    /* 
                     * By symmetry with MethodBinder.Invoke, when there are out 
                     * parameters we expect to receive a tuple containing 
                     * the result, if any, followed by the out parameters. If there is only 
                     * one out parameter and the return type of the method is void, 
                     * we instead receive the out parameter as the result from Python. 
                     */
                    bool isVoid = rtype == typeof(void);
                    int tupleSize = byRefCount + (isVoid ? 0 : 1);
                    if (isVoid && byRefCount == 1)
                    {
                        // The return type is void and there is a single out parameter.
                        for (int i = 0; i < pi.Length; i++)
                        {
                            Type t = pi[i].ParameterType;
                            if (t.IsByRef)
                            {
                                if (!Converter.ToManaged(op, t, out args[i], true))
                                {
                                    Exceptions.RaiseTypeError($"The Python function did not return {t.GetElementType()} (the out parameter type)");
                                    throw PythonException.ThrowLastAsClrException();
                                }
                                break;
                            }
                        }
                        return null;
                    }
                    else if (PyTuple_Check(op) && PyTuple_Size(op) == tupleSize)
                    {
                        int index = isVoid ? 0 : 1;
                        for (int i = 0; i < pi.Length; i++)
                        {
                            Type t = pi[i].ParameterType;
                            if (t.IsByRef)
                            {
                                BorrowedReference item = PyTuple_GetItem(op, index++);
                                if (!Converter.ToManaged(item, t, out args[i], true))
                                {
                                    Exceptions.RaiseTypeError($"The Python function returned a tuple where element {i} was not {t.GetElementType()} (the out parameter type)");
                                    throw PythonException.ThrowLastAsClrException();
                                }
                            }
                        }
                        if (isVoid) { return null; }

                        BorrowedReference item0 = PyTuple_GetItem(op, 0);
                        if (!Converter.ToManaged(item0, rtype, out object? result0, true))
                        {
                            Exceptions.RaiseTypeError($"The Python function returned a tuple where element 0 was not {rtype} (the return type)");
                            throw PythonException.ThrowLastAsClrException();
                        }
                        return result0;
                    }
                    else
                    {
                        string tpName = PyObject_GetTypeName(op);
                        if (PyTuple_Check(op)) { tpName += $" of size {PyTuple_Size(op)}"; }

                        var sb = new StringBuilder();
                        if (!isVoid) sb.Append(rtype.FullName);
                        for (int i = 0; i < pi.Length; i++)
                        {
                            Type t = pi[i].ParameterType;
                            if (t.IsByRef)
                            {
                                if (sb.Length > 0) sb.Append(",");
                                sb.Append(t.GetElementType().FullName);
                            }
                        }
                        string returnValueString = isVoid ? "" : "the return value and ";
                        Exceptions.RaiseTypeError($"Expected a tuple ({sb}) of {returnValueString}the values for out and ref parameters, got {tpName}.");
                        throw PythonException.ThrowLastAsClrException();
                    }
                }

                if (rtype == typeof(void)) { return null; }

                if (!Converter.ToManaged(op, rtype, out object? result, true)) { throw PythonException.ThrowLastAsClrException(); }

                return result;
            }
        }
    }
}
