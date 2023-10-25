using DotNetCore.CAP;
using System.Reflection;
using Wheel.DependencyInjection;

namespace Wheel.EventBus.Distributed.Cap
{
    public class CapDistributedEventBus : IDistributedEventBus
    {
        private readonly ICapPublisher _capBus;

        public CapDistributedEventBus(ICapPublisher capBus)
        {
            _capBus = capBus;
        }

        public Task PublishAsync<TEventData>(TEventData eventData, CancellationToken cancellationToken = default)
        {
            var sub = typeof(TEventData).GetCustomAttribute<EventNameAttribute>()?.Name;
            return _capBus.PublishAsync(sub ?? nameof(eventData), eventData, cancellationToken: cancellationToken);
        }
    }
}
