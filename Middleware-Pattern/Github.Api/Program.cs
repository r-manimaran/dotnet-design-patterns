using System.Reflection.Metadata.Ecma335;
using Github.Api;
using Github.Api.Handlers;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// bind the Configuration
builder.Services.AddOptions<GitHubSettings>()
    .BindConfiguration(GitHubSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Register the Delegating Handlers
builder.Services.AddTransient<RetryHandler>();
builder.Services.AddTransient<LoggingHandler>();
builder.Services.AddTransient<GitHubAuthenticationHandler>();

// Create a named HttpClient
builder.Services.AddHttpClient<GitHubService>(httpClient => {
    //builder.Services.AddHttpClient<GitHubService>((serviceProvider,httpClient) => {
    //var githubSettings = serviceProvider.GetRequiredService<IOptions<GitHubSettings>>().Value;
   
    httpClient.BaseAddress = new Uri(GitHubSettings.GitHubApiUrl);

    // Now it is comes from the Delegating Handler middleware
    //httpClient.DefaultRequestHeaders.Add("Authorization", githubSettings.AccessToken);
    //httpClient.DefaultRequestHeaders.Add("User-Agent", githubSettings.UserAgent);
})
.AddHttpMessageHandler<RetryHandler>()
.AddHttpMessageHandler<LoggingHandler>()
.AddHttpMessageHandler<GitHubAuthenticationHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapUserEndpoints();

app.Run();

