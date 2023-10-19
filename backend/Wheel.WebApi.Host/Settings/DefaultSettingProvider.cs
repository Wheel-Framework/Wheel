using Microsoft.Extensions.Caching.Distributed;
using Wheel.Core.Users;
using Wheel.Domain.Settings;
using Wheel.Enums;

namespace Wheel.Settings
{
    public class DefaultSettingProvider : ISettingProvider
    {
        private readonly SettingManager _settingManager;
        private readonly IDistributedCache _distributedCache;
        private readonly ICurrentUser _currentUser;
        private readonly IServiceProvider _serviceProvider;

        public DefaultSettingProvider(SettingManager settingManager, IDistributedCache distributedCache, ICurrentUser currentUser, IServiceProvider serviceProvider)
        {
            _settingManager = settingManager;
            _distributedCache = distributedCache;
            _currentUser = currentUser;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetGolbalSetting(string groupKey, string settingKey, CancellationToken cancellationToken = default)
        {
            var settings = await GetGolbalSettings(groupKey, cancellationToken);
            return settings[settingKey];
        }

        public async Task<T> GetGolbalSetting<T>(string groupKey, string settingKey, CancellationToken cancellationToken = default) where T : struct
        {
            var settings = await GetGolbalSettings(groupKey, cancellationToken);
            return settings[settingKey].To<T>();
        }

        public async Task<Dictionary<string, string>> GetGolbalSettings(string groupKey, CancellationToken cancellationToken = default)
        {
            var cacheSettings = await GetCacheItem(groupKey, SettingScope.Golbal, cancellationToken: cancellationToken);
            if(cacheSettings is null)
            {
                var dbSettings = await _settingManager.GetSettingValues(groupKey, SettingScope.Golbal, cancellationToken: cancellationToken);
                if(dbSettings is null)
                {
                    var settingDefinition = _serviceProvider.GetServices<ISettingDefinition>().FirstOrDefault(a => a.GroupName == groupKey && a.SettingScope == SettingScope.Golbal);
                    if(settingDefinition is null)
                        return new();
                    else
                    {
                        var setting = await settingDefinition.Define();
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

        public async Task<string> GetUserSetting(string groupKey, string settingKey, CancellationToken cancellationToken = default)
        {
            var settings = await GetUserSettings(groupKey, cancellationToken);
            return settings[settingKey];
        }

        public async Task<T> GetUserSetting<T>(string groupKey, string settingKey, CancellationToken cancellationToken = default) where T : struct
        {
            var settings = await GetUserSettings(groupKey, cancellationToken);
            return settings[settingKey].To<T>();
        }

        public async Task<Dictionary<string, string>> GetUserSettings(string groupKey, CancellationToken cancellationToken = default)
        {
            var cacheSettings = await GetCacheItem(groupKey, SettingScope.User, settingScopeKey: _currentUser.Id, cancellationToken: cancellationToken);
            if (cacheSettings is null)
            {
                var dbSettings = await _settingManager.GetSettingValues(groupKey, SettingScope.User, settingScopeKey: _currentUser.Id, cancellationToken: cancellationToken);
                if (dbSettings is null)
                {
                    var settingDefinition = _serviceProvider.GetServices<ISettingDefinition>().FirstOrDefault(a => a.GroupName == groupKey && a.SettingScope == SettingScope.User);
                    if (settingDefinition is null)
                        return new();
                    else
                    {
                        var setting = await settingDefinition.Define();
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
        private async Task<List<SettingValueCacheItem>> GetCacheItem(string groupKey, SettingScope settingScope, string settingScopeKey = null, CancellationToken cancellationToken = default)
        {
            var cacheKey = BuildCacheKey(groupKey, settingScope, settingScopeKey);
            return await _distributedCache.GetAsync<List<SettingValueCacheItem>>(cacheKey, cancellationToken);
        }
        private string BuildCacheKey(string groupKey, SettingScope settingScope, string settingScopeKey)
        {
            return $"{groupKey}:{settingScope}"+ (settingScope == SettingScope.Golbal ? "" : $":{settingScopeKey}");
        }
    }
}
