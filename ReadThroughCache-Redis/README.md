# Read-Through Cache Pattern with Redis

A .NET 9 demonstration project implementing the **Read-Through Cache design pattern** using Redis and Microsoft Aspire.

## Project Structure

```
ReadThroughCache-Redis/
├── ReadThroughCache/                    # Main API application
├── ReadThroughCache-Redis.AppHost/      # Aspire orchestration host
└── ReadThroughCache-Redis.ServiceDefaults/  # Shared service configurations
```

## Architecture

- **Product Model**: Simple entity with Id and Name
- **ProductService**: Implements read-through caching with Redis
- **ProductsEndpoints**: Minimal API with GET `/api/products/{id}`
- **Aspire Integration**: Container orchestration and observability

## Cache Patterns Explained

### Read-Through Cache (This Project)

**Who manages cache?** → Service layer handles all cache logic internally

```csharp
// Endpoint - doesn't know about cache
api.MapGet("/{id}", async (int id, IProductService service) =>
{
    return await service.GetProductByIdAsync(id); // Just asks for data
});

// Service - handles cache internally
public async Task<Product> GetProductByIdAsync(int id)
{
    // Check cache
    string cached = await _redisDb.StringGetAsync($"product:{id}");
    if (!string.IsNullOrEmpty(cached))
        return JsonSerializer.Deserialize<Product>(cached);
    
    // Cache miss - get from database and cache it
    var product = _mockDb[id];
    await _redisDb.StringSetAsync($"product:{id}", JsonSerializer.Serialize(product));
    return product;
}
```

### Cache-Aside Pattern (Alternative)

**Who manages cache?** → Application/Controller manages cache directly

```csharp
// Endpoint - explicitly manages cache
api.MapGet("/{id}", async (int id, IProductService service, IDatabase cache) =>
{
    // 1. Check cache myself
    string cached = await cache.StringGetAsync($"product:{id}");
    if (!string.IsNullOrEmpty(cached))
        return JsonSerializer.Deserialize<Product>(cached);
    
    // 2. Get from service
    var product = await service.GetFromDatabase(id);
    
    // 3. Cache it myself
    await cache.StringSetAsync($"product:{id}", JsonSerializer.Serialize(product));
    return product;
});

// Service - only knows about data, no cache logic
public async Task<Product> GetFromDatabase(int id)
{
    return _mockDb[id]; // Just data access
}
```

## Key Differences

| Aspect | Read-Through Cache | Cache-Aside |
|--------|-------------------|-------------|
| **Cache Responsibility** | Service layer | Application layer |
| **Code Location** | Inside service methods | In controllers/endpoints |
| **Transparency** | Cache is invisible to caller | Caller explicitly manages cache |
| **Separation** | Service handles data + cache | Service only handles data |

## Quick Test

- **Cache-Aside**: Remove cache code → modify controller/endpoint
- **Read-Through**: Remove cache code → only modify service

## Running the Project

1. Ensure Docker is running (for Redis)
2. Run the AppHost project
3. Access API at `/api/products/{id}`
4. Monitor Redis through RedisInsight (included via Aspire)

## Benefits of Read-Through Cache

- **Simplicity**: Calling code doesn't need cache logic
- **Consistency**: Cache loading is automatic on misses
- **Maintainability**: Cache logic centralized in service layer
- **Performance**: Transparent caching improves response times