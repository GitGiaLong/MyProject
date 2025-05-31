using System.Globalization;
using System.Windows.Data;

namespace Libraries.Custom.Converters
{
    internal class ProgressThicknessConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is double height)
            {
                return height / 8;
            }

            return 12.0d;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
