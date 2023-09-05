namespace Wheel.EventBus.Distributed
{
    public interface IDistributedEventHandler<in TEventData>
    {
        Task Handle(TEventData eventData, CancellationToken cancellationToken = default);
    }
}
