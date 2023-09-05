using MediatR;
using Wheel.DependencyInjection;

namespace Wheel.EventBus.Local
{
    public class MediatREventBus : ILocalEventBus, ITransientDependency
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
