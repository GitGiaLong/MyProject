using Libraries.Custom.Enums;
using Libraries.Custom.Interops.Handles;
using Libraries.Custom.Properties;
using Libraries.Custom.Structs;
using System.ComponentModel;
using System.Security;

namespace Libraries.Custom.Interops
{
    internal class InteropMethods
    {
        #region common

        //-

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibrariesDLL.User32, SetLastError = true, ExactSpelling = true, EntryPoint = nameof(GetDC), CharSet = CharSet.Auto)]
        internal static extern IntPtr IntGetDC(HandleRef hWnd);
        [SecurityCritical]
        internal static IntPtr GetDC(HandleRef hWnd)
        {
            var hDc = IntGetDC(hWnd);
            if (hDc == IntPtr.Zero) throw new Win32Exception();

            return Collector.Add(hDc, Common.HDC);
        }

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibrariesDLL.User32, ExactSpelling = true, EntryPoint = nameof(ReleaseDC), CharSet = CharSet.Auto)]
        internal static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

        [SecurityCritical]
        internal static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
        {
            Collector.Remove((IntPtr)hDC, Common.HDC);
            return IntReleaseDC(hWnd, hDC);
        }

        [DllImport(LibrariesDLL.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO monitorInfo);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibrariesDLL.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibrariesDLL.User32)]
        internal static extern int GetSystemMetrics(SM nIndex);

        [DllImport(LibrariesDLL.User32, SetLastError = true)]
        internal static extern int ReleaseDC(IntPtr window, IntPtr dc);

        [DllImport(LibrariesDLL.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport(LibrariesDLL.User32, CharSet = CharSet.Auto)]
        internal static extern IntPtr GetDC(IntPtr ptr);


        [DllImport(LibrariesDLL.Shell32, CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

        [DllImport(LibrariesDLL.User32)]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
        #endregion

    }
}
