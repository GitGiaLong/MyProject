using CoreLib.API.Infrastracture.Repositories.Base;
using MongoDB.Driver;

namespace CoreLib.API.Infrastracture.Repositories.System
{
    public interface ICounterRepository : IBaseRepository<Counter>
    {
        Task<int> NextAutoIndex(string code, IClientSessionHandle session = null);
    }
}
