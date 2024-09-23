using Core.Entities.Handles;
using Core.Entities.Systems;
using Core.Entities.Systems.Enums;
using Core.Entities.Systems.Structs;
using System.ComponentModel;
using System.Security;

namespace Core.Libraries.WPF.Interop
{
    internal class InteropMethods
    {
        #region common

        //-

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(WindowKernel.User32, SetLastError = true, ExactSpelling = true, EntryPoint = nameof(GetDC), CharSet = CharSet.Auto)]
        internal static extern IntPtr IntGetDC(HandleRef hWnd);
        [SecurityCritical]
        internal static IntPtr GetDC(HandleRef hWnd)
        {
            var hDc = IntGetDC(hWnd);
            if (hDc == IntPtr.Zero) throw new Win32Exception();

            return Entities.Handles.HandleCollector.Add(hDc, CommonHandles.HDC);
        }

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(WindowKernel.User32, ExactSpelling = true, EntryPoint = nameof(ReleaseDC), CharSet = CharSet.Auto)]
        internal static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

        [SecurityCritical]
        internal static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
        {
            Entities.Handles.HandleCollector.Remove((IntPtr)hDC, CommonHandles.HDC);
            return IntReleaseDC(hWnd, hDC);
        }

        [DllImport(WindowKernel.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO monitorInfo);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(WindowKernel.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(WindowKernel.User32)]
        internal static extern int GetSystemMetrics(SM nIndex);

        [DllImport(WindowKernel.User32, SetLastError = true)]
        internal static extern int ReleaseDC(IntPtr window, IntPtr dc);

        [DllImport(WindowKernel.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport(WindowKernel.User32, CharSet = CharSet.Auto)]
        internal static extern IntPtr GetDC(IntPtr ptr);


        [DllImport(WindowKernel.Shell32, CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

        [DllImport(WindowKernel.User32)]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
        #endregion

    }
}
