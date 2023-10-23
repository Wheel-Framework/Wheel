using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Wheel.Domain.Common;

namespace Wheel.Domain.Identity
{
    public class UserClaim : IdentityUserClaim<string>, IEntity<int>
    {
        public virtual User User { get; set; }
    }
}
