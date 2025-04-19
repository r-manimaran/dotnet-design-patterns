namespace WebApi.Services;

public interface IPaymentService
{
    string? Process(string paymentMethod, decimal amount);
}
