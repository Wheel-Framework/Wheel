using Wheel.Core.Dto;
using Wheel.DependencyInjection;
using Wheel.Domain.Localization;
using Wheel.Services.LocalizationManage.Dtos;

namespace Wheel.Services.LocalizationManage
{
    public interface ILocalizationManageAppService : ITransientDependency
    {
        Task<R<LocalizationCultureDto>> GetLocalizationCultureAsync(int id);
        Task<Page<LocalizationCultureDto>> GetLocalizationCulturePageListAsync(PageRequest input);
        Task<R<LocalizationCultureDto>> CreateLocalizationCultureAsync(CreateLocalizationCultureDto input);
        Task<R> DeleteLocalizationCultureAsync(int id);
        Task<R<LocalizationResourceDto>> CreateLocalizationResourceAsync(CreateLocalizationResourceDto input);
        Task<R> UpdateLocalizationResourceAsync(UpdateLocalizationResourceDto input);
        Task<R> DeleteLocalizationResourceAsync(int id);
    }
}
