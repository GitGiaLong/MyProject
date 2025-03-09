namespace Core.Libraries.WPF.Controls
{
    /// <summary>
    /// Ala Pa**one color card.
    /// </summary>
    public class CardColor : System.Windows.Controls.Control
    {
        /// <summary>
        /// Gets or sets the main text displayed below the color.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string),
            typeof(CardColor), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets text displayed under main <see cref="Title"/>.
        /// </summary>
        public string Subtitle
        {
            get { return (string)GetValue(SubtitleProperty); }
            set { SetValue(SubtitleProperty, value); }
        }
        public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(nameof(Subtitle), typeof(string),
            typeof(CardColor), new PropertyMetadata(string.Empty, OnSubtitleChanged));

        /// <summary>
        /// Gets or sets the font size of <see cref="Subtitle"/>.
        /// </summary>
        public double SubtitleFontSize
        {
            get { return (double)GetValue(SubtitleFontSizeProperty); }
            set { SetValue(SubtitleFontSizeProperty, value); }
        }
        public static readonly DependencyProperty SubtitleFontSizeProperty = DependencyProperty.Register(nameof(SubtitleFontSize), typeof(double),
            typeof(CardColor), new PropertyMetadata(11.0d));

        /// <summary>
        /// Gets or sets the displayed <see cref="CardBrush"/>.
        /// </summary>
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color),
            typeof(CardColor), new PropertyMetadata(Color.FromArgb(0, 0, 0, 0), OnColorChanged));

        /// <summary>
        /// Gets or sets the displayed <see cref="CardBrush"/>.
        /// </summary>
        public Brush Brush
        {
            get { return (Brush)GetValue(BrushProperty); }
            set { SetValue(BrushProperty, value); }
        }
        public static readonly DependencyProperty BrushProperty = DependencyProperty.Register(nameof(Brush), typeof(Brush),
            typeof(CardColor), new PropertyMetadata(Brushes.Transparent, OnBrushChanged));

        /// <summary>
        /// Gets the <see cref="System.Windows.Media.Brush"/> displayed in <see cref="CardColor"/>.
        /// </summary>
        public Brush CardBrush
        {
            get { return (Brush)GetValue(CardBrushProperty); }
            internal set { SetValue(CardBrushProperty, value); }
        }
        public static readonly DependencyProperty CardBrushProperty = DependencyProperty.Register(nameof(CardBrush), typeof(Brush),
            typeof(CardColor), new PropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// Virtual method triggered when <see cref="Subtitle"/> is changed.
        /// </summary>
        protected virtual void OnSubtitlePropertyChanged() { }

        /// <summary>
        /// Virtual method triggered when <see cref="Color"/> is changed.
        /// </summary>
        protected virtual void OnColorPropertyChanged()
        {
            SetCurrentValue(CardBrushProperty, new SolidColorBrush(Color));
        }

        /// <summary>
        /// Virtual method triggered when <see cref="Brush"/> is changed.
        /// </summary>
        protected virtual void OnBrushPropertyChanged()
        {
            SetCurrentValue(CardBrushProperty, Brush);
        }

        private static void OnSubtitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not CardColor cardColor) { return; }

            cardColor.OnSubtitlePropertyChanged();
        }

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not CardColor cardColor) { return; }

            cardColor.OnColorPropertyChanged();
        }

        private static void OnBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not CardColor cardColor) { return; }

            cardColor.OnBrushPropertyChanged();
        }
    }
}
