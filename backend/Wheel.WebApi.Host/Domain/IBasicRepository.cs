using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Wheel.DependencyInjection;

namespace Wheel.Domain
{
    public interface IBasicRepository<TEntity, TKey> where TEntity : class 
    {
        Task<int> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<int> InsertManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default);
        Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, CancellationToken cancellationToken = default);
        Task<int> UpdateManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default);
        Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<int> DeleteManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default);
        Task<TEntity?> FindAsync(TKey id, CancellationToken cancellationToken = default);
        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<(List<TEntity> items, long total)> GetPageListAsync(Expression<Func<TEntity, bool>> predicate, int skip, int take, string orderby = "Id", CancellationToken cancellationToken = default);
    }
}
