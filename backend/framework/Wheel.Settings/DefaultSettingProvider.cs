using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Wheel.Enums;

namespace Wheel.Settings
{
    public class DefaultSettingProvider : ISettingProvider
    {
        private readonly ISettingStore _settingStore;
        private readonly IDistributedCache _distributedCache;
        private readonly IServiceProvider _serviceProvider;

        public DefaultSettingProvider(ISettingStore settingStore, IDistributedCache distributedCache, IServiceProvider serviceProvider)
        {
            _settingStore = settingStore;
            _distributedCache = distributedCache;
            _serviceProvider = serviceProvider;
        }
        private async Task<List<SettingValueItem>> GetCacheItem(string groupKey, SettingScope settingScope, string settingScopeKey = null, CancellationToken cancellationToken = default)
        {
            var cacheKey = BuildCacheKey(groupKey, settingScope, settingScopeKey);
            return await _distributedCache.GetAsync<List<SettingValueItem>>(cacheKey, cancellationToken);
        }
        private string BuildCacheKey(string groupKey, SettingScope settingScope, string settingScopeKey)
        {
            return $"{groupKey}:{settingScope}" + (settingScope == SettingScope.Global ? "" : $":{settingScopeKey}");
        }

        public async Task<Dictionary<string, string>> GetSettings(string settingGroupName, SettingScope settingScope = SettingScope.Global, string? settingScopeKey = null, CancellationToken cancellationToken = default)
        {
            var cacheSettings = await GetCacheItem(settingGroupName, settingScope, settingScopeKey: settingScopeKey, cancellationToken: cancellationToken);
            if (cacheSettings is null)
            {
                var dbSettings = await _settingStore.GetSettingValues(settingGroupName, settingScope, settingScopeKey: settingScopeKey, cancellationToken: cancellationToken);
                if (dbSettings is null)
                {
                    var settingDefinition = _serviceProvider.GetServices<ISettingDefinition>().FirstOrDefault(a => a.GroupName == settingGroupName && a.SettingScope == settingScope);
                    if (settingDefinition is null)
                        return new();
                    else
                    {
                        var setting = settingDefinition.Define();
                        return setting.ToDictionary(a => a.Key, a => a.Value.DefalutValue)!;
                    }
                }
                return dbSettings.ToDictionary(a => a.Key, a => a.Value);
            }
            else
            {
                return cacheSettings.ToDictionary(a => a.Key, a => a.Value);
            }
        }
    }
}
