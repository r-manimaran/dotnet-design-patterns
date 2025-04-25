using ProductsApi.Models;

namespace ProductsApi.Services;

public class OrderService : IOrderService
{
    public async Task<Order> GetOrderById(int id)
    {
        return new Order
        {
            Id = id,
            CustomerId = Random.Shared.Next(1, 20),
            Amount = Random.Shared.Next(100, 500)
        };
    }
}
