using eCommerceApp.Application.DI;
using eCommerceApp.Host.Endpoints;
using eCommerceApp.Host.Extensions;
using eCommerceApp.Infrastructure.DI;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddApplicationService();

builder.Services.AddInfrastructureService(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint(
        "/openapi/v1.json", "OpenAPI v1");
    });

    app.UseReDoc(options => {
        options.SpecUrl("/openapi/v1.json");
    });

    app.MapScalarApiReference();

    app.ApplyMigration();
}

app.UseHttpsRedirection();

ProductEndpoints.MapProductEndpoints(app);

CategoryEndpoints.MapCategoryEndpoints(app);

app.Run();


