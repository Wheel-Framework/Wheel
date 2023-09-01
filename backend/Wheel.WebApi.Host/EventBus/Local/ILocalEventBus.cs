namespace Wheel.EventBus.Local
{
    public interface ILocalEventBus
    {
        Task PublishAsync<TEventData>(TEventData eventData, CancellationToken cancellationToken = default);
    }
}
