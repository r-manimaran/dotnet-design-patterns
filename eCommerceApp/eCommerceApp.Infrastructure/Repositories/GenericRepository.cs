using eCommerceApp.Application.Exceptions;
using eCommerceApp.Domain.Interfaces;
using eCommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Infrastructure.Repositories;

public class GenericRepository<TEntity>(AppDbContext context) : IGenericRepository<TEntity>
    where TEntity : class
{
    public async Task<int> AddAsync(TEntity entity)
    {
        context.Set<TEntity>().Add(entity);
        return await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await context.Set<TEntity>().FindAsync(id);
        if (entity == null)
        {
            throw new ItemNotFoundException($"{typeof(TEntity).Name} with Id {id} not found");          
        }
        context.Set<TEntity>().Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var items = await context.Set<TEntity>().AsNoTracking().ToListAsync();
        return items;
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
       var result = await context.Set<TEntity>().FindAsync(id);
       if (result == null)
       {
           throw new ItemNotFoundException($"{typeof(TEntity).Name} with Id {id} not found");
       }
       return result;
    }

    public async Task<int> UpdateAsync(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        return await context.SaveChangesAsync();
    }
}
