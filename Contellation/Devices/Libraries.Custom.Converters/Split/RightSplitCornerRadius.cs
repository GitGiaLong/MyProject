using System.Globalization;
using System.Windows.Data;

namespace Libraries.Custom.Converters.Split
{

    internal class RightSplitCornerRadius : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not CornerRadius cornerRadius)
            {
                return default(CornerRadius);
            }

            return new CornerRadius(0, cornerRadius.TopRight, cornerRadius.BottomRight, 0);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
