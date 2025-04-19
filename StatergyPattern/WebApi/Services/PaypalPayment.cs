namespace WebApi.Services;

public sealed class PaypalPayment : IPaymentMethod
{
    public string Method => "Paypal";

    public string Pay(decimal amount) => $"[{Method}] Pay {amount:C}";

}

