using Core.WPF.Interop.Tools;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;
using System.Security;

namespace Core.WPF.Interop
{

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal sealed class BitmapHandle : WpfSafeHandle
    {
        //this constructor, otherwise an error will be reported when using a custom ico file
        [SecurityCritical]
        private BitmapHandle() : this(true) { }

        [SecurityCritical]
        private BitmapHandle(bool ownsHandle) : base(ownsHandle, CommonHandles.GDI) { }

        [SecurityCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected override bool ReleaseHandle() { return InteropMethods.DeleteObject(handle); }

        [SecurityCritical]
        internal HandleRef MakeHandleRef(object obj) { return new(obj, handle); }

        [SecurityCritical]
        internal static BitmapHandle CreateFromHandle(IntPtr hbitmap, bool ownsHandle = true) { return new(ownsHandle) { handle = hbitmap, }; }
    }
}
