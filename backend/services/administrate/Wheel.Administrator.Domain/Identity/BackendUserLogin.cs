using Microsoft.AspNetCore.Identity;

namespace Wheel.Administrator.Domain.Identity
{
    public class BackendUserLogin : IdentityUserLogin<string>
    {
        public virtual BackendUser User { get; set; }
    }
}
