using Core.Libraries.WPF.Designers;
using Core.Libraries.WPF.Extensions;
using Core.Libraries.WPF.Extensions.Intputs;
using Core.Libraries.WPF.Interop;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace Core.Libraries.WPF.Controls
{
    public class TitleBar : Control
    {
        private const string ElementNonClientArea = "PART_NonClientArea";
        private const string ElementIcon = "PART_Icon";
        private const string ElementMainGrid = "PART_MainGrid";

        private const string ElementMinimizeButton = "PART_MinimizeButton";
        private const string ElementMaximizeButton = "PART_MaximizeButton";
        private const string ElementRestoreButton = "PART_RestoreButton";
        private const string ElementCloseButton = "PART_CloseButton";

        private static DpiScale? dpiScale;

        private DependencyObject? _parentWindow;
        private UIElement _nonClientArea;

        /// <summary>
        /// Gets or sets title displayed on the left.
        /// </summary>
        public string? Title
        {
            get { return (string?)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string),
            typeof(TitleBar), new PropertyMetadata(null));

        public string? TimeLine
        {
            get { return (string?)GetValue(TimeLineProperty); }
            set { SetValue(TimeLineProperty, value); }
        }
        public static readonly DependencyProperty TimeLineProperty = DependencyProperty.Register(nameof(TimeLine), typeof(string),
            typeof(TitleBar), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the content displayed in the <see cref="TitleBar"/>.
        /// </summary>
        public object? Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(object),
            typeof(TitleBar), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the foreground of the navigation buttons.
        /// </summary>
        public Brush ButtonsForeground
        {
            get { return (Brush)GetValue(ButtonsForegroundProperty); }
            set { SetValue(ButtonsForegroundProperty, value); }
        }
        public static readonly DependencyProperty ButtonsForegroundProperty = DependencyProperty.Register(nameof(ButtonsForeground), typeof(Brush),
            typeof(TitleBar), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Gets or sets the background of the navigation buttons when hovered.
        /// </summary>
        public Brush ButtonsBackground
        {
            get { return (Brush)GetValue(ButtonsBackgroundProperty); }
            set { SetValue(ButtonsBackgroundProperty, value); }
        }
        public static readonly DependencyProperty ButtonsBackgroundProperty = DependencyProperty.Register(nameof(ButtonsBackground), typeof(Brush),
            typeof(TitleBar), new FrameworkPropertyMetadata(SystemColors.ControlBrush, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Gets a value indicating whether the current window is maximized.
        /// </summary>
        public bool IsMaximized
        {
            get { return (bool)GetValue(IsMaximizedProperty); }
            internal set { SetValue(IsMaximizedProperty, value); }
        }
        public static readonly DependencyProperty IsMaximizedProperty = DependencyProperty.Register(nameof(IsMaximized), typeof(bool),
            typeof(TitleBar), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether the controls affect main application window.
        /// </summary>
        public bool ForceShutdown
        {
            get { return (bool)GetValue(ForceShutdownProperty); }
            set { SetValue(ForceShutdownProperty, value); }
        }
        public static readonly DependencyProperty ForceShutdownProperty = DependencyProperty.Register(nameof(ForceShutdown), typeof(bool),
            typeof(TitleBar), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether to show the maximize button.
        /// </summary>
        public bool ShowMaximize
        {
            get { return (bool)GetValue(ShowMaximizeProperty); }
            set { SetValue(ShowMaximizeProperty, value); }
        }
        public static readonly DependencyProperty ShowMaximizeProperty = DependencyProperty.Register(nameof(ShowMaximize), typeof(bool),
            typeof(TitleBar), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether to show the minimize button.
        /// </summary>
        public bool ShowMinimize
        {
            get { return (bool)GetValue(ShowMinimizeProperty); }
            set { SetValue(ShowMinimizeProperty, value); }
        }
        public static readonly DependencyProperty ShowMinimizeProperty = DependencyProperty.Register(nameof(ShowMinimize), typeof(bool),
            typeof(TitleBar), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether to show the help button
        /// </summary>
        public bool ShowHelp
        {
            get { return (bool)GetValue(ShowHelpProperty); }
            set { SetValue(ShowHelpProperty, value); }
        }
        public static readonly DependencyProperty ShowHelpProperty = DependencyProperty.Register(nameof(ShowHelp), typeof(bool),
            typeof(TitleBar), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether to show the close button.
        /// </summary>
        public bool ShowClose
        {
            get { return (bool)GetValue(ShowCloseProperty); }
            set { SetValue(ShowCloseProperty, value); }
        }
        public static readonly DependencyProperty ShowCloseProperty = DependencyProperty.Register(nameof(ShowClose), typeof(bool),
            typeof(TitleBar), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether the maximize functionality is enabled. If disabled the MaximizeActionOverride action won't be called
        /// </summary>
        public bool CanMaximize
        {
            get { return (bool)GetValue(CanMaximizeProperty); }
            set { SetValue(CanMaximizeProperty, value); }
        }
        public static readonly DependencyProperty CanMaximizeProperty = DependencyProperty.Register(nameof(CanMaximize), typeof(bool),
            typeof(TitleBar), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the titlebar icon.
        /// </summary>
        //public Image? Icon
        public System.Windows.Media.Imaging.BitmapImage? Icon
        {
            get { return (System.Windows.Media.Imaging.BitmapImage?)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(System.Windows.Media.Imaging.BitmapImage),
            typeof(TitleBar), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating whether the window can be closed by double clicking on the icon
        /// </summary>
        public bool CloseWindowByDoubleClickOnIcon
        {
            get { return (bool)GetValue(CloseWindowByDoubleClickOnIconProperty); }
            set { SetValue(CloseWindowByDoubleClickOnIconProperty, value); }
        }
        public static readonly DependencyProperty CloseWindowByDoubleClickOnIconProperty = DependencyProperty.Register(nameof(CloseWindowByDoubleClickOnIcon), typeof(bool),
                typeof(TitleBar), new PropertyMetadata(false));

        /// <summary>
        /// Gets the command triggered when clicking the titlebar button.
        /// </summary>
        public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);
        public static readonly DependencyProperty TemplateButtonCommandProperty = DependencyProperty.Register(nameof(TemplateButtonCommand), typeof(IRelayCommand),
            typeof(TitleBar), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the <see cref="Action"/> that should be executed when the Maximize button is clicked."/>
        /// </summary>
        public Action<TitleBar, System.Windows.Window>? MaximizeActionOverride { get; set; }

        /// <summary>
        /// Gets or sets what <see cref="Action"/> should be executed when the Minimize button is clicked.
        /// </summary>
        public Action<TitleBar, System.Windows.Window>? MinimizeActionOverride { get; set; }

        private readonly List<Button> _buttons = new List<Button>();
        private readonly Button _button = new();
        private System.Windows.Window _currentWindow = null!;

        /*private System.Windows.Controls.Grid _mainGrid = null!;*/
        private System.Windows.Controls.Image _icon = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="TitleBar"/> class and sets the default <see cref="FrameworkElement.Loaded"/> event.
        /// </summary>
        public TitleBar()
        {
            dpiScale ??= VisualTreeHelper.GetDpi(this);

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DesignerHelper.IsInDesignMode) { return; }

            _currentWindow = Window.GetWindow(this) ?? throw new InvalidOperationException("Window is null");
            _currentWindow.StateChanged += OnParentWindowStateChanged;
            _currentWindow.ContentRendered += OnWindowContentRendered;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            Unloaded -= OnUnloaded;
        }

        /// <summary>
        /// Invoked whenever application code or an internal process, such as a rebuilding layout pass, calls the ApplyTemplate method.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _parentWindow = VisualTreeHelper.GetParent(this);

            while (_parentWindow is not null and not Window) { _parentWindow = VisualTreeHelper.GetParent(_parentWindow); }

            MouseRightButtonUp += TitleBar_MouseRightButtonUp;
            /*_mainGrid = GetTemplateChild<System.Windows.Controls.Grid>(ElementMainGrid);*/
            //_icon = GetTemplateChild<System.Windows.Controls.Image>(ElementIcon);


            Button minimizeButton = GetTemplateChild<Button>(ElementMinimizeButton);
            Button maximizeButton = GetTemplateChild<Button>(ElementMaximizeButton);
            Button closeButton = GetTemplateChild<Button>(ElementCloseButton);

            _buttons.Add(maximizeButton);
            _buttons.Add(minimizeButton);
            _buttons.Add(closeButton);

        }

        private void OnParentWindowStateChanged(object? sender, EventArgs e)
        {
            if (IsMaximized != (_currentWindow.WindowState == WindowState.Maximized))
            {
                SetCurrentValue(IsMaximizedProperty, _currentWindow.WindowState == WindowState.Maximized);
            }
        }

        /// <summary> 
        /// Listening window hooks after rendering window content to SizeToContent support
        /// </summary>
        private void OnWindowContentRendered(object? sender, EventArgs e)
        {
            if (sender is not Window window) { return; }

            window.ContentRendered -= OnWindowContentRendered;

            IntPtr handle = new WindowInteropHelper(window).Handle;
            HwndSource windowSource = HwndSource.FromHwnd(handle) ?? throw new InvalidOperationException("Window source is null");
            windowSource.AddHook(HwndSourceHook);
        }

        private IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var message = (User32.WM)msg;

            if (message is not (User32.WM.NCHITTEST or User32.WM.NCMOUSELEAVE or User32.WM.NCLBUTTONDOWN or User32.WM.NCLBUTTONUP)) { return IntPtr.Zero; }

            foreach (Button button in _buttons)
            {
                if (!button.ReactToHwndHook(message, lParam, out IntPtr returnIntPtr)) { continue; }

                /// Fix for when sometimes, button hover backgrounds aren't cleared correctly, causing multiple buttons to appear as if hovered.
                foreach (Button anotherButton in _buttons)
                {
                    if (anotherButton == button) { continue; }

                    if (anotherButton.IsHovered && button.IsHovered) { anotherButton.RemoveHover(); }
                }

                handled = true;
                return returnIntPtr;
            }

            bool isMouseOverHeaderContent = false;

            if (message == User32.WM.NCHITTEST && Header is UIElement headerUiElement) { isMouseOverHeaderContent = headerUiElement.IsMouseOverElement(lParam); }

            switch (message)
            {
                case User32.WM.NCHITTEST when CloseWindowByDoubleClickOnIcon && _icon.IsMouseOverElement(lParam):
                    /// Ideally, clicking on the icon should open the system menu, 
                    /// but when the system menu is opened manually, double-clicking on the icon does not close the window
                    handled = true;
                    return (IntPtr)User32.WM_NCHITTEST.HTSYSMENU;
                case User32.WM.NCHITTEST when this.IsMouseOverElement(lParam) && !isMouseOverHeaderContent:
                    handled = true;
                    return (IntPtr)User32.WM_NCHITTEST.HTCAPTION;
                default:
                    return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Show 'SystemMenu' on mouse right button up.
        /// </summary>
        private void TitleBar_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point point = PointToScreen(e.GetPosition(this));

            if (dpiScale is null) { throw new InvalidOperationException("dpiScale is not initialized."); }

            SystemCommands.ShowSystemMenu(_parentWindow as Window, new Point(point.X / dpiScale.Value.DpiScaleX, point.Y / dpiScale.Value.DpiScaleY));
        }

        private T GetTemplateChild<T>(string name) where T : DependencyObject
        {
            DependencyObject element = GetTemplateChild(name);

            if (element is not T tElement) { throw new InvalidOperationException($"Template part '{name}' is not found or is not of type {typeof(T)}"); }

            return tElement;
        }
    }
}
