using Core.Libraries.Interfaces.PythonNet.Platform;
using System.Runtime.InteropServices;

namespace Core.Libraries.PythonNet.Platform
{
    static class LibraryLoader
    {
        static ILibraryLoader? _instance = null;

        public static ILibraryLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) { _instance = new WindowsLoader(); }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) { _instance = new PosixLoader(LinuxLibDL.GetInstance()); }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) { _instance = new PosixLoader(new MacLibDL()); }
                    else { throw new PlatformNotSupportedException("This operating system is not supported"); }
                }

                return _instance;
            }
        }
    }
}
