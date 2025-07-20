using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Core.Libraries.PythonNet
{

    internal class ThunkInfo
    {
        public readonly Delegate Target;
        public readonly IntPtr Address;

        public ThunkInfo(Delegate target)
        {
            Debug.Assert(target is not null);
            Target = target!;
            Address = Marshal.GetFunctionPointerForDelegate(target);
        }
    }
}
