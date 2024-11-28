using Weather_api.Services;
using Scalar.AspNetCore;
using Scrutor;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();
builder.Services.AddLogging();

/** Implementation 1 **/
/*
builder.Services.AddScoped<WeatherService>();
builder.Services.AddScoped<IWeatherService>(provider =>
{
    var weatherService = provider.GetService<WeatherService>()!;
    return new CachedWeatherService(weatherService,
                                    provider.GetService<IMemoryCache>()!,
                                    provider.GetService<ILogger<CachedWeatherService>>()!);
});*/

/** Implementation 2 with Scrutor **/
builder.Services.AddScoped<IWeatherService, WeatherService>();

//using Scrutor Decorate
builder.Services.Decorate<IWeatherService,CachedWeatherService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
