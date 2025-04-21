using Polly;
using ProductsApi.Models;

namespace ProductsApi.Services.Decorators;

public class RetryProductServiceDecorator(IAsyncPolicy retryPolicy, IProductService inner) : IProductService
{
    public async Task<Product> GetProductById(int Id)
    {
        return await retryPolicy.ExecuteAsync(() => inner.GetProductById(Id));
    }
}
