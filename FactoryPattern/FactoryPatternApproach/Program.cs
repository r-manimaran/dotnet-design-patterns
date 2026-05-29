using FactoryPatternApproach.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped<INotificationService, EmailService>();
builder.Services.AddScoped<INotificationService, SmsService>();
builder.Services.AddScoped<INotificationService, PushNotificationService>();

// Register Factory
builder.Services.AddSingleton<INotificationFactory, NotificationFactory>();

// Loggig
builder.Services.AddLogging();

// Using Keyed Services
/*
builder.Services.AddKeyedSingleton<INotificationService, EmailService>("email");
builder.Services.AddKeyedSingleton<INotificationService, SmsService>("sms");
builder.Services.AddKeyedSingleton<INotificationService, PushNotificationService>("push");
builder.Services.AddSingleton<INotificationFactory, KeyedNotificationFactory>();
*/

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
