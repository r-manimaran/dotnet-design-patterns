using Microsoft.Extensions.Caching.Memory;
using Polly;

namespace ProductsApi.Decorator;


// Extension method for registration
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDecoratedService<TService, TImplementation>(
        this IServiceCollection services )
        where TService : class
        where TImplementation : class, TService
    {
        
        services.AddTransient<TImplementation>();
        services.AddTransient<TService>(sp =>
        {
             var baseService = sp.GetRequiredService<TImplementation>();
            var logger = sp.GetRequiredService<ILogger<LoggingDecorator<TService>>>();
            var cache = sp.GetRequiredService<IMemoryCache>();
            var retryPolicy = sp.GetRequiredService<IAsyncPolicy>();

             // Build the decorator chain from inside out
            TService service = baseService;

            // Add retry decorator
            service = new RetryDecorator<TService>(service, retryPolicy);

            // Add caching decorator
            service = new CachingDecorator<TService>(service, cache);

            // Add logging decorator (outermost)
            service = new LoggingDecorator<TService>(service, logger);

            return service;
        });

        return services;
    }
}
public class DecoratorBuilder<TService> where TService : class
{
    private readonly List<Func<IServiceProvider, TService, TService>> _decorators = new();

    public DecoratorBuilder<TService> WithLogging()
    {
        _decorators.Add((sp, inner) =>
        {
            var logger = sp.GetRequiredService<ILogger<LoggingDecorator<TService>>>();
            return new LoggingDecorator<TService>(inner, logger).Execute;
        });
        return this;
    }

    public DecoratorBuilder<TService> WithCaching()
    {
        _decorators.Add((sp, inner) =>
        {
            var cache = sp.GetRequiredService<IMemoryCache>();
            return new CachingDecorator<TService>(inner, cache).Execute;
        });
        return this;
    }

    public DecoratorBuilder<TService> WithRetry()
    {
        _decorators.Add((sp, inner) =>
        {
            var retryPolicy = sp.GetRequiredService<IAsyncPolicy>();
            return new RetryDecorator<TService>(inner, retryPolicy).ExecuteAsync;
        });
        return this;
    }

    internal TService Build(IServiceProvider serviceProvider, TService implementation)
    {
        return _decorators.Aggregate(implementation,
            (current, decorator) => decorator(serviceProvider, current));
    }
}

