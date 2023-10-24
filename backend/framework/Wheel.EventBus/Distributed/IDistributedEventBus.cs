namespace Wheel.EventBus.Distributed
{
    public interface IDistributedEventBus
    {
        Task PublishAsync<TEventData>(TEventData eventData, CancellationToken cancellationToken = default);
    }
}
