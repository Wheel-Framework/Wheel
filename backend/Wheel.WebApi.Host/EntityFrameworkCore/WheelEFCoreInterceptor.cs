using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Wheel.Domain.Common;

namespace Wheel.EntityFrameworkCore
{
    /// <summary>
    /// EF拦截器
    /// </summary>
    public sealed class WheelEFCoreInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            OnSavingChanges(eventData);
            return base.SavingChanges(eventData, result);
        }

        public static void OnSavingChanges(DbContextEventData eventData)
        {
            ArgumentNullException.ThrowIfNull(eventData.Context);
            eventData.Context.ChangeTracker.DetectChanges();
            foreach (var entityEntry in eventData.Context.ChangeTracker.Entries())
            {
                if (entityEntry is { State: EntityState.Deleted, Entity: ISoftDelete softDeleteEntity })
                {
                    softDeleteEntity.IsDeleted = true;
                    entityEntry.State = EntityState.Modified;
                }
                if (entityEntry is { State: EntityState.Modified, Entity: IHasUpdateTime hasUpdateTimeEntity })
                {
                    hasUpdateTimeEntity.UpdateTime = DateTimeOffset.Now;
                }
                if (entityEntry is { State: EntityState.Added, Entity: IHasCreationTime hasCreationTimeEntity })
                {
                    hasCreationTimeEntity.CreationTime = DateTimeOffset.Now;
                }
            }
        }
    }
}
