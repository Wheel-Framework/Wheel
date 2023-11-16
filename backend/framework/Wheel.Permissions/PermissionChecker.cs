using Microsoft.Extensions.Caching.Distributed;
using Wheel.Authorization;
using Wheel.DependencyInjection;
using Wheel.Users;

namespace Wheel.Permission
{
    public class PermissionChecker(ICurrentUser currentUser, IDistributedCache distributedCache)
        : IPermissionChecker, ITransientDependency
    {
        public async Task<bool> Check(string controller, string action)
        {
            if (currentUser.IsInRoles("admin"))
                return true;
            foreach (var role in currentUser.Roles)
            {
                var permissions = await distributedCache.GetAsync<List<string>>($"Permission:R:{role}");
                if (permissions is null)
                    continue;
                if (permissions.Any(a => a == $"{controller}:{action}"))
                    return true;
            }
            return false;
        }
    }
}
