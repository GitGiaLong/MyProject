using Core.Api.JwtSetup;
using Core.Entities.Connects;
using Core.Entities.UserManagement;

namespace Core.Api.Infrastractures.Repositories.UserManagement
{
    public interface IUserIdentity
    {
        //Account Identity(Account en);
        object Identity(object ens, IBuildJwt _jwtProvider);
    }
}
