using MediatR;

namespace Wheel.EventBus.Local.MediatR
{
    public class MediatREventBus(IMediator mediator) : ILocalEventBus
    {
        public Task PublishAsync<TEventData>(TEventData eventData, CancellationToken cancellationToken)
        {
            return mediator.Publish(eventData, cancellationToken);
        }
    }
}
