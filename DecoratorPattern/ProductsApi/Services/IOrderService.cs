using ProductsApi.Models;

namespace ProductsApi.Services;

public interface IOrderService
{
    Task<Order> GetOrderById(int id);
}
