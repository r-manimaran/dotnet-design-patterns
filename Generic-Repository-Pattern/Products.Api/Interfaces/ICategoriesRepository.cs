using Products.Api.Models;

namespace Products.Api.Interfaces;

public interface ICategoriesRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category> GetByIdAsync(int id);
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    void DeleteAsync(int id);
}
