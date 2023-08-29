using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Wheel.DependencyInjection;
using Wheel.EntityFrameworkCore;

namespace Wheel.Uow
{
    public interface IUnitOfWork : IScopeDependency, IDisposable, IAsyncDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default); 
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WheelDbContext _dbContext;
        private IDbContextTransaction? Transaction = null;

        public UnitOfWork(WheelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            Transaction = await _dbContext.Database.BeginTransactionAsync();
            return Transaction;
        }
        public async Task CommitAsync()
        {
            await Transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await Transaction.RollbackAsync();
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
