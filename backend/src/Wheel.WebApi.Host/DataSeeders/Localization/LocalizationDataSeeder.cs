using Wheel.Domain;
using Wheel.Domain.Localization;

namespace Wheel.DataSeeders.Localization
{
    public class LocalizationDataSeeder(IBasicRepository<LocalizationCulture, int> localizationCultureRepository)
        : IDataSeeder
    {
        public async Task Seed(CancellationToken cancellationToken = default)
        {
            if (!(await localizationCultureRepository.AnyAsync(cancellationToken)))
            {
                await localizationCultureRepository.InsertAsync(new LocalizationCulture() { Name = "en" }, true);
                await localizationCultureRepository.InsertAsync(new LocalizationCulture() { Name = "zh-CN" }, true);
            }
        }
    }
}
