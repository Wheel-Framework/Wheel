using Microsoft.Extensions.Localization;
using System.Globalization;
using Wheel.DependencyInjection;
using Wheel.Domain;
using Wheel.Domain.Localization;

namespace Wheel.Localization
{
    public class EFStringLocalizerStore : IStringLocalizerStore, ISingletonDependency
    {
        private readonly IBasicRepository<LocalizationResource, int> _resourceRepository;

        public EFStringLocalizerStore(IBasicRepository<LocalizationResource, int> resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public IEnumerable<LocalizedString> GetAllStrings()
        {
            var list = _resourceRepository.GetListAsync(r => r.Culture.Name == CultureInfo.CurrentCulture.Name, propertySelectors: r => r.Culture).ConfigureAwait(false).GetAwaiter().GetResult();
            return list
                .Select(r => new LocalizedString(r.Key, r.Value, r.Value == null));
        }

        public string GetString(string name)
        {
            var resource = _resourceRepository.FindAsync(r => r.Culture.Name == CultureInfo.CurrentCulture.Name).ConfigureAwait(false).GetAwaiter().GetResult();
            return resource?.Value;
        }
    }
}
