using Wheel.Enums;

namespace Wheel.Settings
{
    public record SettingValueParams(SettingValueType SettingValueType, string? DefalutValue = null, string? SettingScopeKey = null);
}
