using Microsoft.Extensions.Localization;

namespace Wheel
{
    public interface IStringLocalizerStore
    {
        string GetString(string name);
        IEnumerable<LocalizedString> GetAllStrings();
    }
}
