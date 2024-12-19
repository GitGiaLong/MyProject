using System.IdentityModel.Tokens.Jwt;

namespace Core.Api.Models
{
    public class GetToken
    {
        public static JwtSecurityToken _Token = new JwtSecurityToken();
        public static JwtSecurityToken Token { get { return _Token; } set { _Token = value; } }
        public static string _Code = string.Empty;
        public static string Code { get { return _Code; } set { _Code = value; } }


        public static void Reset()
        {
            Token = new JwtSecurityToken();
            Code = string.Empty;
        }
    }
}
