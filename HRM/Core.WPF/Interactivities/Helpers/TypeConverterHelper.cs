using System.ComponentModel;
using System.Globalization;

namespace Core.WPF.Interactivities
{
    internal static class TypeConverterHelper
    {
        internal static object DoConversionFrom(TypeConverter converter, object value)
        {
            object returnValue = value;

            try
            {
                if (converter != null && value != null && converter.CanConvertFrom(value.GetType()))
                {
                    returnValue = converter.ConvertFrom(context: null, culture: CultureInfo.InvariantCulture, value: value);
                }
            }
            catch (Exception e)
            {
                if (!ShouldEatException(e))
                {
                    throw;
                }
            }

            return returnValue;
        }

        private static bool ShouldEatException(Exception e)
        {
            bool shouldEat = false;

            if (e.InnerException != null)
            {
                shouldEat |= ShouldEatException(e.InnerException);
            }

            shouldEat |= e is FormatException;
            return shouldEat;
        }

        internal static TypeConverter GetTypeConverter(Type type)
        {
            return TypeDescriptor.GetConverter(type);
        }
    }
}
