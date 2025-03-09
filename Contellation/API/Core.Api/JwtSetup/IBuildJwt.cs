using Core.Entities.UserManagement;

namespace Core.Api.JwtSetup
{
    public interface IBuildJwts
    {
        string GenerateToken(Account user);
    }
}
