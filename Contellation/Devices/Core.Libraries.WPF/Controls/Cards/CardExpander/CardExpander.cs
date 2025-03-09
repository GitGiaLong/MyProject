namespace Core.Libraries.WPF.Controls
{
    /// <summary>
    /// Inherited from the <see cref="System.Windows.Controls.Expander"/> control which can hide the collapsible content.
    /// </summary>
    public class CardExpander : System.Windows.Controls.Expander
    {
        /// <summary>
        /// Gets or sets displayed <see cref="IconElement"/>.
        /// </summary>
        public string? Icon
        {
            get { return (string?)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(string),
            typeof(CardExpander), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets displayed <see cref="IconElement"/>.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius),
            typeof(CardExpander), new PropertyMetadata(new CornerRadius(4)));

        /// <summary>
        /// Gets or sets content padding Property
        /// </summary>
        public Thickness ContentPadding
        {
            get { return (Thickness)GetValue(ContentPaddingProperty); }
            set { SetValue(ContentPaddingProperty, value); }
        }
        public static readonly DependencyProperty ContentPaddingProperty = DependencyProperty.Register(nameof(ContentPadding), typeof(Thickness),
            typeof(CardExpander), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsParentMeasure));

    }
}
