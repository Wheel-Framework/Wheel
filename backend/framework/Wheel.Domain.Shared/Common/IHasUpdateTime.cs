namespace Wheel.Domain.Common
{
    public interface IHasUpdateTime
    {
        /// <summary>
        /// 最近修改时间
        /// </summary>
        DateTimeOffset UpdateTime { get; set; }
    }
}
