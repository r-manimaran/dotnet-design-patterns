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
using eCommerceApp.Application.Services.Interfaces.Logging;
using eCommerceApp.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using eCommerceApp.Domain.Interfaces.Authentication;
using eCommerceApp.Infrastructure.Repositories.Authentication;
using eCommerceApp.Domain.Interfaces.Cart;
using eCommerceApp.Infrastructure.Repositories.Cart;
using eCommerceApp.Application.Services.Interfaces.Cart;

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
        services.AddScoped(typeof(IAppLogger<>), typeof(SerilogLoggerAdaptor<>));
        
        services.AddDefaultIdentity<AppUser>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
            options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            options.Password.RequireDigit = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredUniqueChars = 1;
        })
         .AddRoles<IdentityRole>()
         .AddEntityFrameworkStores<AppDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!))

            };
        });

        services.AddScoped<IUserManagement, UserManagement>();
        services.AddScoped<ITokenManagement, TokenManagement>();
        services.AddScoped<IRoleManagement, RoleManagement>();
        services.AddScoped<IPaymentMethod, PaymentMethodRepository>();
        services.AddScoped<IPaymentService, StripePaymentService>();

        return services;
    }

    public static IApplicationBuilder UserInfrastructureService(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }

    
}
