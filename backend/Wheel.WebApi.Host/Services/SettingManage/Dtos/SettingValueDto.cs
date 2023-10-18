using System.Text.Json.Serialization;
using Wheel.Enums;
using Wheel.Json;

namespace Wheel.Services.SettingManage.Dtos
{
    public class SettingValueDto
    {
        public long Id { get; set; }
        public virtual long SettingGroupId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public SettingValueType ValueType { get; set; }
    }
}
