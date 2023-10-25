using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Wheel.Localization;

namespace Wheel
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEFStringLocalizer(this IServiceCollection services, Type stringLocalizerStoreType = null)
        {
            if (stringLocalizerStoreType == null)
            {
                services.TryAddTransient<IStringLocalizerStore, NullStringLocalizerStore>();
            }
            else
            {
                services.TryAddTransient(typeof(IStringLocalizerStore), stringLocalizerStoreType);
            }
            services.AddSingleton<IStringLocalizerFactory, EFStringLocalizerFactory>();
            return services;
        }
    }
}
