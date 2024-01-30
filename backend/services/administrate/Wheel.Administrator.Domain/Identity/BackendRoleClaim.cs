using Microsoft.AspNetCore.Identity;
using Wheel.Domain.Common;

namespace Wheel.Administrator.Domain.Identity
{
    public class BackendRoleClaim : IdentityRoleClaim<string>, IEntity<int>
    {
        public virtual BackendRole Role { get; set; }
    }
}
