using Microsoft.AspNetCore.Identity;
using Wheel.Domain.Common;

namespace Wheel.Administrator.Domain.Identity
{
    public class BackendRole : IdentityRole, IEntity<string>
    {

        public BackendRole(string roleName) : base(roleName)
        {
        }

        public BackendRole() : base()
        {
        }

        public virtual ICollection<BackendUserRole> UserRoles { get; set; }
        public virtual ICollection<BackendRoleClaim> RoleClaims { get; set; }
    }
}
