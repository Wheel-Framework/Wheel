﻿using MediatR;
using MediatR.NotificationPublishers;
using System.Reflection;

namespace Wheel.EventBus.Local.MediatR
{
    public class WheelPublisher : INotificationPublisher
    {
        public Task Publish(IEnumerable<NotificationHandlerExecutor> handlerExecutors, INotification notification, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(async () => 
            {
                foreach (var handler in handlerExecutors.DistinctBy(a => a.HandlerInstance.GetType().FullName)) 
                {
                    await handler.HandlerCallback(notification, cancellationToken).ConfigureAwait(false);
                }
            }, cancellationToken);
        }
    }
}
