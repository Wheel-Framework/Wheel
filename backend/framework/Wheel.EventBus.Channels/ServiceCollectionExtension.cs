using Microsoft.Extensions.DependencyInjection;
using Wheel.EventBus.Local;

namespace Wheel
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddChannelLoacalEventBus(this IServiceCollection services, Action<EventBusChannelOptions> options = null)
        {
            EventBusChannelOptions eventBusChannelOptions = new();
            if (options != null)
            {
                options.Invoke(eventBusChannelOptions);
            }
            services.Configure<EventBusChannelOptions>(a => a = eventBusChannelOptions);

            services.AddTransient<ILocalEventBus, ChannelLocalEventBus>();
            services.AddSingleton<EventBusChannel>();
            services.AddSingleton<EventBusChannelProducer>();
            services.AddSingleton<EventBusChannelConsumer>();
            return services;
        }
    }
}
