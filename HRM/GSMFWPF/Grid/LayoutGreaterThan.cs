using System.Globalization;
using System.Windows.Data;

namespace GSMFWPF.Grid
{
    public class LayoutGreaterThan : IValueConverter
    {
        public static readonly IValueConverter Instance = new LayoutGreaterThan();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double doubleValue = System.Convert.ToDouble(value);
            double compareToValue = System.Convert.ToDouble(parameter);

            return doubleValue > compareToValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
