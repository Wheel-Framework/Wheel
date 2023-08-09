using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Wheel.Domain.Common;

namespace Wheel.Domain.Identity
{
    public class UserClaim : IdentityUserClaim<long>, IEntity<int>
    {
        public virtual User User { get; set; }
    }
}
