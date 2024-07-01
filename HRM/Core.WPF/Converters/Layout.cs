using System.Globalization;
using System.Windows.Data;

namespace Core.WPF.Converters;

public class LessThan : IValueConverter
{
    public static readonly IValueConverter Instance = new LessThan();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double doubleValue = System.Convert.ToDouble(value);
        double compareToValue = System.Convert.ToDouble(parameter);

        return doubleValue < compareToValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class GreaterThan : IValueConverter
{
    public static readonly IValueConverter Instance = new GreaterThan();

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