﻿using System.Globalization;

namespace Core.WPF.Controls.NumberBoxs
{
    /// <summary>
    /// Base nubmer formatter that uses default format specifier and <see cref="CultureInfo"/> that represents the culture used by the current thread.
    /// </summary>
    public class ValidateNumberFormatter : INumberFormatter, INumberParser
    {
        /// <inheritdoc />
        public string FormatDouble(double? value)
        {
            return value?.ToString(GetFormatSpecifier(), GetCurrentCultureConverter()) ?? string.Empty;
        }

        /// <inheritdoc />
        public string FormatInt(int? value)
        {
            return value?.ToString(GetFormatSpecifier(), GetCurrentCultureConverter()) ?? string.Empty;
        }

        /// <inheritdoc />
        public string FormatUInt(uint? value)
        {
            return value?.ToString(GetFormatSpecifier(), GetCurrentCultureConverter()) ?? string.Empty;
        }

        /// <inheritdoc />
        public double? ParseDouble(string? value)
        {
            return double.TryParse(value, out double d) ? d : null;
        }

        /// <inheritdoc />
        public int? ParseInt(string? value)
        {
            return int.TryParse(value, out int i) ? i : null;
        }

        /// <inheritdoc />
        public uint? ParseUInt(string? value)
        {
            return uint.TryParse(value, out uint ui) ? ui : (uint?)null;
        }

        private static string GetFormatSpecifier()
        {
            return "G";
        }

        private static CultureInfo GetCurrentCultureConverter()
        {
            return CultureInfo.CurrentCulture;
        }
    }
}
