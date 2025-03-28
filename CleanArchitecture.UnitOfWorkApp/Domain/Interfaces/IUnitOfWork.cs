using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface IUnitOfWork :IDisposable
{
   IRepository<T> GetRepository<T>() where T : class;

    void SaveChanges();

    Task SaveChangesAsync();

    void BeginTransaction();

    void CommitTransaction();
    void RollbackTransaction();
}
