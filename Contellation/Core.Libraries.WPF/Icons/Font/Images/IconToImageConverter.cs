using Core.Entities.Icons;
using Core.Libraries.WPF.Icons.Interfaces;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Core.Libraries.WPF.Icons.Font
{
    [ValueConversion(typeof(IconType), typeof(Image))]
    public class IconToImageConverter : IValueConverter, IIconFont
    {
        public Brush Foreground { get; set; }
        public Style ImageStyle { get; set; }
        public IconFont IconFont { get; set; } = IconFont.Auto;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var icon = (IconType)value;
            var image = new IconImage
            {
                Icon = icon,
                IconFont = IconFont
            };

            if (Foreground != null) { image.Foreground = Foreground; }

            if (ImageStyle != null) { image.Style = ImageStyle; }

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return null; }
    }
}
