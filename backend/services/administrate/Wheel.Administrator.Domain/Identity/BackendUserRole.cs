using Microsoft.AspNetCore.Identity;

namespace Wheel.Administrator.Domain.Identity
{
    public class BackendUserRole : IdentityUserRole<string>
    {
        public virtual BackendUser User { get; set; }
        public virtual BackendRole Role { get; set; }
    }
}
