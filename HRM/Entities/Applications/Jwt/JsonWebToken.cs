using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Applications.Jwt
{
    public class JsonWebToken
    {
        private static string _SecretKey = string.Empty;
        /// <summary>
        /// Khóa bí mật
        /// </summary>
        public string SecretKey { get { return _SecretKey; } set { _SecretKey = value;  } }

        private static string _Issuer = string.Empty;
        /// <summary>
        /// Nhà phát hành
        /// </summary>
        public string Issuer { get { return _Issuer; } set { _Issuer = value;  } }

        private static string _Audience = string.Empty;
        /// <summary>
        /// Audience
        /// </summary>
        public string Audience { get { return _Audience; } set { _Audience = value;  } }

    }
}
