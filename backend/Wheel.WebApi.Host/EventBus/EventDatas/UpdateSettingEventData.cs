using Wheel.Enums;

namespace Wheel.EventBus.EventDatas
{
    [EventName("UpdateSetting")]
    public class UpdateSettingEventData
    {
        public string GroupName { get; set; }
        public SettingScope SettingScope { get; set; }
        public string? SettingScopeKey { get; set; }
    }
}
