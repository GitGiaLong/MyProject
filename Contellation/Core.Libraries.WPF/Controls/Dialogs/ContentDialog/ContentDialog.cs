﻿using Core.Libraries.WPF.Controls.Dialogs.ContentDialog;
using Core.Libraries.WPF.Controls.Dialogs.ContentDialog.EventArgs;
using Core.Libraries.WPF.Extensions.Intputs;
using Core.Libraries.WPF.Handlers;
using System.Windows.Controls;

namespace Core.Libraries.WPF.Controls
{
    /// <summary>
    /// Dialogue displayed inside the application covering its internals, displaying some content.
    /// </summary>
    /// <example>
    /// <code lang="xml">
    /// &lt;ContentPresenter x:Name="RootContentDialogPresenter" Grid.Row="0" /&gt;
    /// </code>
    /// <code lang="csharp">
    /// var contentDialog = new ContentDialog(RootContentDialogPresenter);
    ///
    /// contentDialog.SetCurrentValue(ContentDialog.TitleProperty, "Hello World");
    /// contentDialog.SetCurrentValue(ContentControl.ContentProperty, "This is a message");
    /// contentDialog.SetCurrentValue(ContentDialog.CloseButtonTextProperty, "Close this dialog");
    ///
    /// await contentDialog.ShowAsync(cancellationToken);
    /// </code>
    /// <code lang="csharp">
    /// var contentDialogService = new ContentDialogService();
    /// contentDialogService.SetContentPresenter(RootContentDialogPresenter);
    ///
    /// await _contentDialogService.ShowSimpleDialogAsync(
    ///     new SimpleContentDialogCreateOptions()
    ///         {
    ///             Title = "The cake?",
    ///             Content = "IS A LIE!",
    ///             PrimaryButtonText = "Save",
    ///             SecondaryButtonText = "Don't Save",
    ///             CloseButtonText = "Cancel"
    ///         }
    ///     );
    /// </code>
    /// </example>
    public class ContentDialog : ContentControl
    {
        /// <summary>
        /// Gets or sets the title of the <see cref="ContentDialog"/>.
        /// </summary>
        public object? Title
        {
            get { return GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(object),
            typeof(ContentDialog), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the title template of the <see cref="ContentDialog"/>.
        /// </summary>
        public DataTemplate? TitleTemplate
        {
            get { return (DataTemplate?)GetValue(TitleTemplateProperty); }
            set { SetValue(TitleTemplateProperty, value); }
        }
        public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register(nameof(TitleTemplate), typeof(DataTemplate),
            typeof(ContentDialog), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the width of the <see cref="ContentDialog"/>.
        /// </summary>
        public double DialogWidth
        {
            get { return (double)GetValue(DialogWidthProperty); }
            set { SetValue(DialogWidthProperty, value); }
        }
        public static readonly DependencyProperty DialogWidthProperty = DependencyProperty.Register(nameof(DialogWidth), typeof(double),
            typeof(ContentDialog), new PropertyMetadata(double.PositiveInfinity));

        /// <summary>
        /// Gets or sets the height of the <see cref="ContentDialog"/>.
        /// </summary>
        public double DialogHeight
        {
            get { return (double)GetValue(DialogHeightProperty); }
            set { SetValue(DialogHeightProperty, value); }
        }
        public static readonly DependencyProperty DialogHeightProperty = DependencyProperty.Register(nameof(DialogHeight), typeof(double),
            typeof(ContentDialog), new PropertyMetadata(double.PositiveInfinity));

        /// <summary>
        /// Gets or sets the max width of the <see cref="ContentDialog"/>.
        /// </summary>
        public double DialogMaxWidth
        {
            get { return (double)GetValue(DialogMaxWidthProperty); }
            set { SetValue(DialogMaxWidthProperty, value); }
        }
        public static readonly DependencyProperty DialogMaxWidthProperty = DependencyProperty.Register(nameof(DialogMaxWidth), typeof(double),
            typeof(ContentDialog), new PropertyMetadata(double.PositiveInfinity));

        /// <summary>
        /// Gets or sets the max height of the <see cref="ContentDialog"/>.
        /// </summary>
        public double DialogMaxHeight
        {
            get { return (double)GetValue(DialogMaxHeightProperty); }
            set { SetValue(DialogMaxHeightProperty, value); }
        }
        public static readonly DependencyProperty DialogMaxHeightProperty = DependencyProperty.Register(nameof(DialogMaxHeight), typeof(double),
            typeof(ContentDialog), new PropertyMetadata(double.PositiveInfinity));

        /// <summary>
        /// Gets or sets the margin of the <see cref="ContentDialog"/>.
        /// </summary>
        public Thickness DialogMargin
        {
            get { return (Thickness)GetValue(DialogMarginProperty); }
            set { SetValue(DialogMarginProperty, value); }
        }
        public static readonly DependencyProperty DialogMarginProperty = DependencyProperty.Register(nameof(DialogMargin), typeof(Thickness),
            typeof(ContentDialog));

        /// <summary>
        /// Gets or sets the text to display on the primary button.
        /// </summary>
        public string PrimaryButtonText
        {
            get { return (string)GetValue(PrimaryButtonTextProperty); }
            set { SetValue(PrimaryButtonTextProperty, value); }
        }
        public static readonly DependencyProperty PrimaryButtonTextProperty = DependencyProperty.Register(nameof(PrimaryButtonText), typeof(string),
            typeof(ContentDialog), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the text to be displayed on the secondary button.
        /// </summary>
        public string SecondaryButtonText
        {
            get { return (string)GetValue(SecondaryButtonTextProperty); }
            set { SetValue(SecondaryButtonTextProperty, value); }
        }
        public static readonly DependencyProperty SecondaryButtonTextProperty = DependencyProperty.Register(nameof(SecondaryButtonText), typeof(string),
            typeof(ContentDialog), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the text to display on the close button.
        /// </summary>
        public string CloseButtonText
        {
            get { return (string)GetValue(CloseButtonTextProperty); }
            set { SetValue(CloseButtonTextProperty, value); }
        }
        public static readonly DependencyProperty CloseButtonTextProperty = DependencyProperty.Register(nameof(CloseButtonText), typeof(string),
            typeof(ContentDialog), new PropertyMetadata("Close"));

        ///// <summary>
        ///// Gets or sets the <see cref="SymbolRegular"/> on the secondary button.
        ///// </summary>
        //public IconElement? PrimaryButtonIcon
        //{
        //    get { return (IconElement?)GetValue(PrimaryButtonIconProperty);}
        //    set { SetValue(PrimaryButtonIconProperty, value);}
        //}
        //public static readonly DependencyProperty PrimaryButtonIconProperty = DependencyProperty.Register(nameof(PrimaryButtonIcon), typeof(IconElement),
        //    typeof(ContentDialog), new PropertyMetadata(null));

        ///// <summary>
        ///// Gets or sets the <see cref="SymbolRegular"/> on the primary button.
        ///// </summary>
        //public IconElement? SecondaryButtonIcon
        //{
        //    get { return (IconElement?)GetValue(SecondaryButtonIconProperty);}
        //    set { SetValue(SecondaryButtonIconProperty, value);}
        //}
        //public static readonly DependencyProperty SecondaryButtonIconProperty = DependencyProperty.Register(nameof(SecondaryButtonIcon), typeof(IconElement),
        //    typeof(ContentDialog), new PropertyMetadata(null));

        ///// <summary>
        ///// Gets or sets the <see cref="SymbolRegular"/> on the close button.
        ///// </summary>
        //public IconElement? CloseButtonIcon
        //{
        //    get { return (IconElement?)GetValue(CloseButtonIconProperty);}
        //    set { SetValue(CloseButtonIconProperty, value);}
        //}
        //public static readonly DependencyProperty CloseButtonIconProperty = DependencyProperty.Register(nameof(CloseButtonIcon), typeof(IconElement),
        //    typeof(ContentDialog), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="ContentDialog"/> primary button is enabled.
        /// </summary>
        public bool IsPrimaryButtonEnabled
        {
            get { return (bool)GetValue(IsPrimaryButtonEnabledProperty); }
            set { SetValue(IsPrimaryButtonEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsPrimaryButtonEnabledProperty = DependencyProperty.Register(nameof(IsPrimaryButtonEnabled), typeof(bool),
            typeof(ContentDialog), new PropertyMetadata(true));

        ///// <summary>
        ///// Gets or sets a value indicating whether the <see cref="ContentDialog"/> secondary button is enabled.
        ///// </summary>
        //public bool IsSecondaryButtonEnabled
        //{
        //    get { return (bool)GetValue(IsSecondaryButtonEnabledProperty);}
        //    set { SetValue(IsSecondaryButtonEnabledProperty, value);}
        //}
        ///// <summary>Identifies the <see cref="IsSecondaryButtonEnabled"/> dependency property.</summary>
        //public static readonly DependencyProperty IsSecondaryButtonEnabledProperty = DependencyProperty.Register(nameof(IsSecondaryButtonEnabled), typeof(bool),
        //    typeof(ContentDialog), new PropertyMetadata(true));

        ///// <summary>
        ///// Gets or sets the <see cref="ControlAppearance"/> to apply to the primary button.
        ///// </summary>
        //public ControlAppearance PrimaryButtonAppearance
        //{
        //    get { return (ControlAppearance)GetValue(PrimaryButtonAppearanceProperty);}
        //    set { SetValue(PrimaryButtonAppearanceProperty, value);}
        //}
        ///// <summary>Identifies the <see cref="PrimaryButtonAppearance"/> dependency property.</summary>
        //public static readonly DependencyProperty PrimaryButtonAppearanceProperty = DependencyProperty.Register(nameof(PrimaryButtonAppearance), typeof(ControlAppearance),
        //    typeof(ContentDialog), new PropertyMetadata(ControlAppearance.Primary));

        ///// <summary>
        ///// Gets or sets the <see cref="ControlAppearance"/> to apply to the secondary button.
        ///// </summary>
        //public ControlAppearance SecondaryButtonAppearance
        //{
        //    get { return (ControlAppearance)GetValue(SecondaryButtonAppearanceProperty);}
        //    set { SetValue(SecondaryButtonAppearanceProperty, value);}
        //}
        ///// <summary>Identifies the <see cref="SecondaryButtonAppearance"/> dependency property.</summary>
        //public static readonly DependencyProperty SecondaryButtonAppearanceProperty = DependencyProperty.Register(nameof(SecondaryButtonAppearance), typeof(ControlAppearance),
        //    typeof(ContentDialog), new PropertyMetadata(ControlAppearance.Secondary));

        ///// <summary>
        ///// Gets or sets the <see cref="ControlAppearance"/> to apply to the close button.
        ///// </summary>
        //public ControlAppearance CloseButtonAppearance
        //{
        //    get { return (ControlAppearance)GetValue(CloseButtonAppearanceProperty);}
        //    set { SetValue(CloseButtonAppearanceProperty, value);}
        //}
        ///// <summary>Identifies the <see cref="CloseButtonAppearance"/> dependency property.</summary>
        //public static readonly DependencyProperty CloseButtonAppearanceProperty = DependencyProperty.Register(nameof(CloseButtonAppearance), typeof(ControlAppearance),
        //    typeof(ContentDialog), new PropertyMetadata(ControlAppearance.Secondary));

        /// <summary>
        /// Gets or sets a value that indicates which button on the dialog is the default action.
        /// </summary>
        public ContentDialogButton DefaultButton
        {
            get { return (ContentDialogButton)GetValue(DefaultButtonProperty); }
            set { SetValue(DefaultButtonProperty, value); }
        }
        public static readonly DependencyProperty DefaultButtonProperty = DependencyProperty.Register(nameof(DefaultButton), typeof(ContentDialogButton),
            typeof(ContentDialog), new PropertyMetadata(ContentDialogButton.Primary));

        /// <summary>
        /// Gets or sets a value indicating whether the footer buttons are visible.
        /// </summary>
        public bool IsFooterVisible
        {
            get { return (bool)GetValue(IsFooterVisibleProperty); }
            set { SetValue(IsFooterVisibleProperty, value); }
        }
        public static readonly DependencyProperty IsFooterVisibleProperty = DependencyProperty.Register(nameof(IsFooterVisible), typeof(bool),
            typeof(ContentDialog), new PropertyMetadata(true));

        /// <summary>
        /// Gets command triggered after clicking the button in the template.
        /// </summary>
        public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);
        public static readonly DependencyProperty TemplateButtonCommandProperty = DependencyProperty.Register(nameof(TemplateButtonCommand), typeof(IRelayCommand),
            typeof(ContentDialog), new PropertyMetadata(null));

        /// <summary>
        /// Occurs after the dialog is opened.
        /// </summary>
        public event TypedEventHandler<ContentDialog, RoutedEventArgs> Opened
        {
            add => AddHandler(OpenedEvent, value);
            remove => RemoveHandler(OpenedEvent, value);
        }
        public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent(nameof(Opened), RoutingStrategy.Bubble,
            typeof(TypedEventHandler<ContentDialog, RoutedEventArgs>), typeof(ContentDialog));

        /// <summary>
        /// Occurs after the dialog starts to close, but before it is closed and before the <see cref="Closed"/> event occurs.
        /// </summary>
        public event TypedEventHandler<ContentDialog, ContentDialogClosingEventArgs> Closing
        {
            add => AddHandler(ClosingEvent, value);
            remove => RemoveHandler(ClosingEvent, value);
        }
        public static readonly RoutedEvent ClosingEvent = EventManager.RegisterRoutedEvent(nameof(Closing), RoutingStrategy.Bubble,
            typeof(TypedEventHandler<ContentDialog, ContentDialogClosingEventArgs>), typeof(ContentDialog));

        /// <summary>
        /// Occurs after the dialog is closed.
        /// </summary>
        public event TypedEventHandler<ContentDialog, ContentDialogClosedEventArgs> Closed
        {
            add => AddHandler(ClosedEvent, value);
            remove => RemoveHandler(ClosedEvent, value);
        }
        public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(nameof(Closed), RoutingStrategy.Bubble,
            typeof(TypedEventHandler<ContentDialog, ContentDialogClosedEventArgs>), typeof(ContentDialog));

        /// <summary>
        /// Occurs after the <see cref="ContentDialogButton"/> has been tapped.
        /// </summary>
        public event TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs> ButtonClicked
        {
            add => AddHandler(ButtonClickedEvent, value);
            remove => RemoveHandler(ButtonClickedEvent, value);
        }
        public static readonly RoutedEvent ButtonClickedEvent = EventManager.RegisterRoutedEvent(nameof(ButtonClicked), RoutingStrategy.Bubble,
            typeof(TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs>), typeof(ContentDialog));

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialog"/> class.
        /// </summary>
        public ContentDialog()
        {
            SetValue(TemplateButtonCommandProperty, new RelayCommand<ContentDialogButton>(OnButtonClick));

            Loaded += static (sender, _) =>
            {
                var self = (ContentDialog)sender;
                self.OnLoaded();
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialog"/> class.
        /// </summary>
        /// <param name="dialogHost"><see cref="DialogHost"/> inside of which the dialogue will be placed. The new <see cref="ContentDialog"/> will replace the current <see cref="ContentPresenter.Content"/>.</param>
        public ContentDialog(ContentPresenter? dialogHost)
        {
            if (dialogHost is null) { throw new ArgumentNullException(nameof(dialogHost)); }

            DialogHost = dialogHost;

            SetValue(TemplateButtonCommandProperty, new RelayCommand<ContentDialogButton>(OnButtonClick));

            Loaded += static (sender, _) =>
            {
                var self = (ContentDialog)sender;
                self.OnLoaded();
            };
        }

        /// <summary>
        ///  Gets or sets <see cref="DialogHost"/> inside of which the dialogue will be placed. The new <see cref="ContentDialog"/> will replace the current <see cref="ContentPresenter.Content"/>.
        /// </summary>
        public ContentPresenter? DialogHost { get; set; } = default;

        [Obsolete("ContentPresenter is deprecated. Please use DialogHost instead.")]
        public ContentPresenter? ContentPresenter { get; set; } = default;

        protected TaskCompletionSource<ContentDialogResult>? Tcs { get; set; }

        /// <summary>
        /// Shows the dialog
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("WpfAnalyzers.DependencyProperty", "WPF0041:Set mutable dependency properties using SetCurrentValue",
            Justification = "SetCurrentValue(ContentProperty, ...) will not work")]
        public async Task<ContentDialogResult> ShowAsync(CancellationToken cancellationToken = default)
        {
            if (DialogHost is null) { throw new InvalidOperationException("DialogHost was not set"); }

            Tcs = new TaskCompletionSource<ContentDialogResult>();
            CancellationTokenRegistration tokenRegistration = cancellationToken.Register(o => Tcs.TrySetCanceled((CancellationToken)o!), cancellationToken);

            ContentDialogResult result = ContentDialogResult.None;

            try
            {
                DialogHost.Content = this;
                result = await Tcs.Task;

                return result;
            }
            finally
            {
#if NET6_0_OR_GREATER
                await tokenRegistration.DisposeAsync();
#else
            tokenRegistration.Dispose();
#endif
                DialogHost.Content = null;
                OnClosed(result);
            }
        }

        /// <summary>
        /// Hides the dialog with result
        /// </summary>
        public virtual void Hide(ContentDialogResult result = ContentDialogResult.None)
        {
            var closingEventArgs = new ContentDialogClosingEventArgs(ClosingEvent, this) { Result = result };

            RaiseEvent(closingEventArgs);

            if (!closingEventArgs.Cancel) { _ = Tcs?.TrySetResult(result); }
        }

        /// <summary>
        /// Occurs after ContentPresenter.Content = null
        /// </summary>
        protected virtual void OnClosed(ContentDialogResult result)
        {
            var closedEventArgs = new ContentDialogClosedEventArgs(ClosedEvent, this) { Result = result };

            RaiseEvent(closedEventArgs);
        }

        /// <summary>
        /// Invoked when a <see cref="ContentDialogButton"/> is clicked.
        /// </summary>
        /// <param name="button">The button that was clicked.</param>
        protected virtual void OnButtonClick(ContentDialogButton button)
        {
            var buttonClickEventArgs = new ContentDialogButtonClickEventArgs(ButtonClickedEvent, this) { Button = button };

            RaiseEvent(buttonClickEventArgs);

            ContentDialogResult result = button switch
            {
                ContentDialogButton.Primary => ContentDialogResult.Primary,
                ContentDialogButton.Secondary => ContentDialogResult.Secondary,
                _ => ContentDialogResult.None
            };

            Hide(result);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var rootElement = (UIElement)GetVisualChild(0)!;

            rootElement.Measure(availableSize);
            Size desiredSize = rootElement.DesiredSize;

            Size newSize = GetNewDialogSize(desiredSize);

            SetCurrentValue(DialogHeightProperty, newSize.Height);
            SetCurrentValue(DialogWidthProperty, newSize.Width);

            ResizeWidth(rootElement);
            ResizeHeight(rootElement);

            return desiredSize;
        }

        /// <summary>
        /// Occurs after Loaded event
        /// </summary>
        protected virtual void OnLoaded()
        {
            _ = Focus();

            RaiseEvent(new RoutedEventArgs(OpenedEvent));
        }

        private Size GetNewDialogSize(Size desiredSize)
        {
            var paddingWidth = Padding.Left + Padding.Right;
            var paddingHeight = Padding.Top + Padding.Bottom;

            var marginHeight = DialogMargin.Bottom + DialogMargin.Top;
            var marginWidth = DialogMargin.Left + DialogMargin.Right;

            var width = desiredSize.Width - marginWidth + paddingWidth;
            var height = desiredSize.Height - marginHeight + paddingHeight;

            return new Size(width, height);
        }

        private void ResizeWidth(UIElement element)
        {
            if (DialogWidth <= DialogMaxWidth) { return; }

            SetCurrentValue(DialogWidthProperty, DialogMaxWidth);
            element.UpdateLayout();

            SetCurrentValue(DialogHeightProperty, element.DesiredSize.Height);

            if (DialogHeight > DialogMaxHeight)
            {
                SetCurrentValue(DialogMaxHeightProperty, DialogHeight);
                /*Debug.WriteLine($"DEBUG | {GetType()} | WARNING | DialogHeight > DialogMaxHeight after resizing width!");*/
            }
        }

        private void ResizeHeight(UIElement element)
        {
            if (DialogHeight <= DialogMaxHeight) { return; }

            SetCurrentValue(DialogHeightProperty, DialogMaxHeight);
            element.UpdateLayout();

            SetCurrentValue(DialogWidthProperty, element.DesiredSize.Width);

            if (DialogWidth > DialogMaxWidth)
            {
                SetCurrentValue(DialogMaxWidthProperty, DialogWidth);
                /*Debug.WriteLine($"DEBUG | {GetType()} | WARNING | DialogWidth > DialogMaxWidth after resizing height!");*/
            }
        }
    }
}
