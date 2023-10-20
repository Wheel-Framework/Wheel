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
                { "Minio.AccessKey", new(SettingValueType.String, "2QgNxo11uxgULRvkrdaT") },
                { "Minio.SecretKey", new(SettingValueType.String, "NvzXnh81UMwEcvLJc8BslA1GA0j0sCq0aXRgHSRJ") },
                { "Minio.Region", new(SettingValueType.String) },
                { "Minio.SessionToken", new(SettingValueType.String) }
            };
        }
    }
}
