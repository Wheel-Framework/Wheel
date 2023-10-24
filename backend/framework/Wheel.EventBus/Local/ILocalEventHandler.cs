namespace Wheel.EventBus.Local
{
    public interface ILocalEventHandler<in TEventData> : IEventHandler
    {
        Task Handle(TEventData eventData, CancellationToken cancellationToken = default);
    }
}
