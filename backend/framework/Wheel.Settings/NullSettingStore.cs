using Wheel.Enums;
using Wheel.Settings;

namespace Wheel
{
    public class NullSettingStore : ISettingStore
    {
        public Task<List<SettingValueItem>?> GetSettingValues(string settingGroupName, SettingScope settingScope = SettingScope.Global, string? settingScopeKey = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(default(List<SettingValueItem>));
        }
    }
}
