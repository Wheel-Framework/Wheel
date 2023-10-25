using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using Microsoft.Extensions.DependencyInjection;
using Wheel.EventBus.Distributed;
using Wheel.EventBus.Distributed.Cap;

namespace Wheel
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCapDistributedEventBus(this IServiceCollection services, Action<CapOptions> options)
        {
            services.AddTransient<IDistributedEventBus, CapDistributedEventBus>();
            services.AddSingleton<IConsumerServiceSelector, WheelConsumerServiceSelector>();
            services.AddCap(options);
            return services;
        }
    }
}
