using Wheel.Domain;
using Wheel.Domain.Localization;

namespace Wheel.Services.LocalizationManage
{
    public class LocalizationManageAppService : ILocalizationManageAppService
    {
        private readonly IBasicRepository<LocalizationCulture, int> _localizationCultureRepository;
        private readonly IBasicRepository<LocalizationResource, int> _localizationResourceRepository;

        public LocalizationManageAppService(IBasicRepository<LocalizationCulture, int> localizationCultureRepository, IBasicRepository<LocalizationResource, int> localizationResourceRepository)
        {
            _localizationCultureRepository = localizationCultureRepository;
            _localizationResourceRepository = localizationResourceRepository;
        }

        public async Task<LocalizationCulture> GetLocalizationCulture(int id)
        {
            return await _localizationCultureRepository.FindAsync(id);
        }
    }
}
