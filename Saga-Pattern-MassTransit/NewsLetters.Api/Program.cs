using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsLetters.Api.Database;
using NewsLetters.Api.Extensions;
using NewsLetters.Api.Messages;
using NewsLetters.Api.Services;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

//builder.AddNpgsqlDataSource("newsletters");

builder.Services.AddDatabaseContext(builder.Configuration);

builder.Services.AddMassTransitRabbitMq(builder.Configuration);

builder.Services.AddFluentEmail(builder.Configuration);

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapPost("/newsletters", async ([FromBody] string email, IBus bus) =>
{
    using Activity? activity = Activity.Current?.Source.StartActivity("SubscribeToNewsLetter");
    activity?.SetTag("EventType", "SubscribeToNewsLetter");
    

    await bus.Publish(new SubscribeToNewsletter(email));

    return Results.Accepted();
});

app.MapControllers();

app.Run();
