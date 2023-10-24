using Wheel.Enums;

namespace Wheel.Domain.FileStorages
{
    public static class FileStorageTypeChecker
    {
        public static FileStorageType CheckFileType(string contentType)
        {
            return contentType switch
            {
                var _ when contentType.StartsWith("audio") => FileStorageType.Audio,
                var _ when contentType.StartsWith("image") => FileStorageType.Image,
                var _ when contentType.StartsWith("text") => FileStorageType.Text,
                var _ when contentType.StartsWith("video") => FileStorageType.Video,
                _ => FileStorageType.File
            };
        }
    }
}
