using Microsoft.AspNetCore.Identity;

namespace Wheel.Administrator.Domain.Identity
{
    public class BackendUserToken : IdentityUserToken<string>
    {
        public virtual BackendUser User { get; set; }
    }
}
