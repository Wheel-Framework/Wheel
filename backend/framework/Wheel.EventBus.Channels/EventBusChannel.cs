using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Wheel
{
    public class EventBusChannel(IOptions<EventBusChannelOptions> options)
    {
        private readonly ConcurrentDictionary<Type, object> _channelDic = new();
        private readonly EventBusChannelOptions eventBusChannelOptions = options.Value;

        public async Task Publish<T>(EventBusChannelData<T> data, CancellationToken cancellationToken = default)
        {
            var channel = GetChannel<T>();
            await channel.Writer.WriteAsync(data, cancellationToken);
        }
        public async Task<EventBusChannelData<T>> Subscribe<T>(CancellationToken cancellationToken = default)
        {
            var channel = GetChannel<T>();
            return await channel.Reader.ReadAsync(cancellationToken);
        }

        private Channel<EventBusChannelData<T>> GetChannel<T>()
        {
            if (!_channelDic.TryGetValue(typeof(T), out var value) || value is not Channel<EventBusChannelData<T>> channel)
            {
                if (eventBusChannelOptions.ChannelType == ChannelType.Unbounded)
                    channel = Channel.CreateUnbounded<EventBusChannelData<T>>();
                else
                    channel = Channel.CreateBounded<EventBusChannelData<T>>(new BoundedChannelOptions(eventBusChannelOptions.Capacity)
                    {
                        FullMode = eventBusChannelOptions.FullMode
                    });
                _channelDic.TryAdd(typeof(T), channel);
            }
            return channel;
        }
    }
}
