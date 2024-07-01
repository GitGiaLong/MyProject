using Microsoft.Extensions.Caching.Distributed;

namespace CoreLib.API.Infrastracture.Repositories.Cache
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;
        Task<T> GetAsync<T>(string key, Func<Task<T>> factory, DistributedCacheEntryOptions options = null, CancellationToken cancellationToken = default) where T : class;
        Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options = null, CancellationToken cancellationToken = default) where T : class;

        Task RemoveAsync(string key, CancellationToken cancellationToken = default);
        Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default);
    }
}
