using Microsoft.AspNetCore.Identity;
using Wheel.Core.Dto;

namespace Wheel.Services.Users.Dtos
{
    public class UserPageRequest : PageRequest
    {

        public virtual string? UserName { get; set; }

        public virtual string? Email { get; set; }

        public virtual bool? EmailConfirmed { get; set; }

        public virtual string? PhoneNumber { get; set; }
        public virtual bool? PhoneNumberConfirmed { get; set; }

        public virtual DateTimeOffset? CreationTimeFrom { get; set; }
        public virtual DateTimeOffset? CreationTimeTo { get; set; }

    }
}
