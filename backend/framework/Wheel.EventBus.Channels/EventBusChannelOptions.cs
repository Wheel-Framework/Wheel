using System.Threading.Channels;

namespace Wheel
{
    public class EventBusChannelOptions
    {
        public ChannelType ChannelType { get; set; } = ChannelType.Unbounded;
        public BoundedChannelFullMode FullMode { get; set; } = BoundedChannelFullMode.Wait;
        public int Capacity { get; set; } = 10;
    }
    public enum ChannelType
    {
        Unbounded,
        bounded
    }
}
