using Core.WPF.Extensions;
using Core.WPF.Hardware;
using Core.WPF.Interop.Tools;
using System.Collections;
using System.ComponentModel;
using System.Security;
using System.Windows.Interop;

namespace Core.WPF.Controls.Windows
{
    public static class WindowHelper
    {
        /// <summary>
        /// Get an active window in the current application
        /// </summary>
        /// <returns></returns>
        public static Window GetActiveWindow()
        {
            var activeWindow = InteropMethods.GetActiveWindow();
            return Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.GetHandle() == activeWindow);
        }

        private static readonly BitArray _cacheValid = new((int)InteropValues.CacheSlot.NumSlots);

        private static bool _setDpiX = true;

        private static bool _dpiInitialized;

        private static readonly object _dpiLock = new();

        private static int _dpi;

        internal static int Dpi
        {
            [SecurityCritical, SecuritySafeCritical]
            get
            {
                if (!_dpiInitialized)
                {
                    lock (_dpiLock)
                    {
                        if (!_dpiInitialized)
                        {
                            var desktopWnd = new HandleRef(null, IntPtr.Zero);

                            /// Win32Exception will get the Win32 error code so we don't have to
                            var dc = InteropMethods.GetDC(desktopWnd);

                            /// Detecting error case from unmanaged call, required by PREsharp to throw a Win32Exception
                            if (dc == IntPtr.Zero)
                            {
                                throw new Win32Exception();
                            }

                            try
                            {
                                _dpi = InteropMethods.GetDeviceCaps(new HandleRef(null, dc), InteropValues.LOGPIXELSY);
                                _dpiInitialized = true;
                            }
                            finally
                            {
                                InteropMethods.ReleaseDC(desktopWnd, new HandleRef(null, dc));
                            }
                        }
                    }
                }
                return _dpi;
            }
        }

        private static int _dpiX;

        internal static int DpiX
        {
            [SecurityCritical, SecuritySafeCritical]
            get
            {
                if (_setDpiX)
                {
                    lock (_cacheValid)
                    {
                        if (_setDpiX)
                        {
                            _setDpiX = false;
                            var desktopWnd = new HandleRef(null, IntPtr.Zero);
                            var dc = InteropMethods.GetDC(desktopWnd);
                            if (dc == IntPtr.Zero)
                            {
                                throw new Win32Exception();
                            }
                            try
                            {
                                _dpiX = InteropMethods.GetDeviceCaps(new HandleRef(null, dc), InteropValues.LOGPIXELSX);
                                _cacheValid[(int)InteropValues.CacheSlot.DpiX] = true;
                            }
                            finally
                            {
                                InteropMethods.ReleaseDC(desktopWnd, new HandleRef(null, dc));
                            }
                        }
                    }
                }

                return _dpiX;
            }
        }

        private static Thickness _windowResizeBorderThickness;

        internal static Thickness WindowResizeBorderThickness
        {
            [SecurityCritical]
            get
            {
                lock (_cacheValid)
                {
                    while (!_cacheValid[(int)InteropValues.CacheSlot.WindowResizeBorderThickness])
                    {
                        _cacheValid[(int)InteropValues.CacheSlot.WindowResizeBorderThickness] = true;

                        var frameSize = new Size(InteropMethods.GetSystemMetrics(InteropValues.SM.CXSIZEFRAME), InteropMethods.GetSystemMetrics(InteropValues.SM.CYSIZEFRAME));
                        var frameSizeInDips = DpiHelper.DeviceSizeToLogical(frameSize, DpiX / 96.0, Dpi / 96.0);

                        _windowResizeBorderThickness = new Thickness(frameSizeInDips.Width, frameSizeInDips.Height, frameSizeInDips.Width, frameSizeInDips.Height);
                    }
                }

                return _windowResizeBorderThickness;
            }
        }

        public static Thickness WindowMaximizedPadding
        {
            get
            {
                InteropValues.APPBARDATA appBarData = default;
                var autoHide = InteropMethods.SHAppBarMessage(4, ref appBarData) != 0;
#if NET40
            return WindowResizeBorderThickness.Add(new Thickness(autoHide ? -8 : 0));
#elif NETCOREAPP
                var hdc = InteropMethods.GetDC(IntPtr.Zero);
                var scale = InteropMethods.GetDeviceCaps(hdc, InteropValues.DESKTOPVERTRES) / (float)InteropMethods.GetDeviceCaps(hdc, InteropValues.VERTRES);
                InteropMethods.ReleaseDC(IntPtr.Zero, hdc);
                return WindowResizeBorderThickness.Add(new Thickness((autoHide ? -4 : 4) * scale));
#else
                return WindowResizeBorderThickness.Add(new Thickness(autoHide ? -4 : 4));
#endif
            }
        }

        public static IntPtr CreateHandle() => new WindowInteropHelper(new Window()).EnsureHandle();

        public static IntPtr GetHandle(this Window window) => new WindowInteropHelper(window).EnsureHandle();

        public static HwndSource GetHwndSource(this Window window) => HwndSource.FromHwnd(window.GetHandle());

        /// <summary>
        /// Let the window be activated as the topmost window in the foreground
        /// </summary>
        /// <param name="window"></param>
        public static void SetWindowToForeground(Window window)
        {
            // [WPF How to activate a window as the topmost window in the foreground - lindexi - Blog Garden](https://www.cnblogs.com/lindexi/p/12749671.html)
            var interopHelper = new WindowInteropHelper(window);
            var thisWindowThreadId = InteropMethods.GetWindowThreadProcessId(interopHelper.Handle, out _);
            var currentForegroundWindow = InteropMethods.GetForegroundWindow();
            var currentForegroundWindowThreadId = InteropMethods.GetWindowThreadProcessId(currentForegroundWindow, out _);

            // [c# - Bring a window to the front in WPF - Stack Overflow](https://stackoverflow.com/questions/257587/bring-a-window-to-the-front-in-wpf )
            // [Correct usage of SetForegroundWindow](https://www.cnblogs.com/ziwuge/archive/2012/01/06/2315342.html )
            /*
                1.Get window handleFindWindow 
                2.Switch keyboard input focusAttachThreadInput 
                3.Show window ShowWindow (some windows are minimized/hidden)
                4.Change the Z Order of the window and use SetWindowPos to make it the highest. In order not to affect the Z Order of subsequent windows, restore it after changing it.
                5.Finally SetForegroundWindow 
             */

            InteropMethods.AttachThreadInput(currentForegroundWindowThreadId, thisWindowThreadId, true);

            window.Show();
            window.Activate();
            // Remove input links to other threads
            InteropMethods.AttachThreadInput(currentForegroundWindowThreadId, thisWindowThreadId, false);

            // Used to kick out other windows on the upper level
            if (window.Topmost != true)
            {
                window.Topmost = true;
                window.Topmost = false;
            }
        }

        /// <summary>
        ///     Start dragging the window using touch and end automatically after touch lift
        /// </summary>
        //public static void TouchDragMove(this Window window) => new TouchDragMoveWindowHelper(window).Start();

        //public static void StartFullScreen(this Window window) => FullScreenHelper.StartFullScreen(window);

        //public static void EndFullScreen(this Window window) => FullScreenHelper.EndFullScreen(window);
    }
}
