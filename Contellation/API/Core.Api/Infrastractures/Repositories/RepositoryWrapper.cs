using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Api.Infrastractures.Repositories.UserManagement;
using Core.Libraries.Connects;

namespace Core.Api.Infrastractures.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        public readonly IConnectServers _connectServers;
        public RepositoryWrapper(IConnectServers connectServers) 
        {
            _connectServers = connectServers;
        }

        public IUserIdentity UserIdentity { get; set; } = new UserIdentity();
    }
}
