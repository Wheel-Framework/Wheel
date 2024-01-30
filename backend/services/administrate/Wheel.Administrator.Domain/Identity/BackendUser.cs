using Microsoft.AspNetCore.Identity;
using Wheel.Domain.Common;

namespace Wheel.Administrator.Domain.Identity
{
    public class BackendUser : IdentityUser, IEntity<string>, IHasCreationTime
    {
        public virtual DateTimeOffset CreationTime { get; set; }
        public virtual ICollection<BackendUserClaim> Claims { get; set; }
        public virtual ICollection<BackendUserLogin> Logins { get; set; }
        public virtual ICollection<BackendUserToken> Tokens { get; set; }
        public virtual ICollection<BackendUserRole>? UserRoles { get; set; }
    }
}
