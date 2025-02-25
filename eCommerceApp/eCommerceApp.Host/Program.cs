using eCommerceApp.Application.DI;
using eCommerceApp.Host.Endpoints;
using eCommerceApp.Host.Extensions;
using eCommerceApp.Infrastructure.DI;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("log/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

builder.Host.UseSerilog();

Log.Logger.Information("Application is building...");

builder.AddServiceDefaults();

builder.Services.AddApplicationService();

builder.Services.AddInfrastructureService(builder.Configuration);

builder.Services.AddOpenApi();

builder.Services.AddCors(builder =>
{
    builder.AddDefaultPolicy(options =>
    {
        options.AllowAnyHeader()
               .AllowAnyMethod()
               .WithOrigins("https://localhost:7825")
               .AllowCredentials();

    });
});


try
{
    var app = builder.Build();
    // cors
    app.UseCors();

    // use serilog
    app.UseSerilogRequestLogging();

    app.MapDefaultEndpoints();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint(
            "/openapi/v1.json", "OpenAPI v1");
        });

        app.UseReDoc(options =>
        {
            options.SpecUrl("/openapi/v1.json");
        });

        app.MapScalarApiReference();

        app.ApplyMigration();
    }

    app.UserInfrastructureService();

    app.UseHttpsRedirection();

    ProductEndpoints.MapProductEndpoints(app);

    CategoryEndpoints.MapCategoryEndpoints(app);

    AuthenticationEndpoints.MapAuthenticationEndpoints(app);

    PaymentMethodEndpoints.MapPaymentMethodEndpoints(app);

    Log.Logger.Information("Application is running...");

    app.Run();
}
catch(InvalidOperationException ex)
{
    Log.Logger.Error(ex, "Service Configuration error");
}
catch(Exception ex)
{
    Log.Logger.Error(ex,"Application failed to Start");
}
finally
{
    Log.CloseAndFlush();
}

