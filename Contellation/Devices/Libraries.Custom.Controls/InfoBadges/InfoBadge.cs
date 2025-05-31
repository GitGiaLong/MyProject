using Libraries.Custom.Controls.InfoBadges;
using System.ComponentModel;

namespace Libraries.Custom.Controls
{
    public class InfoBadge : System.Windows.Controls.Control
    {
        /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(string),
            typeof(Button), new PropertyMetadata(null));

        //public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        //    nameof(Icon),
        //    typeof(string),
        //    typeof(InfoBadge),
        //    new PropertyMetadata(null, null, IconElement.Coerce)
        //);

        /// <summary>Identifies the <see cref="Severity"/> dependency property.</summary>
        public static readonly DependencyProperty SeverityProperty = DependencyProperty.Register(
            nameof(Severity),
            typeof(InfoBadgeSeverity),
            typeof(InfoBadge),
            new PropertyMetadata(InfoBadgeSeverity.Informational)
        );

        /// <summary>Identifies the <see cref="Value"/> dependency property.</summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(string),
            typeof(InfoBadge),
            new PropertyMetadata(string.Empty)
        );

        /// <summary>Identifies the <see cref="CornerRadius"/> dependency property.</summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(InfoBadge),
            new FrameworkPropertyMetadata(
                new CornerRadius(8),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender
            )
        );

        /// <summary>
        /// Gets or sets the title of the <see cref="Severity" />.
        /// </summary>
        public InfoBadgeSeverity Severity
        {
            get => (InfoBadgeSeverity)GetValue(SeverityProperty);
            set => SetValue(SeverityProperty, value);
        }

        /// <summary>
        /// Gets or sets the title of the <see cref="Value" />.
        /// </summary>
        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Gets or sets the title of the <see cref="CornerRadius" />.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Gets or sets displayed <see cref="IconElement"/>.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        public string? Icon
        {
            get => (string?)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
    }
}
