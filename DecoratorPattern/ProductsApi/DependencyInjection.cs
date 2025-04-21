using Microsoft.Extensions.Caching.Memory;
using Polly;
using ProductsApi.Services;
using ProductsApi.Services.Decorators;

namespace ProductsApi;

public static class DependencyInjection
{
    public static IServiceCollection AddDecoratorsManually(this IServiceCollection services)
    {
        services.AddTransient<ProductService>();

        services.AddTransient<IProductService>(serviceProvider =>
        {
            
            var productService = serviceProvider.GetRequiredService<ProductService>();

            var retryPolicy = serviceProvider.GetRequiredService<IAsyncPolicy>();

            var cacheMemory = serviceProvider.GetRequiredService<IMemoryCache>();

            var logging = serviceProvider.GetRequiredService<ILogger<LoggingProductServiceDecorator>>();
            
            // Build the chain from inside out.
            // 4th 
            IProductService service = productService;
            // 3rd
            service = new RetryProductServiceDecorator(retryPolicy, service);
            // 2nd
            service = new CachingProductServiceDecorator(cacheMemory, service);
            // 1st
            service = new LoggingProductServiceDecorator(logging, service);
            
            return service;
        });

        return services;
    }

    public static IServiceCollection AddDecoratorsWithScrutor(this IServiceCollection services)
    {
        services.AddTransient<IProductService, ProductService>();

        services.Decorate<IProductService, RetryProductServiceDecorator>();

        services.Decorate<IProductService, CachingProductServiceDecorator>();

        services.Decorate<IProductService, LoggingProductServiceDecorator>();

        return services;
    }
}
