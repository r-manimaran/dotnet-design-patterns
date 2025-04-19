namespace WebApi.Services;

public sealed class PaymentService(IEnumerable<IPaymentMethod> paymentMethods) : IPaymentService
{
    public string? Process(string paymentMethod, decimal amount)
    {
        //return paymentMethod.ToLower() switch
        //{
        //    "creditcard" => $"[CreditCard] Pay {amount:C}",
        //    "paypal" => $"[PayPal] Pay {amount:C}",
        //    _ => null
        //};

        var method = paymentMethods.FirstOrDefault(s => 
                s.Method.Equals(paymentMethod, StringComparison.OrdinalIgnoreCase));

        return method is null ?
                null :
                method.Pay(amount);
    }
}