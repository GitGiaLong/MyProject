using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoreLib.API.Infrastracture.Repositories.Cache
{
    public class CacheService : ICacheService
    {
        //const string keyRedis = "UONP.API";
        private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();
        private readonly IDistributedCache _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            string cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
            if (cachedValue is null)
            {
                return default;
            }
            //value = JsonConvert.DeserializeObject<T>(cachedValue); // use Newton.Json
            T value = JsonSerializer.Deserialize<T>(cachedValue, GetJsonSerializerOptions());
            return value;
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> factory, DistributedCacheEntryOptions options = null, CancellationToken cancellationToken = default) where T : class
        {
            T cachedValue = await GetAsync<T>(key, cancellationToken);
            if (cachedValue is not null) return cachedValue;

            cachedValue = await factory();
            if (options is null)
                await SetAsync(key, cachedValue, cancellationToken: cancellationToken);
            else
                await SetAsync(key, cachedValue, options, cancellationToken);
            return cachedValue;
        }

        public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options = null, CancellationToken cancellationToken = default) where T : class
        {
            string cacheValue = JsonSerializer.Serialize(value, GetJsonSerializerOptions());
            //var bytes = Encoding.UTF8.GetBytes(cacheValue);
            if (options != null)
            {
                //nếu dùng _distributedCache.SetAsync thì lúc get ở trên, dùng _distributedCache.GetAsync
                //await _distributedCache.SetAsync(key, bytes, options, cancellationToken); 

                // do ở trên, xài GetStringAsync, nên lúc này dùng SetStringAsync
                await _distributedCache.SetStringAsync(key, cacheValue, options, cancellationToken);
            }
            else
            {
                //await _distributedCache.SetAsync(key, bytes, cancellationToken);
                await _distributedCache.SetStringAsync(key, cacheValue, cancellationToken);
            }
            CacheKeys.TryAdd(key, false);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
            CacheKeys.TryRemove(key, out bool _);
        }

        public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
        {
            /*foreach(string key in CacheKeys.Keys)
            {
                if(key.StartsWith(prefix))
                {
                    await RemoveAsync(key, cancellationToken);
                }
            }*/
            // dùng như này perfomance tốt hơn là thực thi từng cái one by one như trên!
            if (CacheKeys.Any())
            {
                IEnumerable<Task> tasks = CacheKeys.Keys
                .Where(k => k.StartsWith(prefix))
                .Select(k => RemoveAsync(k, cancellationToken));
                await Task.WhenAll(tasks);
            }
            /*else
            {
               _distributedCache.
            }*/
        }

        // https://mathieupyle.com/an-easy-way-to-drastically-improve-jsonserializer-performance-system-text-json-use-a-static-variable-for-jsonserializeroptions/
        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
                WriteIndented = true,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }
    }
}
