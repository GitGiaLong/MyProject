using Core.Libraries.Interfaces.PythonNet.Platform;
using System.Runtime.InteropServices;

namespace Core.Libraries.PythonNet.Platform
{
    class LinuxLibDL : ILibDL
    {

        private const string NativeDll = "libdl.so";

        public int RTLD_NOW => 0x2;
        public int RTLD_GLOBAL => 0x100;
        public IntPtr RTLD_DEFAULT => IntPtr.Zero;

        public static ILibDL GetInstance()
        {
            try
            {
                ILibDL libdl2 = new LinuxLibDL2();
                // call dlerror to ensure library is resolved
                libdl2.dlerror();
                return libdl2;
            }
            catch (DllNotFoundException)
            {
                return new LinuxLibDL();
            }
        }

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
