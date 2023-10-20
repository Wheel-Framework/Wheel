using Wheel.Enums;

namespace Wheel.Settings.Email
{
    public class EmailSettingDefinition : ISettingDefinition
    {
        public string GroupName => "EmailSetting";

        public SettingScope SettingScope => SettingScope.Global;

        public Dictionary<string, SettingValueParams> Define()
        {
            return new Dictionary<string, SettingValueParams>
            {
                { "SenderName", new(SettingValueType.String, "Wheel") },
                { "Host", new(SettingValueType.String, "smtp.exmail.qq.com") },
                { "Prot", new(SettingValueType.Int, "465") },
                { "UserName", new(SettingValueType.String) },
                { "Password", new(SettingValueType.String) },
                { "UseSsl", new(SettingValueType.Bool, "true") },
            };
        }
    }
}
