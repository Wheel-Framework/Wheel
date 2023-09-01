namespace Wheel.EventBus.Distributed
{
    public interface IDistributedEventHandler<TEventData>
    {
        Task Handle(TEventData eventData);
    }
}
