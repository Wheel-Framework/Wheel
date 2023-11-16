using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Wheel.Users
{
    public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
    {
        private ClaimsPrincipal User = httpContextAccessor.HttpContext.User;

        public bool IsAuthenticated => User.Identity.IsAuthenticated;
        public string? Id => User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        public string UserName => User.Identity.Name;

        public string[] Roles => User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();


        public bool IsInRoles(string role)
        {
            return User.IsInRole(role);
        }
    }
}
