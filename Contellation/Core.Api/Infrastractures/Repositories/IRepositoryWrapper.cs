using Core.Api.Infrastractures.Repositories.UserManagement;

namespace Core.Api.Infrastractures.Repositories
{
    public interface IRepositoryWrapper
    {
        public IUserIdentity UserIdentity { get; set; }
    }
}
