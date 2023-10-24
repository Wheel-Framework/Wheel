using Wheel.Domain;
using Wheel.Domain.Localization;

namespace Wheel.DataSeeders.Localization
{
    public class LocalizationDataSeeder : IDataSeeder
    {
        private readonly IBasicRepository<LocalizationCulture, int> _localizationCultureRepository;

        public LocalizationDataSeeder(IBasicRepository<LocalizationCulture, int> localizationCultureRepository)
        {
            _localizationCultureRepository = localizationCultureRepository;
        }

        public async Task Seed(CancellationToken cancellationToken = default)
        {
            if (!(await _localizationCultureRepository.AnyAsync(cancellationToken)))
            {
                await _localizationCultureRepository.InsertAsync(new LocalizationCulture() { Name = "en" }, true);
                await _localizationCultureRepository.InsertAsync(new LocalizationCulture() { Name = "zh-CN"}, true);
            }
        }
    }
}
