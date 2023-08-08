using Microsoft.OpenApi.Writers;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Wheel.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection InitWheelDependency(this IServiceCollection services)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(ab=>ab.GetTypes().Where(t => t.IsClass && !t.IsAbstract));
            var transientTypes = types
                .Where(t => typeof(ITransientDependency).IsAssignableFrom(t));
            foreach (var transientType in transientTypes)
            {
                var typeInterfaces = transientType.GetInterfaces();
                if (typeInterfaces.Length > 0)
                {
                    foreach (var typeInterface in typeInterfaces)
                    {
                        services.AddSingleton(typeInterface, transientType);
                    }
                }
                else
                {
                    services.AddTransient(transientType);
                }
            }
            var scopeTypes = types
                .Where(t => typeof(IScopeDependency).IsAssignableFrom(t));
            foreach (var scopeType in scopeTypes)
            {
                var typeInterfaces = scopeType.GetInterfaces();
                if (typeInterfaces.Length > 0)
                {
                    foreach (var typeInterface in typeInterfaces)
                    {
                        services.AddSingleton(typeInterface, scopeType);
                    }
                }
                else
                {
                    services.AddScoped(scopeType);
                }
            }
            var singletonTypes = types
                .Where(t => typeof(ISingletonDependency).IsAssignableFrom(t));
            foreach (var singletonType in singletonTypes)
            {
                var typeInterfaces = singletonType.GetInterfaces();
                if (typeInterfaces.Length > 0)
                {
                    foreach (var typeInterface in typeInterfaces)
                    {
                        services.AddSingleton(typeInterface, singletonType);
                    }
                }
                else
                {
                    services.AddSingleton(singletonType);
                }
            }
            
            return services;
        }
    }
}
