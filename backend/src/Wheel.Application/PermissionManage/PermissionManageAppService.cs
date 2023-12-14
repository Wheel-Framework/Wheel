using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Wheel.Const;
using Wheel.Core.Dto;
using Wheel.Core.Exceptions;
using Wheel.Domain;
using Wheel.Domain.Identity;
using Wheel.Domain.Permissions;
using Wheel.Services.PermissionManage.Dtos;
using Wheel.Utilities;

namespace Wheel.Services.PermissionManage
{
    public class PermissionManageAppService(XmlCommentHelper xmlCommentHelper,
            IBasicRepository<PermissionGrant, Guid> permissionGrantRepository, RoleManager<Role> roleManager)
        : WheelServiceBase, IPermissionManageAppService
    {
        public async Task<R<List<GetAllPermissionDto>>> GetPermission()
        {
            var result = await GetAllDefinePermission();
            // todo: 获取当前用户角色已有权限并标记
            if (CurrentUser.IsInRoles("admin"))
            {
                result.ForEach(p => p.Permissions.ForEach(a => a.IsGranted = true));
            }
            else
            {
                var grantPermissions = (await permissionGrantRepository
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
            return Success(result);
        }
        public async Task<R<List<GetAllPermissionDto>>> GetRolePermission(string RoleName)
        {
            var result = await GetAllDefinePermission();

            var grantPermissions = (await permissionGrantRepository
                .SelectListAsync(a => a.GrantType == "R" && RoleName == a.GrantValue, a => a.Permission))
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

            return Success(result);
        }
        public async Task<R> UpdatePermission(UpdatePermissionDto dto)
        {
            if (dto.Type == "R")
            {
                var exsit = await roleManager.RoleExistsAsync(dto.Value);
                if (!exsit)
                    throw new BusinessException(ErrorCode.RoleNotExist, "RoleNotExist")
                        .WithMessageDataData(dto.Value);
            }
            using (var tran = await UnitOfWork.BeginTransactionAsync())
            {
                await permissionGrantRepository.DeleteAsync(a => a.GrantType == dto.Type && a.GrantValue == dto.Value);
                await permissionGrantRepository.InsertManyAsync(dto.Permissions.Select(a => new PermissionGrant
                {
                    Id = GuidGenerator.Create(),
                    GrantType = dto.Type,
                    GrantValue = dto.Value,
                    Permission = a
                }).ToList());
                await DistributedCache.SetAsync($"Permission:{dto.Type}:{dto.Value}", dto.Permissions);
                await tran.CommitAsync();
            }
            return Success();
        }

        private ValueTask<List<GetAllPermissionDto>> GetAllDefinePermission()
        {

            var result = MemoryCache.Get<List<GetAllPermissionDto>>("AllDefinePermission");
            if (result == null)
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

                    var controllerAllowAnonymous = controllerTypeInfo.GetCustomAttribute<AllowAnonymousAttribute>();

                    var controllerComment = xmlCommentHelper.GetTypeComment(controllerTypeInfo);

                    permissionGroup.Group = controllerTypeInfo.Name;
                    permissionGroup.Summary = controllerComment;
                    foreach (var apiDescription in apiDescriptions)
                    {
                        var method = controllerTypeInfo.GetMethod(apiDescription.ActionDescriptor.RouteValues["action"]);
                        var actionAllowAnonymous = method.GetCustomAttribute<AllowAnonymousAttribute>();
                        var actionAuthorize = method.GetCustomAttribute<AuthorizeAttribute>();
                        if ((controllerAllowAnonymous == null && actionAllowAnonymous == null) || actionAuthorize != null)
                        {
                            var methodComment = xmlCommentHelper.GetMethodComment(method);
                            permissionGroup.Permissions.Add(new PermissionDto { Name = method.Name, Summary = methodComment });
                        }
                    }
                    if (permissionGroup.Permissions.Count > 0)
                        result.Add(permissionGroup);
                }
                MemoryCache.Set("AllDefinePermission", result);
            }
            return ValueTask.FromResult(result);
        }
    }
}
