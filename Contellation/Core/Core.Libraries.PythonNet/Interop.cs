using Core.Libraries.Helpers.PythonNet;
using Core.Libraries.PythonNet.References;
using Core.Libraries.Structs.PythonNet.References;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Core.Libraries.PythonNet
{
    /* 
     * This class defines the function prototypes (delegates) used for low 
     * level integration with the CPython runtime. It also provides name 
     * based lookup of the correct prototype for a particular Python type 
     * slot and utilities for generating method thunks for managed methods. 
     */
    internal class Interop
    {
        static readonly Dictionary<MethodInfo, Type> delegateTypes = new();

        internal static Type GetPrototype(MethodInfo method)
        {
            if (delegateTypes.TryGetValue(method, out var delegateType)) { return delegateType; }

            var parameters = method.GetParameters().Select(p => new ParameterHelper(p)).ToArray();

            foreach (var candidate in typeof(Interop).GetNestedTypes())
            {
                if (!typeof(Delegate).IsAssignableFrom(candidate)) { continue; }

                MethodInfo invoke = candidate.GetMethod("Invoke");
                var candiateParameters = invoke.GetParameters();
                if (candiateParameters.Length != parameters.Length) { continue; }

                var parametersMatch = parameters.Zip(candiateParameters, (expected, actual) 
                    => expected.Matches(actual)).All(matches => matches);

                if (!parametersMatch) { continue; }

                if (invoke.ReturnType != method.ReturnType) { continue; }

                delegateTypes.Add(method, candidate);
                return candidate;
            }

            throw new NotImplementedException(method.ToString());
        }

        internal static Dictionary<IntPtr, Delegate> allocatedThunks = new();

        internal static ThunkInfo GetThunk(MethodInfo method)
        {
            Type dt = GetPrototype(method);
            Delegate d = Delegate.CreateDelegate(dt, method);
            return GetThunk(d);
        }

        internal static ThunkInfo GetThunk(Delegate @delegate)
        {
            var info = new ThunkInfo(@delegate);
            allocatedThunks[info.Address] = @delegate;
            return info;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate NewReference B_N(BorrowedReference ob);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate NewReference BB_N(BorrowedReference ob, BorrowedReference a);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate NewReference BBB_N(BorrowedReference ob, BorrowedReference a1, BorrowedReference a2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int B_I32(BorrowedReference ob);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int BB_I32(BorrowedReference ob, BorrowedReference a);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int BBB_I32(BorrowedReference ob, BorrowedReference a1, BorrowedReference a2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int BP_I32(BorrowedReference ob, IntPtr arg);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr B_P(BorrowedReference ob);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate NewReference BBI32_N(BorrowedReference ob, BorrowedReference a1, int a2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate NewReference BP_N(BorrowedReference ob, IntPtr arg);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void N_V(NewReference ob);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int BPP_I32(BorrowedReference ob, IntPtr a1, IntPtr a2);
    }
}
