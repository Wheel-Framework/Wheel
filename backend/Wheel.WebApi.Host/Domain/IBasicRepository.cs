using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Wheel.DependencyInjection;

namespace Wheel.Domain
{
    public interface IBasicRepository<TEntity, TKey> where TEntity : class 
    {
        Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);
        Task InsertManyAsync(List<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);
        Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);
        Task UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, bool autoSave = false, CancellationToken cancellationToken = default);
        Task UpdateManyAsync(List<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default);
        Task DeleteManyAsync(List<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);
        Task<TEntity?> FindAsync(TKey id, CancellationToken cancellationToken = default);
        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<(List<TEntity> items, long total)> GetPageListAsync(Expression<Func<TEntity, bool>> predicate, int skip, int take, string orderby = "Id", CancellationToken cancellationToken = default);

        IQueryable<TEntity> GetQueryable(bool noTracking = true);

        IQueryable<TEntity> GetQueryableWithIncludes(params Expression<Func<TEntity, object>>[] propertySelectors);

        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
    }
}
