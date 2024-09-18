﻿using Core.WPF.Interop;
using Core.WPF.Properties;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace Core.WPF.Controls
{
    public class TitleBarButton : Button
    {
        /// <summary> Gets or sets the Type of the button.</summary>
        public TitleBarButtonType ButtonType
        {
            get { return (TitleBarButtonType)GetValue(ButtonTypeProperty); }
            set { SetValue(ButtonTypeProperty, value); }
        }
        /// <summary>Identifies the <see cref="ButtonType"/> dependency property.</summary>
        public static readonly DependencyProperty ButtonTypeProperty = DependencyProperty.Register(nameof(ButtonType), typeof(TitleBarButtonType),
            typeof(TitleBarButton), new PropertyMetadata(TitleBarButtonType.Unknown, OnButtonTypeChanged));

        /// <summary> Gets or sets the foreground of the navigation buttons. </summary>
        public Brush ButtonsForeground
        {
            get { return (Brush)GetValue(ButtonsForegroundProperty); }
            set { SetValue(ButtonsForegroundProperty, value); }
        }
        /// <summary>Identifies the <see cref="ButtonsForeground"/> dependency property.</summary>
        public static readonly DependencyProperty ButtonsForegroundProperty = DependencyProperty.Register(nameof(ButtonsForeground), typeof(Brush),
            typeof(TitleBarButton), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary> Gets or sets the foreground of the navigation buttons when moused over.</summary>
        public Brush? MouseOverButtonsForeground
        {
            get { return (Brush?)GetValue(MouseOverButtonsForegroundProperty); }
            set { SetValue(MouseOverButtonsForegroundProperty, value); }
        }
        /// <summary>Identifies the <see cref="MouseOverButtonsForeground"/> dependency property.</summary>
        public static readonly DependencyProperty MouseOverButtonsForegroundProperty = DependencyProperty.Register(nameof(MouseOverButtonsForeground), typeof(Brush),
                typeof(TitleBarButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public Brush RenderButtonsForeground
        {
            get { return (Brush)GetValue(RenderButtonsForegroundProperty); }
            set { SetValue(RenderButtonsForegroundProperty, value); }
        }
        /// <summary>Identifies the <see cref="RenderButtonsForeground"/> dependency property.</summary>
        public static readonly DependencyProperty RenderButtonsForegroundProperty = DependencyProperty.Register(nameof(RenderButtonsForeground), typeof(Brush),
            typeof(TitleBarButton), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits));

        public bool IsHovered { get; private set; }

        private readonly Brush _defaultBackgroundBrush = Brushes.Transparent; // REVIEW: Should it be transparent?
        private User32.WM_NCHITTEST _returnValue;

        private bool _isClickedDown;

        public TitleBarButton()
        {
            Loaded += TitleBarButton_Loaded;
            Unloaded += TitleBarButton_Unloaded;
        }

        private void TitleBarButton_Unloaded(object sender, RoutedEventArgs e)
        {
            DependencyPropertyDescriptor.FromProperty(ButtonsForegroundProperty, typeof(Brush)).RemoveValueChanged(this, OnButtonsForegroundChanged);
        }

        private void TitleBarButton_Loaded(object sender, RoutedEventArgs e)
        {
            SetCurrentValue(RenderButtonsForegroundProperty, ButtonsForeground);
            DependencyPropertyDescriptor.FromProperty(ButtonsForegroundProperty, typeof(Brush)).AddValueChanged(this, OnButtonsForegroundChanged);
        }

        private void OnButtonsForegroundChanged(object? sender, EventArgs e)
        {
            SetCurrentValue(RenderButtonsForegroundProperty, IsHovered ? MouseOverButtonsForeground : ButtonsForeground);
        }

        /// <summary> Forces button background to change. </summary>
        public void Hover()
        {
            if (IsHovered) { return; }

            SetCurrentValue(BackgroundProperty, MouseOverBackground);
            if (MouseOverButtonsForeground != null) { SetCurrentValue(RenderButtonsForegroundProperty, MouseOverButtonsForeground); }

            IsHovered = true;
        }

        /// <summary> Forces button background to change. </summary>
        public void RemoveHover()
        {
            if (!IsHovered) { return; }

            SetCurrentValue(BackgroundProperty, _defaultBackgroundBrush);
            SetCurrentValue(RenderButtonsForegroundProperty, ButtonsForeground);

            IsHovered = false;
            _isClickedDown = false;
        }

        /// <summary> Invokes click on the button. </summary>
        public void InvokeClick()
        {
            if (new ButtonAutomationPeer(this).GetPattern(PatternInterface.Invoke) is IInvokeProvider invokeProvider) { invokeProvider.Invoke(); }

            _isClickedDown = false;
        }

        internal bool ReactToHwndHook(User32.WM msg, IntPtr lParam, out IntPtr returnIntPtr)
        {
            returnIntPtr = IntPtr.Zero;

            switch (msg)
            {
                case User32.WM.NCHITTEST:
                    if (this.IsMouseOverElement(lParam))
                    {
                        /*Debug.WriteLine($"Hitting {ButtonType} | return code {_returnValue}");*/
                        Hover();
                        returnIntPtr = (IntPtr)_returnValue;
                        return true;
                    }

                    RemoveHover();
                    return false;
                case User32.WM.NCMOUSELEAVE: // Mouse leaves the window
                    RemoveHover();
                    return false;
                case User32.WM.NCLBUTTONDOWN when this.IsMouseOverElement(lParam): /// Left button clicked down
                    _isClickedDown = true;
                    return true;
                case User32.WM.NCLBUTTONUP when _isClickedDown && this.IsMouseOverElement(lParam): /// Left button clicked up
                    InvokeClick();
                    return true;
                default:
                    return false;
            }
        }

        private static void OnButtonTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TitleBarButton titleBarButton) { return; }

            titleBarButton.OnButtonTypeChanged(e);
        }

        protected void OnButtonTypeChanged(DependencyPropertyChangedEventArgs e)
        {
            var buttonType = (TitleBarButtonType)e.NewValue;

            _returnValue = buttonType switch
            {
                TitleBarButtonType.Unknown => User32.WM_NCHITTEST.HTNOWHERE,

                TitleBarButtonType.Help => User32.WM_NCHITTEST.HTHELP,
                TitleBarButtonType.Support => User32.WM_NCHITTEST.HTCLIENT,

                TitleBarButtonType.Minimize => User32.WM_NCHITTEST.HTMINBUTTON,
                TitleBarButtonType.Close => User32.WM_NCHITTEST.HTCLOSE,
                TitleBarButtonType.Restore => User32.WM_NCHITTEST.HTMAXBUTTON,
                TitleBarButtonType.Maximize => User32.WM_NCHITTEST.HTMAXBUTTON,
                _ => throw new ArgumentOutOfRangeException("e.NewValue", buttonType, $"Unsupported button type: {buttonType}.")
            };
        }
    }
}