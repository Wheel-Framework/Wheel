using Microsoft.Extensions.Caching.Distributed;
using Wheel.Authorization;
using Wheel.Users;
using Wheel.DependencyInjection;

namespace Wheel.Permission
{
    public class PermissionChecker : IPermissionChecker, ITransientDependency
    {
        private readonly ICurrentUser _currentUser;
        private readonly IDistributedCache _distributedCache;

        public PermissionChecker(ICurrentUser currentUser, IDistributedCache distributedCache)
        {
            _currentUser = currentUser;
            _distributedCache = distributedCache;
        }

        public async Task<bool> Check(string controller, string action)
        {
            if (_currentUser.IsInRoles("admin"))
                return true;
            foreach (var role in _currentUser.Roles)
            {
                var permissions = await _distributedCache.GetAsync<List<string>>($"Permission:R:{role}");
                if (permissions is null)
                    continue;
                if (permissions.Any(a => a == $"{controller}:{action}"))
                    return true;
            }
            return false;
        }
    }
}
