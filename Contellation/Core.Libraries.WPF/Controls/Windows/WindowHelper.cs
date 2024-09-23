﻿using Core.Entities.Systems.Enums;
using Core.Entities.Systems.Structs;
using Core.Libraries.WPF.Extensions;
using Core.Libraries.WPF.Hardware;
using Core.Libraries.WPF.Interop;
using System.Collections;
using System.ComponentModel;
using System.Security;

namespace Core.Libraries.WPF.Controls.Windows
{
    public static class WindowHelper
    {

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

                            // Win32Exception will get the Win32 error code so we don't have to
                            var dc = InteropMethods.GetDC(desktopWnd);

                            // Detecting error case from unmanaged call, required by PREsharp to throw a Win32Exception
                            if (dc == IntPtr.Zero)
                            {
                                throw new Win32Exception();
                            }

                            try
                            {
                                _dpi = InteropMethods.GetDeviceCaps(new HandleRef(null, dc), 90);
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
                                _dpiX = InteropMethods.GetDeviceCaps(new HandleRef(null, dc), 88);
                                _cacheValid[(int)CacheSlot.DpiX] = true;
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

        private static readonly BitArray _cacheValid = new((int)CacheSlot.NumSlots);
        private static Thickness _windowResizeBorderThickness;
        internal static Thickness WindowResizeBorderThickness
        {
            [SecurityCritical]
            get
            {
                lock (_cacheValid)
                {
                    while (!_cacheValid[(int)CacheSlot.WindowResizeBorderThickness])
                    {
                        _cacheValid[(int)CacheSlot.WindowResizeBorderThickness] = true;

                        var frameSize = new Size(InteropMethods.GetSystemMetrics(SM.CXSIZEFRAME), InteropMethods.GetSystemMetrics(SM.CYSIZEFRAME));
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
                APPBARDATA appBarData = default;
                var autoHide = InteropMethods.SHAppBarMessage(4, ref appBarData) != 0;
#if NET40
            return WindowResizeBorderThickness.Add(new Thickness(autoHide ? -8 : 0));
#elif NETCOREAPP
                var hdc = InteropMethods.GetDC(IntPtr.Zero);
                var scale = InteropMethods.GetDeviceCaps(hdc, 117) / (float)InteropMethods.GetDeviceCaps(hdc, 10);
                InteropMethods.ReleaseDC(IntPtr.Zero, hdc);
                return WindowResizeBorderThickness.Add(new Thickness((autoHide ? -4 : 4) * scale));
#else
                return WindowResizeBorderThickness.Add(new Thickness(autoHide ? -4 : 4));
#endif
            }
        }
    }
}
