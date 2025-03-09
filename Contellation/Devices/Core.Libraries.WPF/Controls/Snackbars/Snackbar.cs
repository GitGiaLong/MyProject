using Core.Libraries.WPF.Controls.Snackbars;
using Core.Libraries.WPF.Extensions.Intputs;
using Core.Libraries.WPF.Handlers;
using System.Windows.Controls;

namespace Core.Libraries.WPF.Controls
{
    /// <summary>
    /// Snackbar inform user of a process that an app has performed or will perform. It appears temporarily, towards the bottom of the window.
    /// </summary>
    public class Snackbar : System.Windows.Controls.ContentControl
    {
        /// <summary> 
        /// Gets or sets displayed Icon
        /// </summary>
        public string? Icon
        {
            get { return (string?)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(string),
            typeof(Snackbar), new PropertyMetadata(null));

        /// <summary> 
        /// Gets or sets FontFamily Icon
        /// </summary>
        public FontFamily? FontFamilyIcon
        {
            get { return (FontFamily?)GetValue(FontFamilyIconProperty); }
            set { SetValue(FontFamilyIconProperty, value); }
        }
        public static readonly DependencyProperty FontFamilyIconProperty = DependencyProperty.Register(nameof(FontFamilyIcon), typeof(FontFamily),
            typeof(Snackbar), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Snackbar"/> close button should be visible.
        /// </summary>
        public bool IsCloseButtonEnabled
        {
            get { return (bool)GetValue(IsCloseButtonEnabledProperty); }
            set { SetValue(IsCloseButtonEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsCloseButtonEnabledProperty = DependencyProperty.Register(nameof(IsCloseButtonEnabled), typeof(bool),
            typeof(Snackbar), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the transform.
        /// </summary>
        public TranslateTransform? SlideTransform
        {
            get { return (TranslateTransform?)GetValue(SlideTransformProperty); }
            set { SetValue(SlideTransformProperty, value); }
        }
        public static readonly DependencyProperty SlideTransformProperty = DependencyProperty.Register(nameof(SlideTransform), typeof(TranslateTransform),
            typeof(Snackbar), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Snackbar"/> is visible.
        /// </summary>
        public bool IsShown
        {
            get { return (bool)GetValue(IsShownProperty); }
            set { SetValue(IsShownProperty, value); }
        }
        public static readonly DependencyProperty IsShownProperty = DependencyProperty.Register(nameof(IsShown), typeof(bool),
            typeof(Snackbar), new PropertyMetadata(false, (d, e) => (d as Snackbar)?.OnIsShownChanged(e)));
        protected void OnIsShownChanged(DependencyPropertyChangedEventArgs e)
        {
            bool newValue = (bool)e.NewValue;

            if (newValue) { OnOpened(); }
            else { OnClosed(); }
        }

        /// <summary>
        /// Gets or sets a time for which the <see cref="Snackbar"/> should be visible.
        /// </summary>
        public TimeSpan Timeout
        {
            get { return (TimeSpan)GetValue(TimeoutProperty); }
            set { SetValue(TimeoutProperty, value); }
        }
        public static readonly DependencyProperty TimeoutProperty = DependencyProperty.Register(nameof(Timeout), typeof(TimeSpan),
            typeof(Snackbar), new PropertyMetadata(TimeSpan.FromSeconds(2)));

        /// <summary>
        /// Gets or sets the title of the <see cref="Snackbar"/>.
        /// </summary>
        public object? Title
        {
            get { return GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(object),
            typeof(Snackbar), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the title template of the <see cref="Snackbar"/>.
        /// </summary>
        public DataTemplate? TitleTemplate
        {
            get { return (DataTemplate?)GetValue(TitleTemplateProperty); }
            set { SetValue(TitleTemplateProperty, value); }
        }
        public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register(nameof(TitleTemplate), typeof(DataTemplate),
            typeof(Snackbar), new PropertyMetadata(null));

        ///// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
        //public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        //    nameof(Icon),
        //    typeof(IconElement),
        //    typeof(Snackbar),
        //    new PropertyMetadata(null, null, IconElement.Coerce)
        //);

        ///// <summary>Identifies the <see cref="Appearance"/> dependency property.</summary>
        //public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
        //    nameof(Appearance),
        //    typeof(ControlAppearance),
        //    typeof(Snackbar),
        //    new PropertyMetadata(ControlAppearance.Secondary)
        //);

        ///// <summary>
        ///// Gets or sets the icon
        ///// </summary>
        //[Bindable(true)]
        //[Category("Appearance")]
        //public IconElement? Icon
        //{
        //    get => (IconElement?)GetValue(IconProperty);
        //    set => SetValue(IconProperty, value);
        //}

        ///// <inheritdoc />
        //[Bindable(true)]
        //[Category("Appearance")]
        //public ControlAppearance Appearance
        //{
        //    get => (ControlAppearance)GetValue(AppearanceProperty);
        //    set => SetValue(AppearanceProperty, value);
        //}

        /// <summary>
        /// Gets the command triggered after clicking the button in the template.
        /// </summary>
        public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);
        public static readonly DependencyProperty TemplateButtonCommandProperty = DependencyProperty.Register(nameof(TemplateButtonCommand), typeof(IRelayCommand),
            typeof(Snackbar), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the foreground of the <see cref="ContentControl.Content"/>.
        /// </summary>
        public Brush ContentForeground
        {
            get { return (Brush)GetValue(ContentForegroundProperty); }
            set { SetValue(ContentForegroundProperty, value); }
        }
        public static readonly DependencyProperty ContentForegroundProperty = DependencyProperty.Register(nameof(ContentForeground), typeof(Brush),
            typeof(Snackbar), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Occurs when the snackbar is about to open.
        /// </summary>
        public event TypedEventHandler<Snackbar, RoutedEventArgs> Opened
        {
            add => AddHandler(OpenedEvent, value);
            remove => RemoveHandler(OpenedEvent, value);
        }
        public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent(nameof(Opened), RoutingStrategy.Bubble,
            typeof(TypedEventHandler<Snackbar, RoutedEventArgs>), typeof(Snackbar));

        /// <summary>
        /// Occurs when the snackbar is about to close.
        /// </summary>
        public event TypedEventHandler<Snackbar, RoutedEventArgs> Closed
        {
            add => AddHandler(ClosedEvent, value);
            remove => RemoveHandler(ClosedEvent, value);
        }
        public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(nameof(Closed), RoutingStrategy.Bubble,
            typeof(TypedEventHandler<Snackbar, RoutedEventArgs>), typeof(Snackbar));

        /// <summary>
        /// Initializes a new instance of the <see cref="Snackbar"/> class with a specified presenter.
        /// </summary>
        /// <param name="presenter">The <see cref="SnackbarPresenter"/> to manage the snackbar's display and interactions.</param>
        public Snackbar(SnackbarPresenter presenter)
        {
            Presenter = presenter;

            SetValue(TemplateButtonCommandProperty, new RelayCommand<object>(_ => Hide()));
        }

        protected SnackbarPresenter Presenter { get; }

        /// <summary>
        /// Shows the <see cref="Snackbar"/>
        /// </summary>
        public virtual void Show() { Show(false); }

        /// <summary>
        /// Shows the <see cref="Snackbar"/>
        /// </summary>
        public virtual void Show(bool immediately)
        {
            if (immediately) { _ = Presenter.ImmediatelyDisplay(this); }
            else { Presenter.AddToQue(this); }
        }

        /// <summary>
        /// Shows the <see cref="Snackbar"/>.
        /// </summary>
        public virtual Task ShowAsync() { return ShowAsync(false); }

        /// <summary>
        /// Shows the <see cref="Snackbar"/>.
        /// </summary>
        public virtual async Task ShowAsync(bool immediately)
        {
            if (immediately) { await Presenter.ImmediatelyDisplay(this); }
            else { Presenter.AddToQue(this); }
        }

        /// <summary>
        /// Hides the <see cref="Snackbar"/>
        /// </summary>
        protected virtual void Hide() { _ = Presenter.HideCurrent(); }

        /// <summary>
        /// This virtual method is called when <see cref="Snackbar"/> is opening and it raises the <see cref="Opened"/> <see langword="event"/>.
        /// </summary>
        protected virtual void OnOpened() { RaiseEvent(new RoutedEventArgs(OpenedEvent, this)); }

        /// <summary>
        /// This virtual method is called when <see cref="Snackbar"/> is closing and it raises the <see cref="Closed"/> <see langword="event"/>.
        /// </summary>
        protected virtual void OnClosed() { RaiseEvent(new RoutedEventArgs(ClosedEvent, this)); }
    }
}
