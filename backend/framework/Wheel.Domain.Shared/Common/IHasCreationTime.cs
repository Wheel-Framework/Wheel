namespace Wheel.Domain.Common
{
    public interface IHasCreationTime
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTimeOffset CreationTime { get; set; }
    }
}
