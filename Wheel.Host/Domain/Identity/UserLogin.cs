using Microsoft.AspNetCore.Identity;

namespace Wheel.Domain.Identity
{
    public class UserLogin : IdentityUserLogin<long>
    {
        public virtual User User { get; set; }
    }
}
