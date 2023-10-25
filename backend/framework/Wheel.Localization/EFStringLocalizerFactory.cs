using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;

namespace Wheel.Localization
{
    public class EFStringLocalizerFactory : IStringLocalizerFactory
    {
        IServiceProvider _serviceProvider;
        public EFStringLocalizerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            var scope = _serviceProvider.CreateScope();
            var store = scope.ServiceProvider.GetRequiredService<IStringLocalizerStore>();
            var cahce = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
            return new EFStringLocalizer(cahce, store);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            var scope = _serviceProvider.CreateScope();
            var store = scope.ServiceProvider.GetRequiredService<IStringLocalizerStore>();
            var cahce = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
            return new EFStringLocalizer(cahce, store);
        }
    }
}
