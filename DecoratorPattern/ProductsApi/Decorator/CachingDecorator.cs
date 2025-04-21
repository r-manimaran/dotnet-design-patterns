using Microsoft.Extensions.Caching.Memory;

namespace ProductsApi.Decorator;

public class CachingDecorator<TService>: Decorator<TService> where TService: class
{

    private readonly IMemoryCache _cache;

    public CachingDecorator(TService inner, IMemoryCache cache): base(inner)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public TResult Execute<TResult>(Func<TService, TResult> operation, string cacheKey)
    {
        if (_cache.TryGetValue(cacheKey, out TResult cachedResult))
        {
            return cachedResult;
        }

        var result = operation(Inner);
        _cache.Set(cacheKey, result);
        return result;
    }
}
