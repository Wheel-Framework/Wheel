using System.Text.RegularExpressions;
using Wheel.Domain.Common;

namespace Wheel.Domain.Localization
{
    public class LocalizationResource : IEntity<int>
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public virtual int CultureId { get; set; }
        public virtual LocalizationCulture Culture { get; set; }
    }
}
