using Products.Api.Interfaces;
using Products.Api.Models;

namespace Products.Api.Repositories
{
    public class ProductRepository(AppDbContext dbContext) : Repository<Product>(dbContext), IProductRepository
    {
       
    }
}
