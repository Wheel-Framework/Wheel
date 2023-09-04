using System.Security.Claims;
using Wheel.Domain.Identity;

namespace Wheel.Core.Users
{
    public class CurrentUser : ICurrentUser
    {
        private ClaimsPrincipal User;
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            User = httpContextAccessor.HttpContext.User;
        }

        public bool IsAuthenticated => User.Identity.IsAuthenticated;

        public string UserName => User.Identity.Name;

        public string[] Roles => User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();

        public bool IsInRoles(string role)
        {
            return User.IsInRole(role);
        }
    }
}
