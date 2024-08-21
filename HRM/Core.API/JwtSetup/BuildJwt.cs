﻿using Core.Entities.Applications.Jwt;
using Core.Entities.Managements.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.API.JwtSetup
{
    public class BuildJwt : IBuildJwt
    {
        private readonly JsonWebToken _options;

        public BuildJwt(IOptions<JsonWebToken> options)
        {
            _options = options.Value;
        }

        public string GenerateToken(User user)
        {
            string key = _options.SecretKey;
            byte[] secretKey = Encoding.ASCII.GetBytes(key);
            string token;
            JwtSecurityToken JWToken = new(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: ClaimValues(user),
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Today.AddDays(2)).DateTime,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            );
            token = new JwtSecurityTokenHandler().WriteToken(JWToken);

            return token;
        }
        private static Claim[] ClaimValues(User user)
        {
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username)
            };
            return claims;
        }
    }
}