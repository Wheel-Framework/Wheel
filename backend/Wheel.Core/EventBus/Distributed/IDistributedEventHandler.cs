using Wheel.DependencyInjection;

namespace Wheel.EventBus.Distributed
{
    public interface IDistributedEventHandler<in TEventData> : IEventHandler
    {
        Task Handle(TEventData eventData, CancellationToken cancellationToken = default);
    }
}
