using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HelpDesk.Data.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TPocoEntity> GetRepository<TPocoEntity>() where TPocoEntity : class;
        int Complete(TransactionScope scope = null);
        Task<int> CompleteAsync();
    }
}
