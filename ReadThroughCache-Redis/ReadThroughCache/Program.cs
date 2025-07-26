using ReadThroughCache.Endpoints;
using ReadThroughCache.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRedisClient("cache");

//builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
//    ConnectionMultiplexer.Connect("localhost:6379"));

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/openapi/v1.json", "OpenApi v1"));

app.UseHttpsRedirection();

app.MapProductsEndpoints();

app.Run();

