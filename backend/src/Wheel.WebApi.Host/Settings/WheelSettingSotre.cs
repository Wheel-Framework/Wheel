using Wheel.Domain.Settings;
using Wheel.Enums;

namespace Wheel.Settings
{
    public class WheelSettingSotre(SettingManager settingManager) : ISettingStore
    {
        public async Task<List<SettingValueItem>?> GetSettingValues(string settingGroupName, SettingScope settingScope = SettingScope.Global, string? settingScopeKey = null, CancellationToken cancellationToken = default)
        {
            var dbData = await settingManager.GetSettingValues(settingGroupName, SettingScope.User, settingScopeKey: settingScopeKey, cancellationToken: cancellationToken); ;
            if (dbData == null)
            {
                return null;
            }
            return dbData.Select(x => new SettingValueItem { Key = x.Key, Value = x.Value, ValueType = x.ValueType }).ToList();
        }
    }
}
