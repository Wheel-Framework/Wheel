using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Wheel.EventBus;
using Wheel.EventBus.Local;

namespace Wheel
{
    public class EventBusChannelConsumer
    {
        private readonly EventBusChannel _eventBusChannel;
        private readonly IServiceProvider _serviceProvider;

        private Dictionary<Type, Type> HandlerDic = new Dictionary<Type, Type>();

        private readonly ILogger<EventBusChannelConsumer> _logger;
        public EventBusChannelConsumer(EventBusChannel eventBusChannel, IServiceProvider serviceProvider, ILogger<EventBusChannelConsumer> logger)
        {
            _eventBusChannel = eventBusChannel;
            _serviceProvider = serviceProvider;
            _logger = logger;
            InitHandlers();
            _ = Task.Factory.StartNew(async () =>
            {
                await Parallel.ForEachAsync(HandlerDic, async (typeHandler, cancellationToken) =>
                    {
                        MethodInfo handleMethod = typeof(EventBusChannelConsumer).GetMethod(nameof(this.Handle));
                        MethodInfo genericMethod = handleMethod.MakeGenericMethod(typeHandler.Key);
                        var handle = genericMethod.Invoke(this, new object[] { cancellationToken }) as Task;
                        await handle;
                    });
            });
        }

        public async Task Handle<T>(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                var eventData = await _eventBusChannel.Subscribe<T>(cancellationToken);
                _logger.LogInformation($"EventBusChannelConsumer Handle Start: {eventData.DataType.FullName} .");
                try
                {
                    if (HandlerDic.TryGetValue(eventData.DataType, out var handlerType))
                    {
                        var handlers = _serviceProvider.GetServices<ILocalEventHandler<T>>();
                        foreach (var handler in handlers)
                        {
                            _logger.LogInformation($"{handler.GetType().FullName} Handle.");
                            await handler.Handle(eventData.Data, cancellationToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

            }
        }

        void InitHandlers()
        {
            //handlers
            var handlers = _serviceProvider.GetServices<IEventHandler>()
                     .Select(o => o.GetType()).ToList();
            foreach (var handler in handlers)
            {
                var interfaces = handler.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    if (!typeof(IEventHandler).GetTypeInfo().IsAssignableFrom(@interface))
                    {
                        continue;
                    }
                    var genericArgs = @interface.GetGenericArguments();

                    if (genericArgs.Length != 1)
                    {
                        continue;
                    }
                    if (!(@interface.GetGenericTypeDefinition() == typeof(ILocalEventHandler<>)))
                    {
                        continue;
                    }
                    HandlerDic.TryAdd(genericArgs[0], @interface);

                }
            }
        }
    }
}
