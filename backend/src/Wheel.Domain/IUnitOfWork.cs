﻿using Wheel.DependencyInjection;

namespace Wheel.Uow
{
    public interface IUnitOfWork : IScopeDependency, IDisposable, IAsyncDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
