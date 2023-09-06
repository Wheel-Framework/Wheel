using Microsoft.AspNetCore.Identity;
using Wheel.Domain.Common;

namespace Wheel.Domain.Identity
{
    public class Role : IdentityRole, IEntity<string>
    {
        public Role(string roleName) : base (roleName)
        {
        }

        public Role() : base ()
        {
        }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
    }
}
