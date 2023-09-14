using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Wheel.Domain;
using Wheel.Domain.Common;

namespace Wheel.EntityFrameworkCore
{
    public class EFBasicRepository<TEntity, TKey> : IBasicRepository<TEntity, TKey> where TEntity : class
    {
        private readonly WheelDbContext _dbContext;

        private DbSet<TEntity> DbSet => _dbContext.Set<TEntity>();

        public EFBasicRepository(WheelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var savedEntity = (await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken)).Entity;
            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            return savedEntity;
        }
        public async Task InsertManyAsync(List<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var savedEntity = _dbContext.Set<TEntity>().Update(entity).Entity;

            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            return savedEntity;
        }
        public async Task UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<TEntity>().Where(predicate).ExecuteUpdateAsync(setPropertyCalls, cancellationToken);
            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        public async Task UpdateManyAsync(List<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<TEntity>().UpdateRange(entities);
            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        public async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id, cancellationToken);
            if(entity != null)
                _dbContext.Set<TEntity>().Remove(entity);
            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        public async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<TEntity>().Where(predicate).ExecuteDeleteAsync(cancellationToken);
            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        public async Task DeleteManyAsync(List<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        public async Task<TEntity?> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id, cancellationToken);
        }
        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
        }
        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken);
        }
        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return await GetQueryableWithIncludes(propertySelectors).Where(predicate).ToListAsync(cancellationToken);
        }
        public async Task<List<TSelect>> SelectListAsync<TSelect>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TSelect>> selectPredicate, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return await GetQueryableWithIncludes(propertySelectors).Where(predicate).Select(selectPredicate).ToListAsync(cancellationToken);
        }
        public async Task<List<TSelect>> SelectListAsync<TSelect>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TSelect>> selectPredicate, CancellationToken cancellationToken = default)
        {
            return await GetQueryable().Where(predicate).Select(selectPredicate).ToListAsync(cancellationToken);
        }
        public async Task<(List<TSelect> items, long total)> SelectPageListAsync<TSelect>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TSelect>> selectPredicate, int skip, int take, string orderby = "Id", CancellationToken cancellationToken = default)
        {
            var query = GetQueryable().Where(predicate).Select(selectPredicate);
            var total = await query.LongCountAsync(cancellationToken);
            var items = await query.OrderBy(orderby)
                .Skip(skip).Take(take)
                .ToListAsync(cancellationToken);
            return (items, total);
        }
        public async Task<(List<TSelect> items, long total)> SelectPageListAsync<TSelect>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TSelect>> selectPredicate, int skip, int take, string orderby = "Id", CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = GetQueryableWithIncludes(propertySelectors).Where(predicate).Select(selectPredicate);
            var total = await query.LongCountAsync(cancellationToken);
            var items = await query.OrderBy(orderby)
                .Skip(skip).Take(take)
                .ToListAsync(cancellationToken);
            return (items, total);
        }
        public async Task<(List<TEntity> items, long total)> GetPageListAsync(Expression<Func<TEntity, bool>> predicate, int skip, int take, string orderby = "Id", CancellationToken cancellationToken = default)
        {
            var query = GetQueryable().Where(predicate);
            var total = await query.LongCountAsync(cancellationToken);
            var items = await query.OrderBy(orderby)
                .Skip(skip).Take(take)
                .ToListAsync(cancellationToken);
            return (items, total);
        }
        public async Task<(List<TEntity> items, long total)> GetPageListAsync(Expression<Func<TEntity, bool>> predicate, 
            int skip, int take, string orderby = "Id", CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = GetQueryableWithIncludes(propertySelectors).Where(predicate);
            var total = await query.LongCountAsync(cancellationToken);
            var items = await query.OrderBy(orderby)
                .Skip(skip).Take(take)
                .ToListAsync(cancellationToken);
            return (items, total);
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return DbSet.AnyAsync(cancellationToken);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return DbSet.AnyAsync(predicate, cancellationToken);
        }
        public IQueryable<TEntity> GetQueryable(bool noTracking = true)
        {
            if (noTracking)
            {
                return _dbContext.Set<TEntity>().AsNoTracking();
            }
            return _dbContext.Set<TEntity>();
        }
        public IQueryable<TEntity> GetQueryableWithIncludes(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return Includes(GetQueryable(), propertySelectors);
        }

        private static IQueryable<TEntity> Includes(IQueryable<TEntity> query, Expression<Func<TEntity, object>>[] propertySelectors)
        {
            if (propertySelectors != null && propertySelectors.Length > 0)
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }
        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        protected DbSet<TEntity> GetDbSet()
        {
            return _dbContext.Set<TEntity>();
        }

        protected IDbConnection GetDbConnection()
        {
            return _dbContext.Database.GetDbConnection();
        }

        protected IDbTransaction? GetDbTransaction()
        {
            return _dbContext.Database.CurrentTransaction?.GetDbTransaction();
        }

    }
}
