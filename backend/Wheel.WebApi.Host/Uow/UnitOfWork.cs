using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Wheel.DependencyInjection;
using Wheel.EntityFrameworkCore;

namespace Wheel.Uow
{
    public interface IUnitOfWork : IScopeDependency, IDisposable, IAsyncDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default); 
        Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WheelDbContext _dbContext;
        private IDbTransaction? Transaction = null;

        public UnitOfWork(WheelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            Transaction = new DbTransaction(_dbContext);
            await Transaction.BeginTransactionAsync(cancellationToken);
            return Transaction;
        }
        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if(Transaction == null) 
            {
                throw new Exception("Transaction is null, Please BeginTransaction");
            }
            await Transaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (Transaction == null)
            {
                throw new Exception("Transaction is null, Please BeginTransaction");
            }
            await Transaction.RollbackAsync(cancellationToken);
        }
        public void Dispose()
        {
            if(Transaction != null)
                Transaction.Dispose();
            _dbContext.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if (Transaction != null)
                await Transaction.DisposeAsync();
            await _dbContext.DisposeAsync();
        }
    }
}
