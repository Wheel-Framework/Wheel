using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Wheel.Localization;

namespace Wheel
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEFStringLocalizer<TStore>(this IServiceCollection services) where TStore : class, IStringLocalizerStore
        {
            services.AddTransient<IStringLocalizerStore, TStore>();
            services.AddSingleton<IStringLocalizerFactory, EFStringLocalizerFactory>();
            return services;
        }
    }
}
