namespace Wheel
{
    public class EventBusChannelProducer
    {
        private readonly EventBusChannel _eventBusChannel;

        public EventBusChannelProducer(EventBusChannel eventBusChannel)
        {
            _eventBusChannel = eventBusChannel;
        }

        public async Task Publish<T>(T data, CancellationToken cancellationToken)
        {
            await _eventBusChannel.Publish(new EventBusChannelData<T> { Data = data, DataType = data.GetType() }, cancellationToken);
        }
    }
}
