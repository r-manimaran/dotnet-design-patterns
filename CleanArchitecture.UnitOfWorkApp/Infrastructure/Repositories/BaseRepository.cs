using Domain.Common.Specificaitons;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class BaseRepository<T>(AppDbContext dbContext) : IDisposable<T>, IRepository<T> where T : class
{
    public async Task AddAsync(T entity)
    {
        await dbContext.Set<T>().AddAsync(entity);
    }

    public Task DeleteAsync(T entity)
    {
        dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await dbContext.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
    {
        return await dbContext.Set<T>().Where(filter).AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter)
    {
        return await dbContext.Set<T>().Where(filter).AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await dbContext.Set<T>().FindAsync(id);
    }

    public async Task<T> GetBySpecificationAsync(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }
    private IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        var query = dbContext.Set<T>().AsQueryable();

        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

        return query;
    }
    public Task UpdateAsync(T entity)
    {
        dbContext.Entry<T>(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }


}

public interface IDisposable<T> where T : class
{
}