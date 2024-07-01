using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.API.Infrastracture.Repositories.Wrapper
{
    public interface IRepositoryWrapper : IDisposable
    {
        Task<bool> CommitAsync();
        Task<bool> CommitAsyncTransaction(IClientSessionHandle session);
    }
}
