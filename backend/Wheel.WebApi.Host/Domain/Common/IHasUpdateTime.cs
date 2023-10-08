namespace Wheel.Domain.Common
{
    public interface IHasUpdateTime
    {
        DateTimeOffset UpdateTime { get; set; }
    }
}
