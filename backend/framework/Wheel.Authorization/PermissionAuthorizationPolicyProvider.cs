using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Wheel.DependencyInjection;

namespace Wheel.Authorization
{
    public class PermissionAuthorizationPolicyProvider
        (IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options), ITransientDependency
    {
        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            if (policy != null)
            {
                return policy;
            }
            if (policyName == "Permission")
            {
                var policyBuilder = new AuthorizationPolicyBuilder(Array.Empty<string>());
                policyBuilder.AddRequirements(new PermissionAuthorizationRequirement());
                return policyBuilder.Build();
            }
            return null;
        }
    }
}
