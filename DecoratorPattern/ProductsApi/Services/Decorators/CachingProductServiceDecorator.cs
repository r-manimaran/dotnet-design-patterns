using Microsoft.Extensions.Caching.Memory;
using ProductsApi.Models;

namespace ProductsApi.Services.Decorators;

public class CachingProductServiceDecorator(IMemoryCache cache, 
                                     IProductService inner) : IProductService
{
    public async Task<Product> GetProductById(int Id)
    {
       if(cache.TryGetValue(Id,out Product product))
        {
            Console.WriteLine("Returning product from cache...");
            
            return product;
        }

        product = await inner.GetProductById(Id);

        cache.Set(Id, product);

        return product;
    }
}
