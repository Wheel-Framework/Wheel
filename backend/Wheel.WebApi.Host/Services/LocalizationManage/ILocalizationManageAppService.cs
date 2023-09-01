using Wheel.Core.Http;
using Wheel.DependencyInjection;
using Wheel.Domain.Localization;
using Wheel.Services.LocalizationManage.Dtos;

namespace Wheel.Services.LocalizationManage
{
    public interface ILocalizationManageAppService : ITransientDependency
    {
        Task<LocalizationCultureDto?> GetLocalizationCultureAsync(int id);
        Task<Page<LocalizationCultureDto>?> GetLocalizationCulturePageListAsync(PageRequest input);
        Task<LocalizationCultureDto> CreateLocalizationCultureAsync(CreateLocalizationCultureDto input);
        Task DeleteLocalizationCultureAsync(int id);
        Task<LocalizationResourceDto> CreateLocalizationResourceAsync(CreateLocalizationResourceDto input);
        Task UpdateLocalizationResourceAsync(UpdateLocalizationResourceDto input);
        Task DeleteLocalizationResourceAsync(int id);
    }
}
