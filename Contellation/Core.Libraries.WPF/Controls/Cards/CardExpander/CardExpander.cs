using System.ComponentModel;

namespace Core.Libraries.WPF.Controls
{

    /// <summary>
    /// Inherited from the <see cref="System.Windows.Controls.Expander"/> control which can hide the collapsible content.
    /// </summary>
    public class CardExpander : System.Windows.Controls.Expander
    {
        /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon),
            typeof(string),
            typeof(CardExpander),
            new PropertyMetadata(null)
        );

        /// <summary>Identifies the <see cref="CornerRadius"/> dependency property.</summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(CardExpander),
            new PropertyMetadata(new CornerRadius(4))
        );

        /// <summary>Identifies the <see cref="ContentPadding"/> dependency property.</summary>
        public static readonly DependencyProperty ContentPaddingProperty = DependencyProperty.Register(
            nameof(ContentPadding),
            typeof(Thickness),
            typeof(CardExpander),
            new FrameworkPropertyMetadata(
                default(Thickness),
                FrameworkPropertyMetadataOptions.AffectsParentMeasure
            )
        );

        /// <summary>
        /// Gets or sets displayed <see cref="IconElement"/>.
        /// </summary>
        public string? Icon
        {
            get => (string?)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        /// <summary>
        /// Gets or sets displayed <see cref="IconElement"/>.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Gets or sets content padding Property
        /// </summary>
        public Thickness ContentPadding
        {
            get { return (Thickness)GetValue(ContentPaddingProperty); }
            set { SetValue(ContentPaddingProperty, value); }
        }
    }
}
