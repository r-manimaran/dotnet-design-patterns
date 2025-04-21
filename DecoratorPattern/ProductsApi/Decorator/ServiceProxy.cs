using Microsoft.Extensions.Caching.Memory;
using Polly;

namespace ProductsApi.Decorator;

public class ServiceProxy<TService> where TService : class
{
    private readonly TService _service;
    private readonly ILogger<LoggingDecorator<TService>> _logger;
    private readonly IMemoryCache _cache;
    private readonly IAsyncPolicy _retryPolicy;

    public ServiceProxy(
        TService service,
        ILogger<LoggingDecorator<TService>> logger,
        IMemoryCache cache,
        IAsyncPolicy retryPolicy)
    {
        _service = service;
        _logger = logger;
        _cache = cache;
        _retryPolicy = retryPolicy;
    }

    public async Task<TResult> InvokeAsync<TResult>(
        Func<TService, Task<TResult>> operation,
        bool enableLogging = true,
        bool enableCaching = true,
        bool enableRetry = true,
        string cacheKey = null)
    {
        var service = _service;

        if (enableRetry)
        {
            var retryDecorator = new RetryDecorator<TService>(service, _retryPolicy);
            return await retryDecorator.ExecuteAsync(operation);
        }

        if (enableCaching)
        {
            var cachingDecorator = new CachingDecorator<TService>(service, _cache);
            return cachingDecorator.Execute(() => operation(service).Result, cacheKey);
        }

        if (enableLogging)
        {
            var loggingDecorator = new LoggingDecorator<TService>(service, _logger);
            return loggingDecorator.Execute(() => operation(service).Result);
        }

        return await operation(service);
    }
}

