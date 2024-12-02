using jokes.Api.Services;
using Polly;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Incase the Api calls fails for 3 attempts, it will wait for 2 Mins and then try again

builder.Services.AddHttpClient<IJokesService, JokesService>(client =>
{
    client.BaseAddress = new Uri("https://official-joke-api.appspot.com/random_joke");
})
.AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(3,TimeSpan.FromMilliseconds(12000)));

builder.Services.AddScoped<IJokesService, JokesService>();

builder.Services.AddOpenApi();

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
