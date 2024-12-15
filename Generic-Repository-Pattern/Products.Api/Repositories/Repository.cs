using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Products.Api.Repositories;

public abstract class Repository<TEntity> where TEntity : class
{
    private readonly AppDbContext _dbContext;

    protected Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Simple Add
    public async Task AddAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    // AddRange Async
    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().AddRange(entities);
        await _dbContext.SaveChangesAsync();
    }

    // Single Update
    public async Task UpdateAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    // Bulk Update
    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().UpdateRange(entities);
        await _dbContext.SaveChangesAsync();
    }

    // Get All
     public async Task<List<TEntity>> GetAllAsync()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    // Get By Id
    public async Task<TEntity> GetByIdAsync(int id)
    {
        var model = await _dbContext.FindAsync<TEntity>(id);
        return model!;
    }

   // Find with predicate
   public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }

    // Get with Filtering
    public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter=null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy=null,
        string includeProperties="")
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }
    

    // Check if entity exists
    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbContext.Set<TEntity>().AnyAsync(predicate);
    }

    // Count entities
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate=null)
    {
        if (predicate == null)
        {
            return await _dbContext.Set<TEntity>().CountAsync();
        }
        else
        {
            return await _dbContext.Set<TEntity>().CountAsync(predicate);
        }
    }

    // Get with pagination
    public async Task<(IEnumerable<TEntity> items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize,
        Expression<Func<TEntity, bool>>? filter=null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy=null,
        string includeProperties="")
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }
        var totalItems = await query.CountAsync();

        var items = await query
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

        return (items, totalItems);
    }

    // Get with no Tracking (for reao-only-operations)
    public async Task<IEnumerable<TEntity>> GetAllNoTrackingAsync()
    {
        return await _dbContext.Set<TEntity>()
                            .AsNoTracking()
                            .ToListAsync();
    }

    // Delete By Id
    public async void DeleteAsync(int id)
    {
        var model = await _dbContext.FindAsync<TEntity>(id);
        if (model != null)
        {
            _dbContext.Set<TEntity>().Remove(model!);
            await _dbContext.SaveChangesAsync();
        }
    }
}
