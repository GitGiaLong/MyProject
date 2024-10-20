﻿using Core.Libraries.Systems.Extensions;
using Core.Libraries.WPF.Hardware;
using System.ComponentModel;

namespace Core.Libraries.WPF.Controls
{
    /// <summary>
    /// If you use <see cref="WindowChrome"/> to extend the UI elements to the non-client area, you can include this container
    /// in the template of <see cref="Window"/> so that the content inside automatically fills the client area.
    /// Using this container can let you get rid of various margin adaptations done in
    /// Setter/Trigger of the style of <see cref="Window"/> when the window state changes.
    /// </summary>
    public class ClientAreaBorder : System.Windows.Controls.Border//, IThemeControl
    {
        /*private const int SM_CXFRAME = 32;
        private const int SM_CYFRAME = 33;
        private const int SM_CXPADDEDBORDER = 92;*/
        private static Thickness? _paddedBorderThickness;
        private static Thickness? _resizeFrameBorderThickness;
        private static Thickness? _windowChromeNonClientFrameThickness;
        private bool _borderBrushApplied = false;
        private System.Windows.Window? _oldWindow;

        /// <summary>
        /// Gets the system value for the padded border thickness (<see cref="User32.SM.CXPADDEDBORDER"/>) in WPF units.
        /// </summary>
        public Thickness PaddedBorderThickness
        {
            get
            {
                if (_paddedBorderThickness is not null)
                {
                    return _paddedBorderThickness.Value;
                }

                var paddedBorder = Interop.User32.GetSystemMetrics(Interop.User32.SM.CXPADDEDBORDER);

                (double factorX, double factorY) = GetDpi();

                var frameSize = new Size(paddedBorder, paddedBorder);
                var frameSizeInDips = new Size(frameSize.Width / factorX, frameSize.Height / factorY);

                _paddedBorderThickness = new Thickness(
                    frameSizeInDips.Width,
                    frameSizeInDips.Height,
                    frameSizeInDips.Width,
                    frameSizeInDips.Height
                );

                return _paddedBorderThickness.Value;
            }
        }

        /// <summary>
        /// Gets the system <see cref="User32.SM.CXFRAME"/> and <see cref="User32.SM.CYFRAME"/> values in WPF units.
        /// </summary>
        public static Thickness ResizeFrameBorderThickness =>
            _resizeFrameBorderThickness ??= new Thickness(
                SystemParameters.ResizeFrameVerticalBorderWidth,
                SystemParameters.ResizeFrameHorizontalBorderHeight,
                SystemParameters.ResizeFrameVerticalBorderWidth,
                SystemParameters.ResizeFrameHorizontalBorderHeight
            );

        /// <summary>
        /// Gets the thickness of the window's non-client frame used for maximizing the window with a custom chrome.
        /// </summary>
        /// <remarks>
        /// If you use a <see cref="WindowChrome"/> to extend the client area of a window to the non-client area, you need to handle the edge margin issue when the window is maximized.
        /// Use this property to get the correct margin value when the window is maximized, so that when the window is maximized, the client area can completely cover the screen client area by no less than a single pixel at any DPI.
        /// The<see cref="User32.GetSystemMetrics"/> method cannot obtain this value directly.
        /// </remarks>
        public Thickness WindowChromeNonClientFrameThickness =>
            _windowChromeNonClientFrameThickness ??= new Thickness(
                ClientAreaBorder.ResizeFrameBorderThickness.Left + PaddedBorderThickness.Left,
                ClientAreaBorder.ResizeFrameBorderThickness.Top + PaddedBorderThickness.Top,
                ClientAreaBorder.ResizeFrameBorderThickness.Right + PaddedBorderThickness.Right,
                ClientAreaBorder.ResizeFrameBorderThickness.Bottom + PaddedBorderThickness.Bottom
            );

        public ClientAreaBorder() { }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);

            if (_oldWindow is { } oldWindow)
            {
                oldWindow.StateChanged -= OnWindowStateChanged;
                oldWindow.Closing -= OnWindowClosing;
            }

            var newWindow = (System.Windows.Window?)System.Windows.Window.GetWindow(this);

            if (newWindow is not null)
            {
                newWindow.StateChanged -= OnWindowStateChanged; // Unsafe
                newWindow.StateChanged += OnWindowStateChanged;
                newWindow.Closing += OnWindowClosing;
            }

            _oldWindow = newWindow;

            ApplyDefaultWindowBorder();
        }

        private void OnWindowClosing(object? sender, CancelEventArgs e)
        {
            //Appearance.ApplicationThemeManager.Changed -= OnThemeChanged;
            if (_oldWindow != null) { _oldWindow.Closing -= OnWindowClosing; }
        }

        private void OnWindowStateChanged(object? sender, EventArgs e)
        {
            if (sender is not System.Windows.Window window) { return; }

            Thickness padding = window.WindowState == WindowState.Maximized ? WindowChromeNonClientFrameThickness : default;
            SetCurrentValue(PaddingProperty, padding);
        }

        private void ApplyDefaultWindowBorder()
        {
            if (Utilities.IsOSWindows11OrNewer || _oldWindow == null) { return; }

            _borderBrushApplied = true;

            /// SystemParameters.WindowGlassBrush
            Color borderColor = Color.FromArgb(0xFF, 0x7A, 0x7A, 0x7A);
            _oldWindow.SetCurrentValue(System.Windows.Controls.Control.BorderBrushProperty, new SolidColorBrush(borderColor));
            _oldWindow.SetCurrentValue(System.Windows.Controls.Control.BorderThicknessProperty, new Thickness(1));
        }

        private (double FactorX, double FactorY) GetDpi()
        {
            if (PresentationSource.FromVisual(this) is { } source)
            {
                return (source.CompositionTarget.TransformToDevice.M11,/*Possible null reference*/source.CompositionTarget.TransformToDevice.M22);
            }

            DisplayDpi systemDPi = DpiHelper.GetSystemDpi();

            return (systemDPi.DpiScaleX, systemDPi.DpiScaleY);
        }
    }
}
