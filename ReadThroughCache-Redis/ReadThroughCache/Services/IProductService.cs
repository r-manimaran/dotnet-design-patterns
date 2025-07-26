using ReadThroughCache.Model;

namespace ReadThroughCache.Services;

public interface IProductService
{
    Task<Product> GetProductByIdAsync(int id);
}