using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Wheel.Settings;

namespace Wheel
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSetting(this IServiceCollection services, Type settingStoreType = null)
        {
            if (settingStoreType == null)
            {
                services.TryAddTransient<ISettingStore, NullSettingStore>();
            }
            else
            {
                services.TryAddTransient(typeof(ISettingStore), settingStoreType);
            }
            services.TryAddTransient<ISettingProvider, DefaultSettingProvider>();
            return services;
        }
    }
}
