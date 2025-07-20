using Core.Libraries.PythonNet.Attributes;
using Core.Libraries.PythonNet.Utils;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Core.Libraries.Structs.PythonNet.References
{
    /// <summary>
    /// Should only be used for the arguments of Python C API functions, that steal references,
    /// and internal <see cref="PyObject"/> constructors.
    /// </summary>
    [NonCopyable]
    readonly ref struct StolenReference
    {
        internal readonly IntPtr Pointer;

        [DebuggerHidden]
        StolenReference(IntPtr pointer)
        {
            Pointer = pointer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StolenReference Take(ref IntPtr ptr)
        {
            if (ptr == IntPtr.Zero) throw new ArgumentNullException(nameof(ptr));
            return TakeNullable(ref ptr);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerHidden]
        public static StolenReference TakeNullable(ref IntPtr ptr)
        {
            var stolenAddr = ptr;
            ptr = IntPtr.Zero;
            return new StolenReference(stolenAddr);
        }

        [Pure]
        public static bool operator ==(in StolenReference reference, NullOnly? @null)
            => reference.Pointer == IntPtr.Zero;
        [Pure]
        public static bool operator !=(in StolenReference reference, NullOnly? @null)
            => reference.Pointer != IntPtr.Zero;

        [Pure]
        public override bool Equals(object obj)
        {
            if (obj is IntPtr ptr)
                return ptr == Pointer;

            return false;
        }

        [Pure]
        public override int GetHashCode() => Pointer.GetHashCode();

        [Pure]
        public static StolenReference DangerousFromPointer(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero) throw new ArgumentNullException(nameof(ptr));
            return new StolenReference(ptr);
        }
    }
}
