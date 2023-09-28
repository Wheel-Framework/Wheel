using Microsoft.AspNetCore.Identity;
using Wheel.Domain.Common;

namespace Wheel.Domain.Identity
{
    public class User : IdentityUser, IEntity<string>
    {
        public virtual DateTimeOffset CreationTime { get; set; }
        public virtual ICollection<UserClaim> Claims { get; set; }
        public virtual ICollection<UserLogin> Logins { get; set; }
        public virtual ICollection<UserToken> Tokens { get; set; }
        public virtual ICollection<UserRole>? UserRoles { get; set; }
    }
}
