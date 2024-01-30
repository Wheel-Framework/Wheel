using Wheel.Enums;
using Wheel.EventBus;

namespace Wheel.Administrator.EventBus.EventDatas
{
    [EventName("UpdateSetting")]
    public class UpdateSettingEventData
    {
        public string GroupName { get; set; }
        public SettingScope SettingScope { get; set; }
        public string? SettingScopeKey { get; set; }
    }
}
