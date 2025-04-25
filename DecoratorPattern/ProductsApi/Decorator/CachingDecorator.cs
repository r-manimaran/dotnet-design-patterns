using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

namespace ProductsApi.Decorator;

public class CachingDecorator<TService> : DispatchProxy where TService : class
{
    private TService _decorated;
    private IMemoryCache _cache;

    public static TService Create(TService decorated, IMemoryCache cache)
    {
        var proxy = Create<TService, CachingDecorator<TService>>();
        ((CachingDecorator<TService>)(object)proxy).SetParameters(decorated, cache);
        return proxy;
    }

    private void SetParameters(TService decorated, IMemoryCache cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    /*protected override object Invoke(MethodInfo targetMethod, object[] args)
    {
        var returnType = targetMethod.ReturnType;

        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            var key = $"{targetMethod.Name}_{string.Join("_", args)}";

            if (_cache.TryGetValue(key, out var cachedResult))
            {
                return cachedResult;
            }

            var resultTask = (Task)targetMethod.Invoke(_decorated, args);
            var genericResultType = returnType.GetGenericArguments()[0];

            return Task.Run(async () =>
            {
                try
                {
                    var result = await ((dynamic)resultTask);
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                    
                    _cache.Set(key, Task.FromResult(result), cacheEntryOptions);
                    return result;
                }
                catch (Exception ex)
                {
                    // Log the exception if needed
                    throw;
                }
            });
        }

        return targetMethod.Invoke(_decorated, args);
    }*/
    protected override object Invoke(MethodInfo targetMethod, object[] args)
    {
        var returnType = targetMethod.ReturnType;

        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            var key = $"{targetMethod.Name}_{string.Join("_", args)}";

            if (_cache.TryGetValue(key, out var cachedResult))
            {
                return cachedResult;
            }

            var resultTask = (Task)targetMethod.Invoke(_decorated, args);
            var genericResultType = returnType.GetGenericArguments()[0];

            async Task<object> GetResultAsync()
            {
                var result = await ((dynamic)resultTask);
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                var taskResult = Task.FromResult((object)result);
                _cache.Set(key, taskResult, cacheEntryOptions);

                return result;
            }

            return GetResultAsync();
        }

        return targetMethod.Invoke(_decorated, args);
    }

}