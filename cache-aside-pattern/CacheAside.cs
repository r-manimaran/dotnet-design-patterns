namespace cache_aside_pattern;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

public static class CacheAside
{
    private static readonly DistributedCacheEntryOptions Default = new() {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
    };

    // add semaphore to prevent multiple threads from accessing the cache at the same time
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    public static async Task<T> GetOrCreateAsync<T>(
            this IDistributedCache cache,
            string key,
            Func<CancellationToken, Task<T>> factory,
            DistributedCacheEntryOptions? options = null,
            CancellationToken cancellationToken = default)
    {

        var cachedValue = await cache.GetStringAsync(key,cancellationToken);

        T? value;
        if (cachedValue is not null)
        {
            value = JsonSerializer.Deserialize<T>(cachedValue);
            if (value is not null)
            {
                return value;
            }
        }
        var hasLock = await _semaphore.WaitAsync(5000);
        if (!hasLock)
        {
            return default(T);
        }
        try
        {
            cachedValue = await cache.GetStringAsync(key,cancellationToken);
            if (cachedValue is not null)
            {
                value = JsonSerializer.Deserialize<T>(cachedValue);
                if (value is not null)
                {
                    return value;
                }
            }

            value = await factory(cancellationToken);
            if (value is not null)
            {
                var serializedValue = JsonSerializer.Serialize(value);
                await cache.SetStringAsync(key, serializedValue, options ?? Default, cancellationToken);
            }
            else {
                return default(T);
            }
        }
        finally
        {
            _semaphore.Release();
        }
       
        return value;
    }
}
