using Castle.Core.Resource;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;
using Wheel.DependencyInjection;
using Wheel.Domain.Localization;
using Wheel.EntityFrameworkCore;

namespace Wheel.Localization
{
    public class EFStringLocalizerFactory : IStringLocalizerFactory, ISingletonDependency
    {
        IServiceProvider _serviceProvider;
        public EFStringLocalizerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<WheelDbContext>();
            var cahce = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
            return new EFStringLocalizer(db, cahce);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<WheelDbContext>();
            var cahce = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
            return new EFStringLocalizer(db, cahce);
        }
    }
}
