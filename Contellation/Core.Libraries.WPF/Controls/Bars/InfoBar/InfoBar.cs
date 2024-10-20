﻿using Core.Libraries.WPF.Controls.Bars.InfoBar;
using Core.Libraries.WPF.Extensions.Intputs;

namespace Core.Libraries.WPF.Controls
{
    /// <summary>
    /// An <see cref="InfoBar" /> is an inline notification for essential app-
    /// wide messages. The InfoBar will take up space in a layout and will not
    /// cover up other content or float on top of it. It supports rich content
    /// (including titles, messages, and icons) and can be configured to be
    /// user-dismissable or persistent.
    /// </summary>
    public class InfoBar : System.Windows.Controls.ContentControl
    {
        /// <summary>
        /// Gets or sets a value indicating whether the user can close the <see cref="InfoBar" />. Defaults to <c>true</c>.
        /// </summary>
        public bool IsClosable
        {
            get { return (bool)GetValue(IsClosableProperty); }
            set { SetValue(IsClosableProperty, value); }
        }
        public static readonly DependencyProperty IsClosableProperty = DependencyProperty.Register(nameof(IsClosable), typeof(bool),
            typeof(InfoBar), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="InfoBar" /> is open.
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool),
            typeof(InfoBar), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the title of the <see cref="InfoBar" />.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string),
            typeof(InfoBar), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the message of the <see cref="InfoBar" />.
        /// </summary>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message), typeof(string),
            typeof(InfoBar), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the type of the <see cref="InfoBar" /> to apply
        /// consistent status color, icon, and assistive technology settings
        /// dependent on the criticality of the notification.
        /// </summary>
        public InfoBarSeverity Severity
        {
            get { return (InfoBarSeverity)GetValue(SeverityProperty); }
            set { SetValue(SeverityProperty, value); }
        }
        public static readonly DependencyProperty SeverityProperty = DependencyProperty.Register(nameof(Severity), typeof(InfoBarSeverity),
            typeof(InfoBar), new PropertyMetadata(InfoBarSeverity.Informational));

        /// <summary>
        /// Gets the <see cref="RelayCommand{T}"/> triggered after clicking
        /// the close button.
        /// </summary>
        public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);
        public static readonly DependencyProperty TemplateButtonCommandProperty = DependencyProperty.Register(nameof(TemplateButtonCommand), typeof(IRelayCommand),
            typeof(InfoBar), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoBar"/> class.
        /// </summary>
        public InfoBar()
        {
            SetValue(TemplateButtonCommandProperty, new RelayCommand<object>(_ => SetCurrentValue(IsOpenProperty, false)));
        }
    }
}
