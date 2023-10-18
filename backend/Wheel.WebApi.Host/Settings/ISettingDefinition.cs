using Wheel.DependencyInjection;
using Wheel.Enums;

namespace Wheel.Settings
{
    public interface ISettingDefinition : ITransientDependency
    {
        string GroupName { get; }
        SettingScope SettingScope { get; }

        ValueTask<Dictionary<string, SettingValueParams>> Define();
    }
}
