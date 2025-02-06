namespace eCommerceApp.Domain.Interfaces;

public interface IGenericRepository<TEntity> 
    where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(Guid id);
    Task<int> AddAsync(TEntity entity);
    Task<int> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(Guid id);
}
