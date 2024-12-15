using Products.Api.Interfaces;
using Products.Api.Models;

namespace Products.Api.Repositories;

public class BrandRepository(AppDbContext context) : Repository<Brand>(context), IBrandRespository
{
   
}
