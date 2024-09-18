﻿using System.Globalization;
using System.Windows.Data;

namespace Core.WPF.Converters
{
    internal class RightSplitThicknessConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not Thickness thickness) { return default(Thickness); }

            return new Thickness(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}