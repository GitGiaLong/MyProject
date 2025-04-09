using System.Globalization;
using System.Windows.Data;

namespace Libraries.Custom.Converters.Split
{

    internal class LeftSplitCornerRadius : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not CornerRadius cornerRadius)
            {
                return default(CornerRadius);
            }

            return new CornerRadius(cornerRadius.TopLeft, 0, 0, cornerRadius.BottomLeft);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
