using Core.WPF.Icon.Enums;
using Core.WPF.Icon.Font.Images;
using Core.WPF.Icon.Font.Interfaces;
using Core.WPF.Icon.Helpers;

namespace Core.WPF.Icon.Font
{
    public class IconImage : IconImageBase<IconChar>, IHaveIconFont
    {
        protected override ImageSource ImageSourceFor(IconChar icon)
        {
            var size = Math.Max(IconHelper.DefaultSize, Math.Max(ActualWidth, ActualHeight));
            return icon.ToImageSource(IconFont, Foreground, size);
            //return icon.WpfFontFor(IconFont).ToImageSource(icon, Foreground, size);
        }
        public static readonly DependencyProperty IconFontProperty = DependencyProperty.Register(nameof(IconFont), typeof(IconFont), typeof(IconImage),
            new PropertyMetadata(default(IconFont), OnIconFontPropertyChanged));

        private static void OnIconFontPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not IconImage iconImage) { return; }
            var imageSource = iconImage.ImageSourceFor(iconImage.Icon);
            iconImage.SetValue(SourceProperty, imageSource);
        }


        public IconFont IconFont
        {
            get => (IconFont)GetValue(IconFontProperty);
            set => SetValue(IconFontProperty, value);
        }
    }
}
