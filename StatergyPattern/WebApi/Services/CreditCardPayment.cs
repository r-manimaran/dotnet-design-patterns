namespace WebApi.Services;

public sealed class CreditCardPayment : IPaymentMethod
{
    public string Method => "CreditCard";

    public string Pay(decimal amount) => $"[{Method}] Pay {amount:C}";
   
}
