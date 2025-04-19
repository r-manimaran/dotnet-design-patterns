namespace WebApi.Services;

public sealed class ApplePayPayment : IPaymentMethod
{
    public string Method => "ApplePay";

    public string Pay(decimal amount) => $"[{Method}] Pay {amount:C}";

}

