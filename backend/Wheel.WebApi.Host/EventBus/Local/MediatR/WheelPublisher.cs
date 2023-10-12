using MediatR;

namespace Wheel.EventBus.Local.MediatR
{
    public class WheelPublisher : INotificationPublisher
    {
        public Task Publish(IEnumerable<NotificationHandlerExecutor> handlerExecutors, INotification notification, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                foreach (var handler in handlerExecutors)
                {
                    await handler.HandlerCallback(notification, cancellationToken).ConfigureAwait(false);
                }
            }, cancellationToken);
        }
    }
}
