namespace WebApi.Services;

public interface IPaymentMethod
{
    string Method { get; }
    string Pay(decimal amount);
}
