using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Wheel.Localization
{
    public class EFStringLocalizerFactory(IServiceProvider serviceProvider) : IStringLocalizerFactory
    {
        public IStringLocalizer Create(Type resourceSource)
        {
            var scope = serviceProvider.CreateScope();
            var store = scope.ServiceProvider.GetRequiredService<IStringLocalizerStore>();
            var cahce = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
            return new EFStringLocalizer(cahce, store);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            var scope = serviceProvider.CreateScope();
            var store = scope.ServiceProvider.GetRequiredService<IStringLocalizerStore>();
            var cahce = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
            return new EFStringLocalizer(cahce, store);
        }
    }
}
