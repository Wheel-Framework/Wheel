using Castle.Core.Resource;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;
using Wheel.Domain.Localization;
using Wheel.EntityFrameworkCore;

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
            var db = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<WheelDbContext>();
            return new EFStringLocalizer(db);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            var db = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<WheelDbContext>();
            return new EFStringLocalizer(db);
        }
    }
}
