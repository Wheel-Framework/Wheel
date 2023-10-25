using System.Threading.Channels;
using Wheel.DependencyInjection;

namespace Wheel
{
    public class EventBusChannel : ISingletonDependency
    {
        private readonly Channel<EventBusChannelData> _channel;

        public EventBusChannel()
        {
            _channel = Channel.CreateUnbounded<EventBusChannelData>();
        }

        public async Task Publish(EventBusChannelData data, CancellationToken cancellationToken = default)
        {
            while (await _channel.Writer.WaitToWriteAsync(cancellationToken))
            {
                await _channel.Writer.WriteAsync(data, cancellationToken);
                return;
            }
        }
        public async Task<EventBusChannelData> Subscribe(CancellationToken cancellationToken = default)
        {
            while (await _channel.Reader.WaitToReadAsync(cancellationToken))
            {
                return await _channel.Reader.ReadAsync(cancellationToken);
            }
            return default;
        }
    }
}
