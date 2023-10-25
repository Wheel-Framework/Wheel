using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Enums;

namespace Wheel.Settings
{
    public static class SettingProviderExtensions
    {
        public static async Task<string> GetGolbalSetting(this ISettingProvider provider, string groupKey, string settingKey, CancellationToken cancellationToken = default)
        {
            var settings = await provider.GetGolbalSettings(groupKey, cancellationToken);
            return settings[settingKey];
        }

        public static async Task<T> GetGolbalSetting<T>(this ISettingProvider provider, string groupKey, string settingKey, CancellationToken cancellationToken = default) where T : struct
        {
            var settings = await provider.GetGolbalSettings(groupKey, cancellationToken);
            return settings[settingKey].To<T>();
        }

        public static async Task<Dictionary<string, string>> GetGolbalSettings(this ISettingProvider provider, string groupKey, CancellationToken cancellationToken = default)
        {
            return await provider.GetSettings(groupKey, SettingScope.Global, cancellationToken: cancellationToken);
        }

        public static async Task<string> GetUserSetting(this ISettingProvider provider, string groupKey, string settingKey, string userId, CancellationToken cancellationToken = default)
        {
            var settings = await provider.GetUserSettings(groupKey, userId, cancellationToken);
            return settings[settingKey];
        }

        public static async Task<T> GetUserSetting<T>(this ISettingProvider provider, string groupKey, string settingKey, string userId, CancellationToken cancellationToken = default) where T : struct
        {
            var settings = await provider.GetUserSettings(groupKey, userId, cancellationToken);
            return settings[settingKey].To<T>();
        }

        public static async Task<Dictionary<string, string>> GetUserSettings(this ISettingProvider provider, string groupKey, string userId, CancellationToken cancellationToken = default)
        {
            return await provider.GetSettings(groupKey, SettingScope.User, userId, cancellationToken: cancellationToken);
            
        }
    }
}
