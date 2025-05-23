using Polly;
using ProductsApi;
using ProductsApi.Decorator;
using ProductsApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSingleton<IAsyncPolicy>(serviceProvider =>
{
    return Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
});

// builder.Services.AddDecoratorsManually();

// builder.Services.AddDecoratorsWithScrutor();

builder.Services.AddScoped<ProductService>();

builder.Services.AddScoped<IProductService>(sp =>
{
    var imple = sp.GetRequiredService<ProductService>();
    return ServiceDecoratorBuilder.Decorate<IProductService>(sp, imple, useLogging: true, useCaching: false, useRetry: true);
});

builder.Services.AddMemoryCache();

//builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(opt =>
        opt.SwaggerEndpoint("/openapi/v1.json", "OpenAPI v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
