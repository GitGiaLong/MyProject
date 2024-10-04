using Core.Entities.Enums;

namespace Core.Libraries.WPF.Controls
{

    /// <summary>
    /// Extended <see cref="System.Windows.Controls.TextBlock"/> with additional parameters like <see cref="FontTypography"/>.
    /// </summary>
    public class TextBlock : System.Windows.Controls.TextBlock
    {
        /// <summary>Identifies the <see cref="FontTypography"/> dependency property.</summary>
        public static readonly DependencyProperty FontTypographyProperty = DependencyProperty.Register(
            nameof(FontTypography),
            typeof(FontTypography),
            typeof(TextBlock),
            new PropertyMetadata(
                FontTypography.Body
            )
        );

        /// <summary>
        /// Gets or sets the <see cref="FontTypography"/> of the text.
        /// </summary>
        public FontTypography FontTypography
        {
            get => (FontTypography)GetValue(FontTypographyProperty);
            set => SetValue(FontTypographyProperty, value);
        }
    }

}
