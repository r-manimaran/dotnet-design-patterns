using Microsoft.Extensions.Caching.Memory;
using Polly;
using System.Reflection;

namespace ProductsApi.Decorator;


// Extension method for registration
public static class ServiceCollectionExtensions
{
     public static IServiceCollection AddServiceWithDecorators<TService, TImplementation>(
        this IServiceCollection services,
        Action<DecoratorBuilder<TService>> decoratorBuilder,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TService : class
        where TImplementation : class, TService
    {
        // Register the base implementation
        services.Add(new ServiceDescriptor(typeof(TImplementation), typeof(TImplementation), lifetime));

        // Register the service interface
        services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime));

        // Apply decorators using Scrutor
        var builder = new DecoratorBuilder<TService>();
        decoratorBuilder(builder);

        // Apply each decorator in the order they were added
        foreach (var decorator in builder.GetDecorators())
        {
            services.Decorate<TService>((inner, provider) => decorator(provider, inner));
        }

        return services;
    }
}

public class LoggingDispatchProxy<TService> : DispatchProxy where TService : class
{
    private LoggingDecorator<TService> _decorator;

    public object Initialize(LoggingDecorator<TService> decorator)
    {
        _decorator = decorator;
        return this;
    }

    protected override object Invoke(MethodInfo targetMethod, object[] args)
    {
        return _decorator.Execute(service =>
        {
            return targetMethod.Invoke(service, args);
        });
    }
}

public class DecoratorBuilder<TService> where TService : class
{
    private readonly List<Func<IServiceProvider, TService, TService>> _decorators = new();

    public DecoratorBuilder<TService> AddLogging()
    {
        _decorators.Add((sp, inner) =>
        {
            var logger = sp.GetRequiredService<ILogger<LoggingDecorator<TService>>>();
            var decorator = new LoggingDecorator<TService>(inner, logger);
            return (TService)DispatchProxy.Create<TService, LoggingDispatchProxy<TService>>()
                .Initialize(decorator);
        });
        return this;
    }

    public DecoratorBuilder<TService> AddCaching()
    {
        _decorators.Add((sp, inner) =>
        {
            var cache = sp.GetRequiredService<IMemoryCache>();
            return new CachingDecorator<TService>(inner, cache);
        });
        return this;
    }

    public DecoratorBuilder<TService> AddRetry()
    {
        _decorators.Add((sp, inner) =>
        {
            var retryPolicy = sp.GetRequiredService<IAsyncPolicy>();
            return new RetryDecorator<TService>(inner, retryPolicy);
        });
        return this;
    }

    // New method to expose decorators
    internal IEnumerable<Func<IServiceProvider, TService, TService>> GetDecorators()
    {
        return _decorators;
    }

    internal TService Build(IServiceProvider serviceProvider, TService implementation)
    {
        return _decorators.Aggregate(implementation,
            (current, decorator) => decorator(serviceProvider, current));
    }
}

