using Wheel.DependencyInjection;
using Wheel.Domain.Localization;

namespace Wheel.Services.LocalizationManage
{
    public interface ILocalizationManageAppService : IScopeDependency
    {
        Task<LocalizationCulture> GetLocalizationCulture(int id);
    }
}
