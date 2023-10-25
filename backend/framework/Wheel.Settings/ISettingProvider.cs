using Wheel.DependencyInjection;
using Wheel.Enums;

namespace Wheel.Settings
{
    public interface ISettingProvider
    {
        public Task<Dictionary<string, string>> GetSettings(string settingGroupName, SettingScope settingScope = SettingScope.Global, string? settingScopeKey = null, CancellationToken cancellationToken = default);
    }
}
