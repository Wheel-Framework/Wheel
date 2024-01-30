using Wheel.Administrator.Enums;

namespace Wheel.Services.FileStorageManage.Dtos
{
    public class FileStorageDto
    {
        public long Id { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件类型ContentType
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public FileStorageType FileStorageType { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 存储路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }
        /// <summary>
        /// 存储类型
        /// </summary>
        public string Provider { get; set; }
    }
}
