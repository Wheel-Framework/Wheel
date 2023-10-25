using Microsoft.Extensions.Options;
using System.Threading.Channels;
using Wheel.DependencyInjection;

namespace Wheel
{
    public class EventBusChannel : ISingletonDependency
    {
        private readonly Channel<EventBusChannelData> _channel;

        public EventBusChannel(IOptions<EventBusChannelOptions> options)
        {
            var eventBusChannelOptions = options.Value;
            if (eventBusChannelOptions.ChannelType == ChannelType.Unbounded)
                _channel = Channel.CreateUnbounded<EventBusChannelData>();
            else
                _channel = Channel.CreateBounded<EventBusChannelData>(new BoundedChannelOptions(eventBusChannelOptions.Capacity)
                {
                    FullMode = eventBusChannelOptions.FullMode
                });
        }

        public async Task Publish(EventBusChannelData data, CancellationToken cancellationToken = default)
        {
            await _channel.Writer.WriteAsync(data, cancellationToken);
        }
        public async Task<EventBusChannelData> Subscribe(CancellationToken cancellationToken = default)
        {
            return await _channel.Reader.ReadAsync(cancellationToken);
        }
    }
}
