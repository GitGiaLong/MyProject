using CoreLib.API.Infrastracture.Repositories.Base;
using CoreLib.API.Infrastracture.Repositories.DbContext;
using Entities.System.Users;
using Microsoft.AspNetCore.Http;

namespace CoreLib.API.Infrastracture.Repositories.Identity
{
    public class LoginRepository : BaseRepository<User>, ILoginRepository
    {
        public LoginRepository(IMongoContext context, IHttpContextAccessor accessor) : base(context, accessor)
        {
        }
    }
}
