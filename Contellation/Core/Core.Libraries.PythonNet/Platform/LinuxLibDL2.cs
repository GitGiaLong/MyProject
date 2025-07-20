using Core.Libraries.Interfaces.PythonNet.Platform;
using System.Runtime.InteropServices;

namespace Core.Libraries.PythonNet.Platform
{
    class LinuxLibDL2 : ILibDL
    {
        private const string NativeDll = "libdl.so.2";

        public int RTLD_NOW => 0x2;
        public int RTLD_GLOBAL => 0x100;
        public IntPtr RTLD_DEFAULT => IntPtr.Zero;

        IntPtr ILibDL.dlopen(string? fileName, int flags) => dlopen(fileName, flags);
        IntPtr ILibDL.dlsym(IntPtr handle, string symbol) => dlsym(handle, symbol);
        int ILibDL.dlclose(IntPtr handle) => dlclose(handle);
        IntPtr ILibDL.dlerror() => dlerror();

        [DllImport(NativeDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr dlopen(string? fileName, int flags);

        [DllImport(NativeDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr dlsym(IntPtr handle, string symbol);

        [DllImport(NativeDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern int dlclose(IntPtr handle);

        [DllImport(NativeDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr dlerror();
    }
}
