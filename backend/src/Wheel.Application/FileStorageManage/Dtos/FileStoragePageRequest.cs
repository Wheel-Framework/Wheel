using Wheel.Core.Dto;
using Wheel.Enums;

namespace Wheel.Services.FileStorageManage.Dtos
{
    public class FileStoragePageRequest : PageRequest
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string? FileName { get; set; }
        /// <summary>
        /// 文件类型ContentType
        /// </summary>
        public string? ContentType { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public FileStorageType? FileStorageType { get; set; }
        /// <summary>
        /// 存储路径
        /// </summary>
        public string? Path { get; set; }
        /// <summary>
        /// 创建时间From
        /// </summary>
        public DateTimeOffset CreationTimeFrom { get; set; }
        /// <summary>
        /// 创建时间To
        /// </summary>
        public DateTimeOffset CreationTimeTo { get; set; }
        /// <summary>
        /// 存储类型
        /// </summary>
        public string? Provider { get; set; }
    }
}
