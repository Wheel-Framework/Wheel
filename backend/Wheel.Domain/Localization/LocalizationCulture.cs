using Wheel.Domain.Common;

namespace Wheel.Domain.Localization
{
    public class LocalizationCulture : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<LocalizationResource> Resources { get; set; }
    }
}
