using Microsoft.AspNetCore.Identity;

namespace Wheel.Domain.Identity
{
    public class UserLogin : IdentityUserLogin<string>
    {
        public virtual User User { get; set; }
    }
}
