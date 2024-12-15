using Products.Api.Interfaces;
using Products.Api.Models;

namespace Products.Api.Repositories;
public class CategoriesRepository(AppDbContext dbContext) : Repository<Category>(dbContext), ICategoriesRepository
{
   

}
