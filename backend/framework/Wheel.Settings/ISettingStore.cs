using Wheel.Enums;
using Wheel.Settings;

namespace Wheel
{
    public interface ISettingStore
    {
        Task<List<SettingValueItem>?> GetSettingValues(string settingGroupName, SettingScope settingScope = SettingScope.Global, string? settingScopeKey = null, CancellationToken cancellationToken = default);
    }
}
