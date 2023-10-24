using Wheel.DependencyInjection;

namespace Wheel.Settings
{
    public interface ISettingProvider : ITransientDependency
    {
        public Task<Dictionary<string, string>> GetGolbalSettings(string groupKey, CancellationToken cancellationToken = default);
        public Task<string> GetGolbalSetting(string groupKey, string settingKey, CancellationToken cancellationToken = default);
        public Task<T> GetGolbalSetting<T>(string groupKey, string settingKey, CancellationToken cancellationToken = default) where T : struct;

        public Task<Dictionary<string, string>> GetUserSettings(string groupKey, CancellationToken cancellationToken = default);
        public Task<string> GetUserSetting(string groupKey, string settingKey, CancellationToken cancellationToken = default);
        public Task<T> GetUserSetting<T>(string groupKey, string settingKey, CancellationToken cancellationToken = default) where T : struct;
    }
}
