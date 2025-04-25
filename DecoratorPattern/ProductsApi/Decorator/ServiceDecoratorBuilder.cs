using Microsoft.Extensions.Caching.Memory;
using Polly;

namespace ProductsApi.Decorator;

public static class ServiceDecoratorBuilder
{
    public static TService Decorate<TService>(
        IServiceProvider provider,
        TService implementation,
        bool useLogging = true,
        bool useRetry = true,
        bool useCaching = false)
        where TService : class
    {
        if (useRetry)
        {
            var retryPolicy = Policy.Handle<Exception>()
                                    .WaitAndRetryAsync(3, i => TimeSpan.FromMilliseconds(100 * i));
            implementation = RetryDecorator<TService>.Create(implementation, retryPolicy);
        }

        if (useCaching)
        {
            var cache = provider.GetRequiredService<IMemoryCache>();
            implementation = CachingDecorator<TService>.Create(implementation, cache);
        }

        if (useLogging)
        {
            var logger = provider.GetRequiredService<ILogger<TService>>();
            implementation = LoggingDecorator<TService>.Create(implementation, logger);
        }

        return implementation;
    }
}
