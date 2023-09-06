using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc.Controllers;
using Wheel.DependencyInjection;

namespace Wheel.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>, ISingletonDependency
    {
        private readonly IPermissionChecker _permissionChecker;

        public PermissionAuthorizationHandler(IPermissionChecker permissionChecker)
        {
            _permissionChecker = permissionChecker;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            if (context.Resource is HttpContext httpContext)
            {
                var actionDescriptor = httpContext.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();
                var controllerName = actionDescriptor?.ControllerName;
                var actionName = actionDescriptor?.ActionName;
                if (await _permissionChecker.Check(controllerName, actionName))
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
