using Microsoft.Extensions.Localization;

namespace Wheel
{
    public class NullStringLocalizerStore : IStringLocalizerStore
    {
        public IEnumerable<LocalizedString> GetAllStrings()
        {
            return null;
        }

        public string GetString(string name)
        {
            return null;
        }
    }
}
