using Core.Entities.Systems;
using System.Runtime.InteropServices;

namespace Core.Libraries.Systems.Interops
{
    /// <summary>
    /// Used by multiple technologies.
    /// </summary>
    internal class Kernel32
    {
        /// <summary>
        /// Retrieves the calling thread's last-error code value. The last-error code is maintained on a per-thread basis. Multiple threads do not overwrite each other's last-error code.
        /// </summary>
        /// <returns>The return value is the calling thread's last-error code.</returns>
        [DllImport(WindowKernel.Kernel32)]
        public static extern int GetLastError();

        /// <summary>
        /// Sets the last-error code for the calling thread.
        /// </summary>
        /// <param name="dwErrorCode">The last-error code for the thread.</param>
        [DllImport(WindowKernel.Kernel32, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern void SetLastError([In] int dwErrorCode);

        /// <summary>
        /// Determines whether the calling process is being debugged by a user-mode debugger.
        /// </summary>
        /// <returns>If the current process is running in the context of a debugger, the return value is nonzero.</returns>
        [DllImport(WindowKernel.Kernel32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsDebuggerPresent();
    }
}
