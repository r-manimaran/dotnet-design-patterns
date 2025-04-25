namespace ProductsApi.Models;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public  decimal Amount { get; set; }
}
