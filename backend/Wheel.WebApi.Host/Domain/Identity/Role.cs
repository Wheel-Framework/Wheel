using Microsoft.AspNetCore.Identity;
using Wheel.Domain.Common;

namespace Wheel.Domain.Identity
{
    public class Role : IdentityRole, IEntity<string>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
    }
}
