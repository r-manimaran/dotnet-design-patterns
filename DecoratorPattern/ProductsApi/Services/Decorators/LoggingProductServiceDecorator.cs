using ProductsApi.Models;

namespace ProductsApi.Services.Decorators;

public class LoggingProductServiceDecorator(ILogger<LoggingProductServiceDecorator> logger, 
                                            IProductService inner) : IProductService
{
    public async Task<Product> GetProductById(int Id)
    {
        logger.LogInformation($"Getting Product with Id {Id}");

        var product = await inner.GetProductById(Id);

        logger.LogInformation($"Retrieved Prodct:{product.Name}");

        return product;
    }
}
