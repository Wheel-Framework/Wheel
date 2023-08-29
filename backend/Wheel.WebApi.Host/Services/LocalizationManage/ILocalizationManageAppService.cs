using Wheel.Core.Http;
using Wheel.DependencyInjection;
using Wheel.Domain.Localization;
using Wheel.Services.LocalizationManage.Dtos;

namespace Wheel.Services.LocalizationManage
{
    public interface ILocalizationManageAppService : IScopeDependency
    {
        Task<LocalizationCultureDto?> GetLocalizationCultureAsync(int id);
        Task<Page<LocalizationCultureDto>?> GetLocalizationCulturePageListAsync(PageRequest input);
        Task<LocalizationCultureDto> CreateLocalizationCultureAsync(CreateLocalizationCultureDto input);
        Task DeleteLocalizationCultureAsync(int id);
    }
}
