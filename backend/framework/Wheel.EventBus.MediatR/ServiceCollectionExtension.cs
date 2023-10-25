using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Wheel.EventBus.Local;
using Wheel.EventBus.Local.MediatR;

namespace Wheel
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMediatRLoacalEventBus(this IServiceCollection services)
        {
            services.AddTransient<ILocalEventBus, MediatREventBus>();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                                    .Where(x => !x.Contains("Microsoft.") && !x.Contains("System."))
                                    .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x))).ToArray());
                cfg.NotificationPublisher = new WheelPublisher();
                cfg.NotificationPublisherType = typeof(WheelPublisher);
            });
            return services;
        }
    }
}
