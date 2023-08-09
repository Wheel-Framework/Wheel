using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Wheel.Domain.Common;

namespace Wheel.Domain.Identity
{
    public class RoleClaim : IdentityRoleClaim<long>, IEntity<int>
    {
        public virtual Role Role { get; set; }
    }
}
