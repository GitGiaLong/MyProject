using Core.Libraries.Interfaces.PythonNet.Platform;
using System.Runtime.InteropServices;

namespace Core.Libraries.PythonNet.Platform
{
    class PosixLoader : ILibraryLoader
    {
        private readonly ILibDL libDL;

        public PosixLoader(ILibDL libDL)
        {
            this.libDL = libDL ?? throw new ArgumentNullException(nameof(libDL));
        }

        public IntPtr Load(string? dllToLoad)
        {
            ClearError();
            var res = libDL.dlopen(dllToLoad, libDL.RTLD_NOW | libDL.RTLD_GLOBAL);
            if (res == IntPtr.Zero)
            {
                var err = GetError();
                throw new DllNotFoundException($"Could not load {dllToLoad} with flags RTLD_NOW | RTLD_GLOBAL: {err}");
            }

            return res;
        }

        public void Free(IntPtr handle) { libDL.dlclose(handle); }

        public IntPtr GetFunction(IntPtr dllHandle, string name)
        {
            // look in the exe if dllHandle is NULL
            if (dllHandle == IntPtr.Zero) { dllHandle = libDL.RTLD_DEFAULT; }

            ClearError();
            IntPtr res = libDL.dlsym(dllHandle, name);
            if (res == IntPtr.Zero)
            {
                var err = GetError();
                throw new MissingMethodException($"Failed to load symbol {name}: {err}");
            }
            return res;
        }

        void ClearError() { libDL.dlerror(); }

        string? GetError()
        {
            var res = libDL.dlerror();
            if (res != IntPtr.Zero) { return Marshal.PtrToStringAnsi(res); }
            else { return null; }
        }
    }
}
