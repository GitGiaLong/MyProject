namespace Core.Libraries.WPF.Controls.NumberBoxs
{
    /// <summary>
    /// An interface that parses a string representation of a numeric value.
    /// </summary>
    public interface INumberParser
    {
        /// <summary>
        /// Attempts to parse a string representation of a <see cref="double"/> numeric value.
        /// </summary>
        double? ParseDouble(string value);

        /// <summary>
        /// Attempts to parse a string representation of an <see cref="int"/> numeric value.
        /// </summary>
        int? ParseInt(string value);

        /// <summary>
        /// Attempts to parse a string representation of an <see cref="uint"/> numeric value.
        /// </summary>
        uint? ParseUInt(string value);
    }
}
