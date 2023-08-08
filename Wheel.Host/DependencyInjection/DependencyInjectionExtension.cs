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
            var abs = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in abs)
            {
                var transientTypes = assembly.GetTypes()
                    .Where(t => !t.IsInterface)
                    .Where(t => t.GetInterface(nameof(ITransientDependency))!= null);
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
                var scopeTypes = assembly.GetTypes()
                    .Where(t => !t.IsInterface)
                    .Where(t => t.GetInterface(nameof(IScopeDependency))!= null);
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
                var singletonTypes = assembly.GetTypes()
                    .Where(t => !t.IsInterface)
                    .Where(t => t.GetInterface(nameof(ISingletonDependency))!= null);
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
            }
            return services;
        }
    }
}
