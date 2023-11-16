namespace Wheel
{
    public class EventBusChannelProducer(EventBusChannel eventBusChannel)
    {
        public async Task Publish<T>(T data, CancellationToken cancellationToken)
        {
            await eventBusChannel.Publish(new EventBusChannelData<T> { Data = data, DataType = data.GetType() }, cancellationToken);
        }
    }
}
