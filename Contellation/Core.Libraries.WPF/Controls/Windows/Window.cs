using Core.Entities.Systems;
using Core.Entities.Systems.Structs;
using Core.Libraries.WPF.Controls.Windows;
using Core.Libraries.WPF.Helpers;
using Core.Libraries.WPF.Interop;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Shell;

namespace Core.Libraries.WPF.Controls
{
    public class Window : System.Windows.Window
    {

        #region prop

        public object AreaBarContent
        {
            get { return GetValue(AreBarContentProperty); }
            set { SetValue(AreBarContentProperty, value); }
        }
        public static readonly DependencyProperty AreBarContentProperty = DependencyProperty.Register(nameof(AreaBarContent), typeof(object), 
            typeof(Window), new PropertyMetadata(default(object)));

        public Brush AreaBarBackground
        {
            get { return (Brush)GetValue(AreaBarBackgroundProperty); }
            set { SetValue(AreaBarBackgroundProperty, value); }
        }
        public static readonly DependencyProperty AreaBarBackgroundProperty = DependencyProperty.Register(nameof(AreaBarBackground), typeof(Brush), 
            typeof(Window), new PropertyMetadata(default(Brush)));

        public Brush AreaBarForeground
        {
            get { return (Brush)GetValue(AreaBarForegroundProperty); }
            set { SetValue(AreaBarForegroundProperty, value); }
        }
        public static readonly DependencyProperty AreaBarForegroundProperty = DependencyProperty.Register(nameof(AreaBarForeground), typeof(Brush),
            typeof(Window), new PropertyMetadata(default(Brush)));

        public double AreaBarHeight
        {
            get { return (double)GetValue(AreaBarHeightProperty); }
            set { SetValue(AreaBarHeightProperty, value); }
        }
        public static readonly DependencyProperty AreaBarHeightProperty = DependencyProperty.Register(nameof(AreaBarHeight), typeof(double), 
            typeof(Window), new PropertyMetadata(22.0));

        /// <summary>
        /// Gets or sets a value indicating whether to show the minimize button.
        /// </summary>
        public bool ShowMinimize
        {
            get { return (bool)GetValue(ShowMinimizeProperty); }
            set { SetValue(ShowMinimizeProperty, value); }
        }
        /// <summary>Identifies the <see cref="ShowMinimize"/> dependency property.</summary>
        public static readonly DependencyProperty ShowMinimizeProperty = DependencyProperty.Register(nameof(ShowMinimize), typeof(bool),
            typeof(Window), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether to show the maximize button.
        /// </summary>
        public bool ShowMaximize
        {
            get { return (bool)GetValue(ShowMaximizeProperty); }
            set { SetValue(ShowMaximizeProperty, value); }
        }
        /// <summary>Identifies the <see cref="ShowMaximize"/> dependency property.</summary>
        public static readonly DependencyProperty ShowMaximizeProperty = DependencyProperty.Register(nameof(ShowMaximize), typeof(bool),
            typeof(Window), new PropertyMetadata(true));

        /// <summary>
        /// Gets a value indicating whether the current window is maximized.
        /// </summary>
        public bool IsMaximized
        {
            get { return (bool)GetValue(IsMaximizedProperty); }
            internal set { SetValue(IsMaximizedProperty, value); }
        }
        /// <summary>Identifies the <see cref="IsMaximized"/> dependency property.</summary>
        public static readonly DependencyProperty IsMaximizedProperty = DependencyProperty.Register(nameof(IsMaximized), typeof(bool),
            typeof(Window), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether to show the close button.
        /// </summary>
        public bool ShowClose
        {
            get { return (bool)GetValue(ShowCloseProperty); }
            set { SetValue(ShowCloseProperty, value); }
        }
        /// <summary>Identifies the <see cref="ShowClose"/> dependency property.</summary>
        public static readonly DependencyProperty ShowCloseProperty = DependencyProperty.Register(nameof(ShowClose), typeof(bool),
            typeof(Window), new PropertyMetadata(true));

        #endregion

        #region fields

        private double _tempNonClientAreaHeight;
        private readonly Thickness _commonPadding;
        private const string ElementMaximizeButton = "PART_MaximizeButton";

        #endregion


        static Window()
        {
            StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(ResourceHelper.GetResourceInternal<Style>("WindowCustom")));
        }

        public Window()
        {
#if NET40
            var chrome = new WindowChrome
            {
                CornerRadius = new CornerRadius(),
                GlassFrameThickness = new Thickness(0, 0, 0, 1)
            };
#else
            var chrome = new WindowChrome
            {
                CornerRadius = new CornerRadius(),
                GlassFrameThickness = new Thickness(0, 0, 0, 1),
                ResizeBorderThickness = ResizeMode == ResizeMode.NoResize ? default : new Thickness(4),
                UseAeroCaptionButtons = false
            };
#endif
            BindingOperations.SetBinding(chrome, WindowChrome.CaptionHeightProperty,
                new Binding(AreaBarHeightProperty.Name) { Source = this });
            WindowChrome.SetWindowChrome(this, chrome);
            _commonPadding = Padding;

            Loaded += (s, e) => OnLoaded(e);
        }

        protected void OnLoaded(RoutedEventArgs args)
        {

            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand,
                (s, e) => WindowState = WindowState.Minimized));
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


            ContentRendered += OnWindowContentRendered;
            if (SizeToContent != SizeToContent.WidthAndHeight)
                return;


            SizeToContent = SizeToContent.Height;
            Dispatcher.BeginInvoke(new Action(() => { SizeToContent = SizeToContent.WidthAndHeight; }));
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            if (WindowState == WindowState.Maximized)
            {
                BorderThickness = new Thickness();
                _tempNonClientAreaHeight = AreaBarHeight;
                SetCurrentValue(IsMaximizedProperty, true);
                AreaBarHeight += 8;
            }
            else
            {
                SetCurrentValue(IsMaximizedProperty, false);
                AreaBarHeight = _tempNonClientAreaHeight;
            }
        }

        /// <summary>
        /// Invoked whenever application code or an internal process,
        /// such as a rebuilding layout pass, calls the ApplyTemplate method.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button maximizeButton = GetTemplateChild<Button>(ElementMaximizeButton);

            _buttons.Add(maximizeButton);

        }
        /// <summary> Listening window hooks after rendering window content to SizeToContent support</summary>
        private void OnWindowContentRendered(object? sender, EventArgs e)
        {
            if (sender is not Window window) { return; }

            window.ContentRendered -= OnWindowContentRendered;

            IntPtr handle = new WindowInteropHelper(window).Handle;
            HwndSource windowSource = HwndSource.FromHwnd(handle) ?? throw new InvalidOperationException("Window source is null");
            windowSource.AddHook(HwndSourceHook);
        }

        private List<Button> _buttons = new List<Button>();
        private IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {

            var message = (User32.WM)msg;
            foreach (Button button in _buttons)
            {
                if (!button.ReactToHwndHook(message, lparam, out IntPtr returnIntPtr)) { continue; }

                /// Fix for when sometimes, button hover backgrounds aren't cleared correctly, causing multiple buttons to appear as if hovered.
                foreach (Button anotherButton in _buttons)
                {
                    if (anotherButton == button) { continue; }

                    if (anotherButton.IsHovered && button.IsHovered) { anotherButton.RemoveHover(); }
                }

                handled = true;
                return returnIntPtr;
            }
            switch (msg)
            {
                case User32.WM_WINDOWPOSCHANGED:
                    Padding = WindowState == WindowState.Maximized ? WindowHelper.WindowMaximizedPadding : _commonPadding;
                    break;
                case User32.WM_GETMINMAXINFO:
                    WmGetMinMaxInfo(hwnd, lparam);
                    Padding = WindowState == WindowState.Maximized ? WindowHelper.WindowMaximizedPadding : _commonPadding;
                    break;
                case 0x0084:
                    // for fixing #886
                    // https://developercommunity.visualstudio.com/t/overflow-exception-in-windowchrome/167357
                    try
                    {
                        _ = lparam.ToInt32();
                    }
                    catch (OverflowException)
                    {
                        handled = true;
                    }
                    break;
            }

            return IntPtr.Zero;
        }
        private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            var mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            var monitor = InteropMethods.MonitorFromWindow(hwnd, 0x00000002);

            if (monitor != IntPtr.Zero && mmi != null)
            {
                APPBARDATA appBarData = default;
                var autoHide = InteropMethods.SHAppBarMessage(4, ref appBarData) != 0;
                if (autoHide)
                {
                    var monitorInfo = default(MONITORINFO);
                    monitorInfo.cbSize = (uint)Marshal.SizeOf(typeof(MONITORINFO));
                    InteropMethods.GetMonitorInfo(monitor, ref monitorInfo);
                    var rcWorkArea = monitorInfo.rcWork;
                    var rcMonitorArea = monitorInfo.rcMonitor;
                    mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
                    mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
                    mmi.ptMaxSize.x = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
                    mmi.ptMaxSize.y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top - 1);
                }
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }
        private T GetTemplateChild<T>(string name) where T : DependencyObject
        {
            DependencyObject element = GetTemplateChild(name);

            if (element is not T tElement) { throw new InvalidOperationException($"Template part '{name}' is not found or is not of type {typeof(T)}"); }

            return tElement;
        }
    }
}
