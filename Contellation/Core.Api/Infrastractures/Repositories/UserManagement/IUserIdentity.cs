using Core.Entities.UserManagement;

namespace Core.Api.Infrastractures.Repositories.UserManagement
{
    public interface IUserIdentity
    {
        Account Identity(Account en);
    }
}
