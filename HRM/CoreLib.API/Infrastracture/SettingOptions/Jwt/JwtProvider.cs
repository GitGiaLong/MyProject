using Entities.Application.Jwt;
using Entities.System.StaticClaim;
using Entities.System.Users;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoreLib.API.Infrastracture.SettingOptions.Jwt
{
    public class JwtProvider
    {

        private readonly JwtOptions _options;
        public JwtProvider(IOptions<JwtOptions> options)
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
                claims: GenerateUserClaims(user),
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Today.AddDays(2)).DateTime,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            );
            token = new JwtSecurityTokenHandler().WriteToken(JWToken);

            return token;
        }

        private static IEnumerable<Claim> GenerateUserClaims(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Username),
                //new Claim(StaticClaimTypes.CompanyId, user.CompanyId),
                new Claim(StaticClaimTypes.KeyApp, user.ListKeyApp is not null && user.ListKeyApp.Any() ? string.Join(",",user.ListKeyApp) : string.Empty),
                new Claim(StaticClaimTypes.AccessLevel, user.AccessLevel.ToString())
            };
            return claims;
        }
    }
}
