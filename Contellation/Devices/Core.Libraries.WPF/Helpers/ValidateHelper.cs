namespace Core.Libraries.WPF.Helpers
{
    /// <summary>
    /// Validation helper
    /// </summary>
    public class ValidateHelper
    {
        /// <summary>
        /// Whether it is within the range of floating point numbers
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInRangeOfDouble(object value)
        {
            var v = (double)value;
            return !(double.IsNaN(v) || double.IsInfinity(v));
        }

        /// <summary>
        /// Whether it is within the range of positive floating point numbers
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInRangeOfPosDouble(object value)
        {
            var v = (double)value;
            return !(double.IsNaN(v) || double.IsInfinity(v)) && v > 0;
        }

        /// <summary>
        /// Whether it is within the range of positive floating point numbers (including 0)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInRangeOfPosDoubleIncludeZero(object value)
        {
            var v = (double)value;
            return !(double.IsNaN(v) || double.IsInfinity(v)) && v >= 0;
        }

        /// <summary>
        /// Whether it is within the range of negative floating point numbers
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInRangeOfNegDouble(object value)
        {
            var v = (double)value;
            return !(double.IsNaN(v) || double.IsInfinity(v)) && v < 0;
        }

        /// <summary>
        /// Whether it is within the range of negative floating point numbers (including 0)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInRangeOfNegDoubleIncludeZero(object value)
        {
            var v = (double)value;
            return !(double.IsNaN(v) || double.IsInfinity(v)) && v <= 0;
        }

        /// <summary>
        /// Whether it is within the range of positive integers
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInRangeOfPosInt(object value)
        {
            var v = (int)value;
            return v > 0;
        }

        /// <summary>
        /// Whether it is within the range of positive integers (including 0)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInRangeOfPosIntIncludeZero(object value)
        {
            var v = (int)value;
            return v >= 0;
        }

        /// <summary>
        /// Whether it is within the range of negative integers
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInRangeOfNegInt(object value)
        {
            var v = (int)value;
            return v < 0;
        }

        /// <summary>
        /// Whether it is within the range of negative integers (including 0)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInRangeOfNegIntIncludeZero(object value)
        {
            var v = (int)value;
            return v <= 0;
        }
    }
}
