// Cache-Aside Pattern Example

// Service only handles data access - NO cache logic
public class ProductService : IProductService
{
    private static readonly Dictionary<int, Product> _mockDb = new()
    {
        { 1, new Product { Id=1, Name="Laptop" } },
        { 2, new Product { Id=2, Name="Mobile" } },
        { 3, new Product { Id=3, Name="Bike" } }
    };

    public async Task<Product> GetProductByIdAsync(int id)
    {
        // Service only knows about data - no cache logic
        _mockDb.TryGetValue(id, out var product);
        return product;
    }
}

// Application/Controller handles ALL cache logic
public static class ProductsEndpoints
{
    public static void MapProductsEndpoints(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/products").WithTags("Products").WithOpenApi();

        api.MapGet("/{id}", async (int id, IProductService productService, IConnectionMultiplexer redis) =>
        {
            var db = redis.GetDatabase();
            string redisKey = $"product:{id}";
            
            // 1. Application checks cache first
            string cachedData = await db.StringGetAsync(redisKey);
            
            if (!string.IsNullOrEmpty(cachedData))
            {
                var cachedProduct = JsonSerializer.Deserialize<Product>(cachedData);
                return Results.Ok(cachedProduct);
            }
            
            // 2. Cache miss - application calls service
            var product = await productService.GetProductByIdAsync(id);
            
            if (product == null)
            {
                return Results.NotFound($"Product with Id {id} not exists");
            }
            
            // 3. Application explicitly caches the result
            await db.StringSetAsync(redisKey, JsonSerializer.Serialize(product), TimeSpan.FromMinutes(5));
            
            return Results.Ok(product);
            
        }).WithName("GetProduct");
    }
}