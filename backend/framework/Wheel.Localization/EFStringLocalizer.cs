using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace Wheel.Localization
{
    public class EFStringLocalizer(IMemoryCache memoryCache, IStringLocalizerStore localizerStore)
        : IStringLocalizer
    {
        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            CultureInfo.DefaultThreadCurrentCulture = culture;
            return new EFStringLocalizer(memoryCache, localizerStore);
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures)
        {
            return localizerStore.GetAllStrings();
        }

        private string? GetString(string name)
        {
            if (memoryCache.TryGetValue<string>($"{CultureInfo.CurrentCulture.Name}:{name}", out var value))
            {
                return value;
            }
            else
            {
                value = localizerStore.GetString(name);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    memoryCache.Set($"{CultureInfo.CurrentCulture.Name}:{name}", value, TimeSpan.FromMinutes(1));
                }
                return value;
            }
        }
    }

    public class EFStringLocalizer<T>(IMemoryCache memoryCache, IStringLocalizerStore localizerStore)
        : IStringLocalizer<T>
    {
        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            CultureInfo.DefaultThreadCurrentCulture = culture;
            return new EFStringLocalizer(memoryCache, localizerStore);
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures)
        {
            return localizerStore.GetAllStrings();
        }

        private string? GetString(string name)
        {
            if (memoryCache.TryGetValue<string>($"{CultureInfo.CurrentCulture.Name}:{name}", out var value))
            {
                return value;
            }
            else
            {
                value = localizerStore.GetString(name);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    memoryCache.Set($"{CultureInfo.CurrentCulture.Name}:{name}", value, TimeSpan.FromMinutes(1));
                }
                return value;
            }
        }
    }
}
