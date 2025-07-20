using Core.Libraries.Interfaces.PythonNet.Platform;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Core.Libraries.PythonNet.Platform
{
    class WindowsLoader : ILibraryLoader
    {
        private const string NativeDll = "kernel32.dll";

        public IntPtr Load(string? dllToLoad)
        {
            if (dllToLoad is null) { return IntPtr.Zero; }

            var res = WindowsLoader.LoadLibrary(dllToLoad);
            
            if (res == IntPtr.Zero)
            { 
                throw new DllNotFoundException($"Could not load {dllToLoad}.", new Win32Exception()); 
            }
            
            return res;
        }

        public IntPtr GetFunction(IntPtr hModule, string procedureName)
        {
            if (hModule == IntPtr.Zero)
            {
                foreach (var module in GetAllModules())
                {
                    var func = GetProcAddress(module, procedureName);
                    if (func != IntPtr.Zero) { return func; }
                }
            }

            var res = WindowsLoader.GetProcAddress(hModule, procedureName);
            if (res == IntPtr.Zero)
            { 
                throw new MissingMethodException($"Failed to load symbol {procedureName}.", new Win32Exception()); 
            }

            return res;
        }

        public void Free(IntPtr hModule) => WindowsLoader.FreeLibrary(hModule);

        static IntPtr[] GetAllModules()
        {
            using var self = Process.GetCurrentProcess();

            uint bytes = 0;
            var result = new IntPtr[0];
            if (!EnumProcessModules(self.Handle, result, bytes, out var needsBytes)) { throw new Win32Exception(); }
            while (bytes < needsBytes)
            {
                bytes = needsBytes;
                result = new IntPtr[bytes / IntPtr.Size];
                if (!EnumProcessModules(self.Handle, result, bytes, out needsBytes)) { throw new Win32Exception(); }
            }
            return result.Take((int)(needsBytes / IntPtr.Size)).ToArray();
        }

        [DllImport(NativeDll, SetLastError = true)]
        static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport(NativeDll, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport(NativeDll)]
        static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("Psapi.dll", SetLastError = true)]
        static extern bool EnumProcessModules(IntPtr hProcess, [In, Out] IntPtr[] lphModule, uint lphModuleByteCount, out uint byteCountNeeded);
    }
}
