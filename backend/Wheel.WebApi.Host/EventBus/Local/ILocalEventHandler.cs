namespace Wheel.EventBus.Local
{
    public interface ILocalEventHandler<TEventData>
    {
        Task Handle(TEventData eventData);
    }
}
