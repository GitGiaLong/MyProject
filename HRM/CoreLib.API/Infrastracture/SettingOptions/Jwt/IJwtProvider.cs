using Entities.System.Users;

namespace CoreLib.API.Infrastracture.SettingOptions.Jwt
{
    public class IJwtProvider
    {
        string GenerateToken(User user);
    }
}
