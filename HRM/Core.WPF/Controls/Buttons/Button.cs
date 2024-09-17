using System.Windows.Controls;

namespace Core.WPF.Controls
{
    /// <summary>Inherited from the <see cref="System.Windows.Controls.Button"/>.</summary>
    public class Button : System.Windows.Controls.Button
    {
        /// <summary> Gets or sets displayed Icon</summary>
        public string? Icon
        {
            get { return (string?)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(string), 
            typeof(Button), new PropertyMetadata(null));

        public FontFamily? FontFamilyIcon
        {
            get { return (FontFamily?)GetValue(FontFamilyIconProperty); }
            set { SetValue(FontFamilyIconProperty, value); }
        }
        /// <summary>Identifies the <see cref="FontFamilyIcon"/> dependency property.</summary>
        public static readonly DependencyProperty FontFamilyIconProperty = DependencyProperty.Register(nameof(FontFamilyIcon), typeof(FontFamily), 
            typeof(Button), new PropertyMetadata(null));

        /// <summary>Gets or sets background <see cref="Brush"/>.</summary>
        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }
        /// <summary>Identifies the <see cref="MouseOverBackground"/> dependency property.</summary>
        public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.Register(nameof(MouseOverBackground), typeof(Brush), 
            typeof(Button),
            new PropertyMetadata(Border.BackgroundProperty.DefaultMetadata.DefaultValue));

        /// <summary>Gets or sets border <see cref="Brush"/> when the user mouses over the button.</summary>
        public Brush MouseOverBorderBrush
        {
            get { return (Brush)GetValue(MouseOverBorderBrushProperty); }
            set { SetValue(MouseOverBorderBrushProperty, value); }
        }
        /// <summary>Identifies the <see cref="MouseOverBorderBrush"/> dependency property.</summary>
        public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.Register(nameof(MouseOverBorderBrush), typeof(Brush), 
            typeof(Button), new PropertyMetadata(Border.BorderBrushProperty.DefaultMetadata.DefaultValue));

        /// <summary>Gets or sets the foreground <see cref="Brush"/> when the user clicks the button.</summary>
        public Brush PressedForeground
        {
            get { return (Brush)GetValue(PressedForegroundProperty); }
            set { SetValue(PressedForegroundProperty, value); }
        }
        /// <summary>Identifies the <see cref="PressedForeground"/> dependency property.</summary>
        public static readonly DependencyProperty PressedForegroundProperty = DependencyProperty.Register(nameof(PressedForeground), typeof(Brush), 
            typeof(Button), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>Gets or sets background <see cref="Brush"/> when the user clicks the button.</summary>
        public Brush PressedBackground
        {
            get { return (Brush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }
        /// <summary>Identifies the <see cref="PressedBackground"/> dependency property.</summary>
        public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register(nameof(PressedBackground), typeof(Brush), 
            typeof(Button), new PropertyMetadata(Border.BackgroundProperty.DefaultMetadata.DefaultValue));

        /// <summary>Gets or sets border <see cref="Brush"/> when the user clicks the button.</summary>
        public Brush PressedBorderBrush
        {
            get { return (Brush)GetValue(PressedBorderBrushProperty); }
            set { SetValue(PressedBorderBrushProperty, value); }
        }
        /// <summary>Identifies the <see cref="PressedBorderBrush"/> dependency property.</summary>
        public static readonly DependencyProperty PressedBorderBrushProperty = DependencyProperty.Register(nameof(PressedBorderBrush), typeof(Brush), 
            typeof(Button), new PropertyMetadata(Border.BorderBrushProperty.DefaultMetadata.DefaultValue));

        /// <summary>Gets or sets a value that represents the degree to which the corners of a <see cref="T:System.Windows.Controls.Border" /> are rounded.</summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, (object)value); }
        }
        /// <summary>Identifies the <see cref="CornerRadius"/> dependency property.</summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), 
            typeof(Button), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
    }
}
