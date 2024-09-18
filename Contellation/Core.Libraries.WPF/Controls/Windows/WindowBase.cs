using Core.Entities.Systems.Enums;
using Core.Libraries.WPF.Interop.Unsafes;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Shell;

namespace Core.Libraries.WPF.Controls
{
    public class WindowBase : Window
    {
        private WindowInteropHelper? _interopHelper = null;

        /// <summary>
        /// Gets contains helper for accessing this window handle.
        /// </summary>
        protected WindowInteropHelper InteropHelper
        {
            get => _interopHelper ??= new WindowInteropHelper(this);
        }

        /// <summary>
        /// Gets or sets a value determining corner preference for current <see cref="Window"/>.
        /// </summary>
        public WindowCornerPreference WindowCornerPreference
        {
            get { return (WindowCornerPreference)GetValue(WindowCornerPreferenceProperty); }
            set { SetValue(WindowCornerPreferenceProperty, value); }
        }
        /// <summary>Identifies the <see cref="WindowCornerPreference"/> dependency property.</summary>
        public static readonly DependencyProperty WindowCornerPreferenceProperty = DependencyProperty.Register(nameof(WindowCornerPreference),
            typeof(WindowCornerPreference), typeof(WindowBase), new PropertyMetadata(WindowCornerPreference.Round, OnWindowCornerPreferenceChanged));

        /// <summary>
        /// Gets or sets a value determining preferred backdrop type for current <see cref="Window"/>.
        /// </summary>
        public WindowBackdropType WindowBackdropType
        {
            get { return (WindowBackdropType)GetValue(WindowBackdropTypeProperty); }
            set { SetValue(WindowBackdropTypeProperty, value); }
        }
        /// <summary>Identifies the <see cref="WindowBackdropType"/> dependency property.</summary>
        public static readonly DependencyProperty WindowBackdropTypeProperty = DependencyProperty.Register(nameof(WindowBackdropType),
            typeof(WindowBackdropType), typeof(WindowBase), new PropertyMetadata(WindowBackdropType.None, OnWindowBackdropTypeChanged));

        /// <summary>
        /// Gets or sets a value indicating whether the default title bar of the window should be hidden to create space for app content.
        /// </summary>
        public bool ExtendsContentIntoTitleBar
        {
            get { return (bool)GetValue(ExtendsContentIntoTitleBarProperty); }
            set { SetValue(ExtendsContentIntoTitleBarProperty, value); }
        }
        /// <summary>Identifies the <see cref="ExtendsContentIntoTitleBar"/> dependency property.</summary>
        public static readonly DependencyProperty ExtendsContentIntoTitleBarProperty = DependencyProperty.Register(nameof(ExtendsContentIntoTitleBar),
            typeof(bool), typeof(WindowBase), new PropertyMetadata(false, OnExtendsContentIntoTitleBarChanged));


        /// <summary>
        /// Initializes a new instance of the <see cref="FluentWindow"/> class.
        /// </summary>
        public WindowBase()
        {
            SetResourceReference(StyleProperty, typeof(WindowBase));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (s, e) => WindowState = WindowState.Minimized));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand,
                (s, e) =>
                {
                    if (WindowState == WindowState.Normal)
                    {
                        SetCurrentValue(WindowStateProperty, WindowState.Maximized);
                    }
                    else
                    {
                        SetCurrentValue(WindowStateProperty, WindowState.Normal);
                    }
                }));
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (s, e) => Close()));
        }

        /// <summary>
        /// Initializes static members of the <see cref="WindowBase"/> class.
        /// Overrides default properties.
        /// </summary>
        /// <remarks> Overrides default properties. </remarks>
        static WindowBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowBase), new FrameworkPropertyMetadata(typeof(WindowBase)));
        }

        /// <inheritdoc />
        protected override void OnSourceInitialized(EventArgs e)
        {
            OnCornerPreferenceChanged(default, WindowCornerPreference);
            OnExtendsContentIntoTitleBarChanged(default, ExtendsContentIntoTitleBar);
            //OnBackdropTypeChanged(default, WindowBackdropType);

            base.OnSourceInitialized(e);
        }

        /// <summary>
        /// Private <see cref="WindowCornerPreference"/> property callback.
        /// </summary>
        private static void OnWindowCornerPreferenceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not WindowBase window) { return; }

            if (e.OldValue == e.NewValue) { return; }

            window.OnCornerPreferenceChanged((WindowCornerPreference)e.OldValue, (WindowCornerPreference)e.NewValue);
        }

        /// <summary>
        /// This virtual method is called when <see cref="WindowCornerPreference"/> is changed.
        /// </summary>
        protected virtual void OnCornerPreferenceChanged(WindowCornerPreference oldValue, WindowCornerPreference newValue)
        {
            if (InteropHelper.Handle == IntPtr.Zero) { return; }

            _ = UnsafeNativeMethods.ApplyWindowCornerPreference(InteropHelper.Handle, newValue);
        }

        /// <summary>
        /// Private <see cref="WindowBackdropType"/> property callback.
        /// </summary>
        private static void OnWindowBackdropTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not WindowBase window)
            {
                return;
            }

            if (e.OldValue == e.NewValue)
            {
                return;
            }

            //window.OnBackdropTypeChanged((WindowBackdropType)e.OldValue, (WindowBackdropType)e.NewValue);
        }

        /// <summary>
        /// Private <see cref="ExtendsContentIntoTitleBar"/> property callback.
        /// </summary>
        private static void OnExtendsContentIntoTitleBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not WindowBase window)
            {
                return;
            }

            if (e.OldValue == e.NewValue)
            {
                return;
            }

            window.OnExtendsContentIntoTitleBarChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        /// This virtual method is called when <see cref="ExtendsContentIntoTitleBar"/> is changed.
        /// </summary>
        protected virtual void OnExtendsContentIntoTitleBarChanged(bool oldValue, bool newValue)
        {
            /// AllowsTransparency = true;
            SetCurrentValue(WindowStyleProperty, WindowStyle.SingleBorderWindow);

            WindowChrome.SetWindowChrome(
                this,
                new WindowChrome
                {
                    CaptionHeight = 0,
                    CornerRadius = default,
                    GlassFrameThickness = new Thickness(-1),
                    ResizeBorderThickness = ResizeMode == ResizeMode.NoResize ? default : new Thickness(4),
                    UseAeroCaptionButtons = false
                }
            );

            // WindowStyleProperty.OverrideMetadata(typeof(FluentWindow), new FrameworkPropertyMetadata(WindowStyle.SingleBorderWindow));
            // AllowsTransparencyProperty.OverrideMetadata(typeof(FluentWindow), new FrameworkPropertyMetadata(false));
            _ = UnsafeNativeMethods.RemoveWindowTitlebarContents(this);
        }
    }
}
