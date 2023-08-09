using Microsoft.AspNetCore.Identity;

namespace Wheel.Domain.Identity
{
    public class UserToken : IdentityUserToken<long>
    {
        public virtual User User { get; set; }
    }
}
