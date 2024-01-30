using Wheel.Domain.Common;
using Wheel.Administrator.Enums;

namespace Wheel.Administrator.Domain.FileStorages
{
    /// <summary>
    /// 文件信息存储表
    /// </summary>
    public class FileStorage : Entity, IHasCreationTime
    {
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
