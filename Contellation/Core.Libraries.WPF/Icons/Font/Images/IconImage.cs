using Core.Entities.Icons;
using Core.Libraries.WPF.Icons.Font.Images;
using Core.Libraries.WPF.Icons.Helpers;
using Core.Libraries.WPF.Icons.Interfaces;

namespace Core.Libraries.WPF.Icons.Font
{
    public class IconImage : IconImageBase<IconType>, IIconFont
    {
        public IconFont IconFont
        {
            get { return (IconFont)GetValue(IconFontProperty); }
            set { SetValue(IconFontProperty, value); }
        }
        public static readonly DependencyProperty IconFontProperty = DependencyProperty.Register(nameof(IconFont), typeof(IconFont), 
            typeof(IconImage), new PropertyMetadata(default(IconFont), OnIconFontPropertyChanged));

        private static void OnIconFontPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not IconImage iconImage) { return; }
            var imageSource = iconImage.ImageSourceFor(iconImage.Icon);
            iconImage.SetValue(SourceProperty, imageSource);
        }
        protected override ImageSource ImageSourceFor(IconType icon)
        {
            var size = Math.Max(IconHelper.DefaultSize, Math.Max(ActualWidth, ActualHeight));
            return icon.ToImageSource(IconFont, Foreground, size);
            ///Pro
            //return icon.WpfFontFor(IconFont).ToImageSource(icon, Foreground, size);
        }

    }
}
