using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped<IPaymentMethod, CreditCardPayment>();

builder.Services.AddScoped<IPaymentMethod, PaypalPayment>();

builder.Services.AddScoped<IPaymentMethod, ApplePayPayment>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/pay", (string method, decimal amount, IPaymentService paymentService) =>
{
    string? result = paymentService.Process(method, amount);

    return result == null ? 
            Results.BadRequest($"Invalid Payment method :{method}") :
            Results.Ok(result);
});

app.Run();
