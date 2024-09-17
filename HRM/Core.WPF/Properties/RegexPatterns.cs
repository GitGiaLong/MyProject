namespace Core.WPF.Properties
{
    /// <summary>
    /// Contains some strings required for regular verification
    /// </summary>
    public sealed class RegexPatterns
    {
        /// <summary> Email regular matching expression </summary>
        public const string MailPattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        /// <summary> Mobile phone number regular matching expression </summary>
        public const string PhonePattern = @"^((13[0-9])|(15[^4,\d])|(18[0,5-9]))\d{8}$";

        /// <summary> IP regular matching </summary>
        public const string IpPattern =
            @"^(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)$";

        /// <summary>
        /// Class A IP regular matching
        /// </summary>
        public const string IpAPattern =
            @"^(12[0-6]|1[0-1]\d|[1-9]?\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)$";

        /// <summary>
        /// Class B IP regular matching
        /// </summary>
        public const string IpBPattern =
            @"^(19[0-1]|12[8-9]|1[3-8]\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)$";

        /// <summary>
        /// Class C IP regular matching
        /// </summary>
        public const string IpCPattern =
            @"^(19[2-9]|22[0-3]|2[0-1]\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)$";

        /// <summary>
        /// Class D IP regular matching
        /// </summary>
        public const string IpDPattern =
            @"^(22[4-9]|23\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)$";

        /// <summary>
        /// Class E IP regular matching
        /// </summary>
        public const string IpEPattern =
            @"^(25[0-5]|24\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\."
            + @"(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)$";

        /// <summary>
        /// Url regular match
        /// </summary>
        public const string UrlPattern =
            @"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?";

        /// <summary>
        /// Number regular matching
        /// </summary>
        public const string NumberPattern = @"^\d+$";

        /// <summary>
        /// Computational properties of numerical regular matching
        /// </summary>
        public const string DigitsPattern = @"[+-]?\d*\.?\d+(?:\.\d+)?(?:[eE][+-]?\d+)?";

        /// <summary>
        /// Positive integer regular matching
        /// </summary>
        public const string PIntPattern = @"^[1-9]\d*$";

        /// <summary>
        /// Negative integer regular matching
        /// </summary>
        public const string NIntPattern = @"^-[1-9]\d*$";

        /// <summary>
        /// Integer regular matching
        /// </summary>
        public const string IntPattern = @"^-?[1-9]\d*|0$";

        /// <summary>
        /// Regular matching of non-negative integers
        /// </summary>
        public const string NnIntPattern = @"^[1-9]\d*|0$";

        /// <summary>
        /// Regular matching of non-positive integers
        /// </summary>
        public const string NpIntPattern = @"^-[1-9]\d*|0$";

        /// <summary>
        /// Positive floating point number regular matching
        /// </summary>
        public const string PDoublePattern = @"^[1-9]\d*\.\d*|0\.\d*[1-9]\d*$";

        /// <summary>
        /// Negative floating point number regular matching
        /// </summary>
        public const string NDoublePattern = @"^-([1-9]\d*\.\d*|0\.\d*[1-9]\d*)$";

        /// <summary>
        /// Floating point number regular matching
        /// </summary>
        public const string DoublePattern = @"^-?([1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0)$";

        /// <summary>
        /// Regular matching of non-negative floating point numbers
        /// </summary>
        public const string NnDoublePattern = @"^[1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0$";

        /// <summary>
        /// Regular matching of non-positive floating point numbers
        /// </summary>
        public const string NpDoublePattern = @"^(-([1-9]\d*\.\d*|0\.\d*[1-9]\d*))|0?\.0+|0$";

        /// <summary>
        /// Use reflection to get value based on property name
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object GetValue(string propertyName) => GetType().GetField(propertyName).GetValue(null);
    }
}
