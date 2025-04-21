using Microsoft.AspNetCore.Http.HttpResults;
using ProductsApi.Models;

namespace ProductsApi.Services;

public class ProductService : IProductService
{
    public async Task<Product> GetProductById(int Id)
    {
        var product  = new Product
        {
            Id = Id,
            Name = "Mobile"
        };
        return product;
    }
}
