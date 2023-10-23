using Wheel.Domain.Common;
using Wheel.Enums;

namespace Wheel.Domain.Settings
{
    public class SettingValue : Entity
    {
        public virtual long SettingGroupId { get; set; }
        public virtual SettingGroup SettingGroup { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public SettingValueType ValueType { get; set; }
        public SettingScope SettingScope { get; set; }
        public string? SettingScopeKey { get; set; }
    }
}
