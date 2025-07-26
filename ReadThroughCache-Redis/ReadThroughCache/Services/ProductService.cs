using ReadThroughCache.Model;
using StackExchange.Redis;
using System.Text.Json;

namespace ReadThroughCache.Services;

public class ProductService : IProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly IDatabase _redisDb;
    private static readonly Dictionary<int, Product> _mockDb = new()
    {
        { 1, new Product { Id=1, Name="Laptop" } },
        { 2, new Product { Id=1, Name="Mobile" } },
        { 3, new Product { Id=1, Name="Bike" } }
    };

    public ProductService(IConnectionMultiplexer redis, ILogger<ProductService> logger)
    {
        _redisDb = redis.GetDatabase();
        _logger = logger;
    }
    public async Task<Product> GetProductByIdAsync(int id)
    {
        string redisKey = $"product:{id}";

        string cahcedData = await _redisDb.StringGetAsync(redisKey);

        if(!string.IsNullOrEmpty(cahcedData))
        {
            return JsonSerializer.Deserialize<Product>(cahcedData);
        }

        _mockDb.TryGetValue(id, out var product);

        if(product != null)
        {
            await _redisDb.StringSetAsync(redisKey,JsonSerializer.Serialize(product),TimeSpan.FromMinutes(5));
        }
        return product;
    }

}
