namespace Core.Api.Models
{
    public class JsonWebToken
    {
        //private static string _SecretKey = string.Empty;
        private static string _SecretKey = "ContellationWebApiForDesktopWebMobileAndSecretKeyIsDemoVersion:1.0";
        /// <summary>
        /// Khóa bí mật
        /// </summary>
        public string SecretKey { get { return _SecretKey; } set { _SecretKey = value; } }

        //private static string _Issuer = string.Empty;
        private static string _Issuer = "Contellation_AdminSSR";
        /// <summary>
        /// Nhà phát hành
        /// </summary>
        public string Issuer { get { return _Issuer; } set { _Issuer = value; } }

        //private static string _Audience = string.Empty;
        private static string _Audience = "Management_HRM";
        /// <summary>
        /// Audience
        /// </summary>
        public string Audience { get { return _Audience; } set { _Audience = value; } }

    }
}
