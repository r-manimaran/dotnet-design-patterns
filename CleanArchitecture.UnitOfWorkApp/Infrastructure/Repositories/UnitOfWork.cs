using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly Dictionary<Type, object> customRepositoryList;
    
     private bool _disposed;
     private readonly AppDbContext context;
     public UnitOfWork(AppDbContext dbContext)
     {
        context = dbContext ?? throw new ArgumentNullException(nameof(context));
        
        customRepositoryList = new Dictionary<Type, object>();

     }
    public IRepository<T> GetRepository<T>() where T : class
    {
        if(customRepositoryList.TryGetValue(typeof(T), out object? repository))
        {
            return (IRepository<T>)repository;    
        }
        // create new Repository if it doesn't exist
        var newRepository = new BaseRepository<T>(context);
        customRepositoryList.Add(typeof(T), newRepository);
        return newRepository;
    }

    public async Task SaveChangesAsync()
    {
        using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            if(transaction.GetDbTransaction().Connection !=null)
                await transaction.RollbackAsync();

            throw ;
        }
        finally{
            await transaction.DisposeAsync();
        }
    }

    public void SaveChanges()
    {
        using var transaction = context.Database.BeginTransaction();
        try
        {
            int id = context.SaveChanges(); 
            transaction.Commit();      
        }
        catch (Exception ex)
        {
            if(transaction.GetDbTransaction().Connection !=null)
                transaction.Rollback();
            throw ;
        }
    }

    public void Dispose()
    {
       _disposed = true;
    }

}
