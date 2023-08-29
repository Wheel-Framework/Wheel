using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using Wheel.DependencyInjection;
using Wheel.EntityFrameworkCore;

namespace Wheel.Uow
{
    public interface IDbTransaction : IDisposable, IAsyncDisposable
    {
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
    public class DbTransaction : IDbTransaction
    {
        private readonly DbContext _dbContext;

        IDbContextTransaction? CurrentDbContextTransaction;

        bool isCommit = false;
        bool isRollback = false;
        public DbTransaction(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            CurrentDbContextTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            return CurrentDbContextTransaction;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync();
            await _dbContext.Database.CommitTransactionAsync();
            isCommit = true;
            CurrentDbContextTransaction = null;
        }
        public void Commit()
        {
            _dbContext.Database.CommitTransaction();
            isCommit = true;
            CurrentDbContextTransaction = null;
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
            isRollback = true;
            CurrentDbContextTransaction = null;
        }
        public void Dispose()
        {
            if(CurrentDbContextTransaction != null)
            {
                if(!isCommit && !isRollback)
                {
                    Commit();
                }
                CurrentDbContextTransaction.Dispose();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if(CurrentDbContextTransaction != null)
            {
                if (!isCommit && !isRollback)
                {
                    await CommitAsync();
                }
                await CurrentDbContextTransaction.DisposeAsync();
            }
        }

    }
}
