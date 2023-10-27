using MediatR;

namespace Wheel.EventBus.Local.MediatR
{
    public class MediatREventBus : ILocalEventBus
    {
        private readonly IMediator _mediator;

        public MediatREventBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task PublishAsync<TEventData>(TEventData eventData, CancellationToken cancellationToken)
        {
            return _mediator.Publish(eventData, cancellationToken);
        }
    }
}
