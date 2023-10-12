using DotNetCore.CAP.Internal;
using System.Reflection;
using Wheel.EntityFrameworkCore;
using Wheel.EventBus.Distributed.Cap;
using Wheel.EventBus.Local.MediatR;

namespace Wheel.EventBus
{
    public static class EventBusExtensions
    {
        public static IServiceCollection AddLocalEventBus(this IServiceCollection services)
        {
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
        public static IServiceCollection AddDistributedEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsumerServiceSelector, WheelConsumerServiceSelector>();
            services.AddCap(x =>
            {
                x.UseEntityFramework<WheelDbContext>();

                x.UseSqlite(configuration.GetConnectionString("Default"));

                //x.UseRabbitMQ(configuration["RabbitMQ:ConnectionString"]);
                x.UseRedis(configuration["Cache:Redis"]);
            });
            return services;
        }
    }
}
