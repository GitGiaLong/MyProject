using Core.Entities.Applications.Base.Filters;
using Core.Entities.Applications.Connects.Server;
using Core.Entities.Managements.User;

namespace Core.API.Infrastracture.Repositories.UserManagement
{
    public interface IUserIdentity
    {
        User Identity(User en);
    }
}
