using Wheel.Domain.Common;

namespace Wheel.Domain.Permissions
{
    public class PermissionGrant : Entity<Guid>
    {
        public string Permission { get; set; }
        public string GrantType { get; set; }
        public string GrantValue { get; set; }
    }
}
