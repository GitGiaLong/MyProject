using Core.Libraries.Structs.PythonNet.References;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Core.Libraries.PythonNet.References
{
    static class StolenReferenceExtensions
    {
        [Pure]
        [DebuggerHidden]
        public static IntPtr DangerousGetAddressOrNull(this in StolenReference reference)
            => reference.Pointer;
        
        [Pure]
        [DebuggerHidden]
        public static IntPtr DangerousGetAddress(this in StolenReference reference)
            => reference.Pointer == IntPtr.Zero ? throw new NullReferenceException() : reference.Pointer;
        
        [DebuggerHidden]
        public static StolenReference AnalyzerWorkaround(this in StolenReference reference)
        {
            IntPtr ptr = reference.DangerousGetAddressOrNull();
            return StolenReference.TakeNullable(ref ptr);
        }
    }
}
