using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Wheel.Uow
{
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

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            CurrentDbContextTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
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
            if (CurrentDbContextTransaction != null)
            {
                if (!isCommit && !isRollback)
                {
                    Commit();
                }
                CurrentDbContextTransaction.Dispose();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (CurrentDbContextTransaction != null)
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
