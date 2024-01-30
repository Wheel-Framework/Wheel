using Wheel.Administrator.Domain.Settings;
using Wheel.Enums;
using Wheel.Settings;

namespace Wheel.Administrator.Settings
{
    public class AdministratorSettingSotre(SettingManager settingManager) : ISettingStore
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
