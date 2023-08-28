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
            var types = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                        .Where(x => !x.Contains("Microsoft.") && !x.Contains("System."))
                        .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)))
                        .SelectMany(ab=>ab.GetTypes().Where(t => t.IsClass && !t.IsAbstract));
            var transientTypes = types
                .Where(t => typeof(ITransientDependency).IsAssignableFrom(t));
            foreach (var transientType in transientTypes)
            {
                var typeInterfaces = transientType.GetInterfaces();
                if (typeInterfaces.Length > 1)
                {
                    foreach (var typeInterface in typeInterfaces)
                    {
                        services.AddTransient(typeInterface, transientType);
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
                if (typeInterfaces.Length > 1)
                {
                    foreach (var typeInterface in typeInterfaces)
                    {
                        services.AddScoped(typeInterface, scopeType);
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
                if (typeInterfaces.Length > 1)
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
