using Wheel.Enums;

namespace Wheel.Domain.FileStorages
{
    public static class FileStorageTypeChecker
    {
        public static FileStorageType CheckFileType(string contentType)
        {
            if (contentType.StartsWith("audio"))
                return FileStorageType.Audio;
            if (contentType.StartsWith("image"))
                return FileStorageType.Image;
            if (contentType.StartsWith("text"))
                return FileStorageType.Text;
            if (contentType.StartsWith("video"))
                return FileStorageType.Video;

            return FileStorageType.File;
        }
    }
}
