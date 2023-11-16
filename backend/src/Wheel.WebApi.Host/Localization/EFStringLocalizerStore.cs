﻿using Microsoft.Extensions.Localization;
using System.Globalization;
using Wheel.DependencyInjection;
using Wheel.Domain;
using Wheel.Domain.Localization;

namespace Wheel.Localization
{
    public class EFStringLocalizerStore(IBasicRepository<LocalizationResource, int> resourceRepository)
        : IStringLocalizerStore, ISingletonDependency
    {
        public IEnumerable<LocalizedString> GetAllStrings()
        {
            var list = resourceRepository.GetListAsync(r => r.Culture.Name == CultureInfo.CurrentCulture.Name, propertySelectors: r => r.Culture).ConfigureAwait(false).GetAwaiter().GetResult();
            return list
                .Select(r => new LocalizedString(r.Key, r.Value, r.Value == null));
        }

        public string GetString(string name)
        {
            var resource = resourceRepository.FindAsync(r => r.Culture.Name == CultureInfo.CurrentCulture.Name).ConfigureAwait(false).GetAwaiter().GetResult();
            return resource?.Value;
        }
    }
}
