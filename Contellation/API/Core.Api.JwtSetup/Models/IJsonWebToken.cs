using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Api.JwtSetup.Models
{
    public interface IJsonWebToken
    {
        /// <summary>
        /// Khóa bí mật
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Nhà phát hành
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Audience
        /// </summary>
        public string Audience { get; set; }
    }
}
