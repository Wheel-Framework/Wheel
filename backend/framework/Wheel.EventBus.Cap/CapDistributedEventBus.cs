using DotNetCore.CAP;
using System.Reflection;

namespace Wheel.EventBus.Distributed.Cap
{
    public class CapDistributedEventBus(ICapPublisher capBus) : IDistributedEventBus
    {
        public Task PublishAsync<TEventData>(TEventData eventData, CancellationToken cancellationToken = default)
        {
            var sub = typeof(TEventData).GetCustomAttribute<EventNameAttribute>()?.Name;
            return capBus.PublishAsync(sub ?? typeof(TEventData).FullName, eventData, cancellationToken: cancellationToken);
        }
    }
}
