﻿using Wheel.DependencyInjection;

namespace Wheel
{
    public class EventBusChannelProducer : ISingletonDependency
    {
        private readonly EventBusChannel _eventBusChannel;

        public EventBusChannelProducer(EventBusChannel eventBusChannel)
        {
            _eventBusChannel = eventBusChannel;
        }

        public async Task Publish<T>(T data, CancellationToken cancellationToken)
        {
            await _eventBusChannel.Publish(new EventBusChannelData { Data = data, DataType = data.GetType() }, cancellationToken);
        }
    }
}