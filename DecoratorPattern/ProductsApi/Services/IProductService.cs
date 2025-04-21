using ProductsApi.Models;

namespace ProductsApi.Services;

public interface IProductService
{
    Task<Product> GetProductById(int Id);
}
