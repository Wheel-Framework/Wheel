using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;
using System.Xml;
using Wheel.Const;
using Wheel.Core.Exceptions;
using Wheel.DependencyInjection;
using Wheel.Domain;
using Wheel.Domain.Identity;
using Wheel.Domain.Permissions;
using Wheel.Services.PermissionManage.Dtos;
using Wheel.Utilities;

namespace Wheel.Services.PermissionManage
{
    public class PermissionManageAppService : WheelServiceBase, IPermissionManageAppService
    {
        private readonly IBasicRepository<PermissionGrant, Guid> _permissionGrantRepository;
        private readonly RoleManager<Role> _roleManager;
        private readonly XmlCommentHelper _xmlCommentHelper;
        public PermissionManageAppService(XmlCommentHelper xmlCommentHelper, IBasicRepository<PermissionGrant, Guid> permissionGrantRepository, RoleManager<Role> roleManager)
        {
            _xmlCommentHelper = xmlCommentHelper;
            _permissionGrantRepository = permissionGrantRepository;
            _roleManager = roleManager;
        }
        public async Task<List<GetAllPermissionDto>> GetPermission()
        {
            var result = MemoryCache.Get<List<GetAllPermissionDto>>("AllDefinePermission");
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
                MemoryCache.Set("AllDefinePermission", result);
            }
            // todo: 获取当前用户角色已有权限并标记
            if (CurrentUser.IsInRoles("admin"))
            {
                result.ForEach(p => p.Permissions.ForEach(a => a.IsGranted = true));
            }
            else
            {
                var grantPermissions = (await _permissionGrantRepository
                    .SelectListAsync(a => a.GrantType == "R" && CurrentUser.Roles.Contains(a.GrantValue), a => a.Permission))
                    .Distinct().ToList();

                foreach (var group in result)
                {
                    foreach (var permission in group.Permissions)
                    {
                        if (grantPermissions.Any(b => b == $"{group.Group}:{permission.Name}"))
                            permission.IsGranted = true;
                        else
                            permission.IsGranted = false;
                    }
                }
            }
            return result;
        }

        public async Task UpdatePermission(UpdatePermissionDto dto)
        {
            if(dto.Type == "R") 
            {
                var exsit = await _roleManager.RoleExistsAsync(dto.Value);
                if (!exsit)
                    throw new BusinessException(ErrorCode.RoleNotExist, "RoleNotExist")
                        .WithMessageDataData(dto.Value);
            }
            using (var tran = await UnitOfWork.BeginTransactionAsync())
            {
                await _permissionGrantRepository.DeleteAsync(a => a.GrantType == dto.Type && a.GrantValue == dto.Value);
                await _permissionGrantRepository.InsertManyAsync(dto.Permissions.Select(a=> new PermissionGrant 
                {
                    Id = GuidGenerator.Create(),
                    GrantType = dto.Type,
                    GrantValue = dto.Value,
                    Permission = a
                }).ToList());
                await DistributedCache.SetAsync($"Permission:{dto.Type}:{dto.Value}", dto.Permissions);
                await tran.CommitAsync();
            }
        }
    }
}
