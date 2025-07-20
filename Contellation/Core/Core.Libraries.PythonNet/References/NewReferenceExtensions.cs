using Core.Libraries.PythonNet.Python;
using Core.Libraries.Structs.PythonNet.References;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Core.Libraries.PythonNet.References
{
    /// <summary>
    /// These members can not be directly in <see cref="NewReference"/> type,
    /// because <c>this</c> is always passed by value, which we need to avoid.
    /// (note <code>this in NewReference</code> vs the usual <code>this NewReference</code>)
    /// </summary>
    static class NewReferenceExtensions
    {
        /// <summary>
        /// Gets a raw pointer to the Python object
        /// </summary>
        [Pure]
        [DebuggerHidden]
        public static IntPtr DangerousGetAddress(this in NewReference reference)
            => NewReference.DangerousGetAddress(reference);
        
        [Pure]
        [DebuggerHidden]
        public static bool IsNull(this in NewReference reference)
            => NewReference.IsNull(reference);

        [Pure]
        [DebuggerHidden]
        public static BorrowedReference BorrowNullable(this in NewReference reference)
            => new(NewReference.DangerousGetAddressOrNull(reference));
        
        [Pure]
        [DebuggerHidden]
        public static BorrowedReference Borrow(this in NewReference reference)
            => reference.IsNull() ? throw new NullReferenceException() : reference.BorrowNullable();
        
        [Pure]
        [DebuggerHidden]
        public static BorrowedReference BorrowOrThrow(this in NewReference reference)
            => reference.IsNull() ? throw PythonException.ThrowLastAsClrException() : reference.BorrowNullable();
    }
}
