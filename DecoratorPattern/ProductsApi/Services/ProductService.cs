using Microsoft.AspNetCore.Http.HttpResults;
using ProductsApi.Models;
using System;

namespace ProductsApi.Services;

public class ProductService : IProductService
{
    private readonly Random _random = new Random();
    public async Task<Product> GetProductById(int Id)
    {
        if (_random.NextDouble() < 0.7) // 70% chance of failure
        {
            throw new HttpRequestException("Random failure!");
        }
        var product  = new Product
        {
            Id = Id,
            Name = "Mobile"
        };
        return product;
    }
}
