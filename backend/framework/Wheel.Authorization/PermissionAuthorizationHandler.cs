using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Wheel.DependencyInjection;

namespace Wheel.Authorization
{
    public class PermissionAuthorizationHandler
        (IPermissionChecker permissionChecker) : AuthorizationHandler<PermissionAuthorizationRequirement>,
            ITransientDependency
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            if (context.Resource is HttpContext httpContext)
            {
                var actionDescriptor = httpContext.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();
                var controllerName = actionDescriptor?.ControllerName;
                var actionName = actionDescriptor?.ActionName;
                if (await permissionChecker.Check(controllerName, actionName))
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
