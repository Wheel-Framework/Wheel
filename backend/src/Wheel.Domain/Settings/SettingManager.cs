﻿using Wheel.DependencyInjection;
using Wheel.Enums;
using Wheel.EventBus.Distributed;
using Wheel.EventBus.EventDatas;
using Wheel.Uow;
using Wheel.Utilities;

namespace Wheel.Domain.Settings
{
    public class SettingManager(IBasicRepository<SettingGroup, long> settingGroupRepository,
            IBasicRepository<SettingValue, long> settingValueRepository, IUnitOfWork unitOfWork,
            SnowflakeIdGenerator snowflakeIdGenerator, IDistributedEventBus distributedEventBus)
        : ITransientDependency
    {
        public async Task<T?> GetSettingValue<T>(string settingGroupName, string settingKey, SettingScope settingScope = SettingScope.Global, string? settingScopeKey = null, CancellationToken cancellationToken = default)
        {
            var settingGroup = await settingGroupRepository.FindAsync(a => a.Name == settingGroupName, cancellationToken);

            if (settingGroup is null)
            {
                return default(T);
            }

            var settingValue = settingGroup?.SettingValues.FirstOrDefault(a => a.Key == settingKey && a.SettingScope == settingScope && a.SettingScopeKey == settingScopeKey);

            if (settingValue is null)
                return default;

            if (settingValue.ValueType == SettingValueType.JsonObject)
                return settingValue.Value.ToObject<T>();

            return (T)Convert.ChangeType(settingValue, typeof(T));
        }

        public async Task<SettingValue?> GetSettingValue(string settingGroupName, string settingKey, SettingScope settingScope = SettingScope.Global, string? settingScopeKey = null, CancellationToken cancellationToken = default)
        {
            var settingGroup = await settingGroupRepository.FindAsync(a => a.Name == settingGroupName, cancellationToken);

            if (settingGroup is null)
            {
                return null;
            }

            var settingValue = settingGroup?.SettingValues.FirstOrDefault(a => a.Key == settingKey && a.SettingScope == settingScope && a.SettingScopeKey == settingScopeKey);

            return settingValue;
        }

        public async Task<List<SettingValue>?> GetSettingValues(string settingGroupName, SettingScope settingScope = SettingScope.Global, string? settingScopeKey = null, CancellationToken cancellationToken = default)
        {
            var settingGroup = await settingGroupRepository.FindAsync(a => a.Name == settingGroupName, cancellationToken);

            if (settingGroup is null)
            {
                return null;
            }

            var settingValues = settingGroup?.SettingValues.Where(a => a.SettingScope == settingScope && a.SettingScopeKey == settingScopeKey).ToList();

            return settingValues;
        }

        public async Task SetSettingValue(string settingGroupName, SettingValue settingValue, CancellationToken cancellationToken = default)
        {
            using (var uow = await unitOfWork.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var settingGroup = await settingGroupRepository.FindAsync(a => a.Name == settingGroupName, cancellationToken);
                    if (settingGroup is null)
                        settingGroup = await settingGroupRepository.InsertAsync(new SettingGroup { Id = snowflakeIdGenerator.Create(), Name = settingGroupName, NormalizedName = settingGroupName.ToUpper() }, cancellationToken: cancellationToken);


                    CheckSettingValueType(settingValue.Value, settingValue.ValueType);

                    var sv = await settingValueRepository.FindAsync(a => a.SettingGroupId == settingGroup.Id && a.Id == settingValue.Id, cancellationToken);
                    if (sv is null)
                    {
                        settingValue.Id = snowflakeIdGenerator.Create();
                        settingValue.SettingGroupId = settingGroup.Id;
                        await settingValueRepository.InsertAsync(settingValue, cancellationToken: cancellationToken);
                    }
                    else
                        await settingValueRepository.UpdateAsync(settingValue, cancellationToken: cancellationToken);

                    await uow.CommitAsync(cancellationToken);
                    await distributedEventBus.PublishAsync(new UpdateSettingEventData() { GroupName = settingGroupName, SettingScope = settingValue.SettingScope, SettingScopeKey = settingValue.SettingScopeKey });
                }
                catch (Exception ex)
                {
                    await uow.RollbackAsync(cancellationToken);
                    ex.ReThrow();
                }
            }
        }
        public async Task SetSettingValues(string settingGroupName, List<SettingValue> settingValues, CancellationToken cancellationToken = default)
        {
            using (var uow = await unitOfWork.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var settingGroup = await settingGroupRepository.FindAsync(a => a.Name == settingGroupName, cancellationToken);
                    if (settingGroup is null)
                        settingGroup = await settingGroupRepository.InsertAsync(new SettingGroup { Id = snowflakeIdGenerator.Create(), Name = settingGroupName, NormalizedName = settingGroupName.ToUpper() }, true, cancellationToken: cancellationToken);

                    foreach (var settingValue in settingValues)
                    {
                        CheckSettingValueType(settingValue.Value, settingValue.ValueType);

                        var sv = await settingValueRepository.FindAsync(a => a.SettingGroupId == settingGroup.Id && a.Id == settingValue.Id, cancellationToken);
                        if (sv is null)
                        {
                            settingValue.Id = snowflakeIdGenerator.Create();
                            settingValue.SettingGroupId = settingGroup.Id;
                            await settingValueRepository.InsertAsync(settingValue, cancellationToken: cancellationToken);
                        }
                        else
                            await settingValueRepository.UpdateAsync(settingValue, cancellationToken: cancellationToken);
                    }

                    await uow.CommitAsync(cancellationToken);
                    await distributedEventBus.PublishAsync(new UpdateSettingEventData() { GroupName = settingGroupName, SettingScope = settingValues.First().SettingScope, SettingScopeKey = settingValues.First().SettingScopeKey });
                }
                catch (Exception ex)
                {
                    await uow.RollbackAsync(cancellationToken);
                    ex.ReThrow();
                }
            }
        }

        private void CheckSettingValueType(string settingValue, SettingValueType settingValueType)
        {
            switch (settingValueType)
            {
                case SettingValueType.String:
                case SettingValueType.JsonObject:
                    return;
                case SettingValueType.Bool:
                    if (bool.TryParse(settingValue, out var _))
                    {
                        return;
                    }
                    else
                    {
                        throw new ArgumentException($"SettingValue: {settingValue} Can Not Parse To Bool Type");
                    }
                case SettingValueType.Int:
                    if (int.TryParse(settingValue, out var _))
                    {
                        return;
                    }
                    else
                    {
                        throw new ArgumentException($"SettingValue: {settingValue} Can Not Parse To Int Type");
                    }
                case SettingValueType.Long:
                    if (long.TryParse(settingValue, out var _))
                    {
                        return;
                    }
                    else
                    {
                        throw new ArgumentException($"SettingValue: {settingValue} Can Not Parse To Long Type");
                    }
                case SettingValueType.Double:
                    if (double.TryParse(settingValue, out var _))
                    {
                        return;
                    }
                    else
                    {
                        throw new ArgumentException($"SettingValue: {settingValue} Can Not Parse To Double Type");
                    }
                case SettingValueType.Decimal:
                    if (decimal.TryParse(settingValue, out var _))
                    {
                        return;
                    }
                    else
                    {
                        throw new ArgumentException($"SettingValue: {settingValue} Can Not Parse To Decimal Type");
                    }
            }
        }
    }
}
