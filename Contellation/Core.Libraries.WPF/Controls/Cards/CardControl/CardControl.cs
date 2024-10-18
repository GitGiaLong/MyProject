using System.Windows.Automation.Peers;

namespace Core.Libraries.WPF.Controls
{
    /// <summary>
    /// Inherited from the <see cref="System.Windows.Controls.Primitives.ButtonBase"/> control which displays an additional control on the right side of the card.
    /// Inherited from the <see cref="System.Windows.Controls.Primitives.ButtonBase"/> interactive card styled according to Fluent Design.
    /// </summary>
    public class CardControl : System.Windows.Controls.Primitives.ButtonBase
    {
        /// <summary>
        /// Gets or sets header which is used for each item in the control.
        /// </summary>
        public object? Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(object),
            typeof(CardControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets displayed <see cref="Icon"/>.
        /// </summary>
        public string? Icon
        {
            get { return (string?)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(string),
            typeof(CardControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets displayed <see cref="Icon"/>.
        /// </summary>
        public FontFamily? FontFamilyIcon
        {
            get { return (FontFamily?)GetValue(FontFamilyIconProperty); }
            set { SetValue(FontFamilyIconProperty, value); }
        }
        public static readonly DependencyProperty FontFamilyIconProperty = DependencyProperty.Register(nameof(Icon), typeof(FontFamily),
            typeof(CardControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating whether to display the chevron icon on the right side of the card.
        /// </summary>
        public bool IsVisibleIcon
        {
            get { return (bool)GetValue(IsVisibleIconProperty); }
            set { SetValue(IsVisibleIconProperty, value); }
        }
        public static readonly DependencyProperty IsVisibleIconProperty = DependencyProperty.Register(nameof(IsVisibleIcon), typeof(bool),
            typeof(CardControl), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the corner radius of the control.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius),
            typeof(CardControl), new PropertyMetadata(new CornerRadius(0)));

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new CardControlAutomationPeer(this);
        }
    }
}
