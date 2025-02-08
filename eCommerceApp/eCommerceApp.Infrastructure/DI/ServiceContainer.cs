using eCommerceApp.Domain.Entities;
using eCommerceApp.Domain.Interfaces;
using eCommerceApp.Infrastructure.Data;
using eCommerceApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.AspNetCore.Builder;
using eCommerceApp.Infrastructure.Middlewares;

namespace eCommerceApp.Infrastructure.DI;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("eCommerce")!;
        services.AddDbContext<AppDbContext>(option =>
            option.UseSqlServer(connectionString,
            sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure();
                sqlOptions.MigrationsAssembly(typeof(ServiceContainer).Assembly.FullName);
            }).UseExceptionProcessor(),
            ServiceLifetime.Scoped);

        services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
        services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
        
        return services;
    }

    public static IApplicationBuilder UserInfrastructureService(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }

    
}
