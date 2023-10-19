using Wheel.Domain.Settings;
using Wheel.Enums;

namespace Wheel.Settings
{
    public class SettingValueCacheItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public SettingValueType ValueType { get; set; }
    }
}
