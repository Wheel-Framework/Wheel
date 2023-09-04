using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Distributed;
using System.Reflection;
using System.Text.Json;
using System.Xml;
using Wheel.DependencyInjection;
using Wheel.Domain;
using Wheel.Services.PermissionManage.Dtos;
using Wheel.Utilities;

namespace Wheel.Services.PermissionManage
{
    public class PermissionManageAppService : WheelServiceBase, IPermissionManageAppService
    {
        private readonly XmlCommentHelper _xmlCommentHelper;
        public PermissionManageAppService(XmlCommentHelper xmlCommentHelper)
        {
            _xmlCommentHelper = xmlCommentHelper;
        }
        public async Task<List<GetAllPermissionDto>> GetAllPermission()
        {
            var result = await DistributedCache.GetAsync<List<GetAllPermissionDto>>("AllDefinePermission");
            if(result == null)
            {
                result = new List<GetAllPermissionDto>();
                var apiDescriptionGroupCollectionProvider = ServiceProvider.GetRequiredService<IApiDescriptionGroupCollectionProvider>();
                var apiDescriptionGroups = apiDescriptionGroupCollectionProvider.ApiDescriptionGroups.Items.SelectMany(group => group.Items)
                    .Where(a => a.ActionDescriptor is ControllerActionDescriptor)
                    .GroupBy(a => (a.ActionDescriptor as ControllerActionDescriptor).ControllerTypeInfo);

                foreach (var apiDescriptions in apiDescriptionGroups)
                {
                    var permissionGroup = new GetAllPermissionDto();
                    var controllerTypeInfo = apiDescriptions.Key;

                    var controllerAllowAnonymous = controllerTypeInfo.GetAttribute<AllowAnonymousAttribute>();

                    var controllerComment = _xmlCommentHelper.GetTypeComment(controllerTypeInfo);

                    permissionGroup.Group = controllerTypeInfo.Name;
                    permissionGroup.Summary = controllerComment;
                    foreach (var apiDescription in apiDescriptions)
                    {
                        var method = controllerTypeInfo.GetMethod(apiDescription.ActionDescriptor.RouteValues["action"]);
                        var actionAllowAnonymous = method.GetAttribute<AllowAnonymousAttribute>();
                        var actionAuthorize = method.GetAttribute<AuthorizeAttribute>();
                        if ((controllerAllowAnonymous == null && actionAllowAnonymous == null) || actionAuthorize != null)
                        {
                            var methodComment = _xmlCommentHelper.GetMethodComment(method);
                            permissionGroup.Permissions.Add(new PermissionDto { Name = method.Name, Summary = methodComment });
                        }
                    }
                    if (permissionGroup.Permissions.Count > 0)
                        result.Add(permissionGroup);
                }
                await DistributedCache.SetAsync("AllDefinePermission", result);
            }
            // todo: 获取当前用户角色已有权限并标记
            return result;
        }
    }
}
