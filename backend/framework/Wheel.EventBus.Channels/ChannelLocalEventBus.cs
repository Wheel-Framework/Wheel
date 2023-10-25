using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.DependencyInjection;
using Wheel.EventBus.Local;

namespace Wheel
{
    public class ChannelLocalEventBus : ILocalEventBus, ITransientDependency
    {
        private readonly EventBusChannelProducer _eventBusChannelProducer;
        private readonly EventBusChannelConsumer _eventBusChannelConsumer;

        public ChannelLocalEventBus(EventBusChannelProducer eventBusChannelProducer, EventBusChannelConsumer eventBusChannelConsumer)
        {
            _eventBusChannelProducer = eventBusChannelProducer;
            _eventBusChannelConsumer = eventBusChannelConsumer;
        }

        public async Task PublishAsync<TEventData>(TEventData eventData, CancellationToken cancellationToken = default)
        {
            await _eventBusChannelProducer.Publish(eventData, cancellationToken).ConfigureAwait(false);
        }
    }
}
