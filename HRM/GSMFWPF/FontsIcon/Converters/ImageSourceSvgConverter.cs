﻿using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace GSMFWPF.FontsIcon.Converters
{
    public class ImageSourceSvgConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not EFontAwesomeIcon)
            {
                return null;
            }

            if (parameter is not Brush brush)
            {
                brush = Brushes.Black;
            }

            return ((EFontAwesomeIcon)value).CreateImageSource(brush, new Pen());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
