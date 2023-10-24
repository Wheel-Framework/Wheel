using Wheel.Domain.Common;

namespace Wheel.Domain.Settings
{
    public class SettingGroup : Entity
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public virtual ICollection<SettingValue> SettingValues { get; set; }
    }
}
