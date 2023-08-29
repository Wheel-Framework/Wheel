using Microsoft.EntityFrameworkCore;
using Wheel.DependencyInjection;
using Wheel.EntityFrameworkCore;

namespace Wheel.Uow
{
    public interface IUnitOfWork : IScopeDependency, IDisposable, IAsyncDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WheelDbContext _dbContext;

        public UnitOfWork(WheelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return _dbContext.DisposeAsync();
        }
    }
}
