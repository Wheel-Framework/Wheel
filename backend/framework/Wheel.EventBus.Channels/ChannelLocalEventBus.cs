using Wheel.EventBus.Local;

namespace Wheel
{
    public class ChannelLocalEventBus(EventBusChannelProducer eventBusChannelProducer,
            EventBusChannelConsumer eventBusChannelConsumer)
        : ILocalEventBus
    {
        private readonly EventBusChannelConsumer _eventBusChannelConsumer = eventBusChannelConsumer;

        public async Task PublishAsync<TEventData>(TEventData eventData, CancellationToken cancellationToken = default)
        {
            await eventBusChannelProducer.Publish(eventData, cancellationToken).ConfigureAwait(false);
        }
    }
}
