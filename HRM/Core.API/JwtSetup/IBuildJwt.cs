using Core.Entities.Managements.User;

namespace Core.API.JwtSetup
{
    public interface IBuildJwt
    {
        string GenerateToken(User user);
    }
}
