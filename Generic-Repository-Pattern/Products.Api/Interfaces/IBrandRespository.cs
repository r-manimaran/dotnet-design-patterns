using Products.Api.Models;

namespace Products.Api.Interfaces;

public interface IBrandRespository
{
    Task<List<Brand>> GetAllAsync();
    Task<Brand> GetByIdAsync(int id);
    Task AddAsync(Brand brand);
    Task UpdateAsync(Brand brand);
    void DeleteAsync(int id);
}
