using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace Wheel.Localization
{
    public class EFStringLocalizer : IStringLocalizer
    {
        private readonly IStringLocalizerStore _localizerStore;
        private readonly IMemoryCache _memoryCache;
        public EFStringLocalizer(IMemoryCache memoryCache, IStringLocalizerStore localizerStore)
        {
            _memoryCache = memoryCache;
            _localizerStore = localizerStore;
        }

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
            return new EFStringLocalizer(_memoryCache, _localizerStore);
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures)
        {
            return _localizerStore.GetAllStrings();
        }

        private string? GetString(string name)
        {
            if (_memoryCache.TryGetValue<string>($"{CultureInfo.CurrentCulture.Name}:{name}", out var value))
            {
                return value;
            }
            else
            {
                value = _localizerStore.GetString(name);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _memoryCache.Set($"{CultureInfo.CurrentCulture.Name}:{name}", value, TimeSpan.FromMinutes(1));
                }
                return value;
            }
        }
    }

    public class EFStringLocalizer<T> : IStringLocalizer<T>
    {
        private readonly IStringLocalizerStore _localizerStore;
        private readonly IMemoryCache _memoryCache;
        public EFStringLocalizer(IMemoryCache memoryCache, IStringLocalizerStore localizerStore)
        {
            _memoryCache = memoryCache;
            _localizerStore = localizerStore;
        }

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
            return new EFStringLocalizer(_memoryCache, _localizerStore);
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures)
        {
            return _localizerStore.GetAllStrings();
        }

        private string? GetString(string name)
        {
            if (_memoryCache.TryGetValue<string>($"{CultureInfo.CurrentCulture.Name}:{name}", out var value))
            {
                return value;
            }
            else
            {
                value = _localizerStore.GetString(name);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _memoryCache.Set($"{CultureInfo.CurrentCulture.Name}:{name}", value, TimeSpan.FromMinutes(1));
                }
                return value;
            }
        }
    }
}
