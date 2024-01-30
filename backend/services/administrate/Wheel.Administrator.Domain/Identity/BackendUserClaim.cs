using Microsoft.AspNetCore.Identity;
using Wheel.Domain.Common;

namespace Wheel.Administrator.Domain.Identity
{
    public class BackendUserClaim : IdentityUserClaim<string>, IEntity<int>
    {
        public virtual BackendUser User { get; set; }
    }
}
