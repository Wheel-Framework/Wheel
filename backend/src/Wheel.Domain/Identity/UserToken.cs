using Microsoft.AspNetCore.Identity;

namespace Wheel.Domain.Identity
{
    public class UserToken : IdentityUserToken<string>
    {
        public virtual User User { get; set; }
    }
}
