using Wheel.Enums;

namespace Wheel.Settings.FileProvider
{
    public class FileProviderSettingDefinition : ISettingDefinition
    {
        public string GroupName => "FileProvider";

        public SettingScope SettingScope => SettingScope.Global;

        public Dictionary<string, SettingValueParams> Define()
        {
            return new Dictionary<string, SettingValueParams>
            {
                { "Minio.Endpoint", new(SettingValueType.String, "127.0.0.1:9000") },
                { "Minio.AccessKey", new(SettingValueType.String, "BW7bTc2dMBsfCSurwtua") },
                { "Minio.SecretKey", new(SettingValueType.String, "RcNT2C9sGrAjumTH85T4h7ZsKP2w7GYFCMyheBII") },
                { "Minio.Region", new(SettingValueType.String) },
                { "Minio.SessionToken", new(SettingValueType.String) }
            };
        }
    }
}
