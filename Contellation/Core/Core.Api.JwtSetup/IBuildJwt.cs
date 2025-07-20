using Core.Entities.UserManagement;

namespace Core.Api.JwtSetup
{
    public interface IBuildJwt
    {
        string GenerateToken(Account user);
    }
}
