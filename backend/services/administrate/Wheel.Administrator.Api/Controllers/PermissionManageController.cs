using Microsoft.AspNetCore.Mvc;
using Wheel.Administrator.Controllers;
using Wheel.Administrator.Services.PermissionManage;
using Wheel.Administrator.Services.PermissionManage.Dtos;
using Wheel.Core.Dto;

namespace Wheel.Controllers
{
    /// <summary>
    /// 权限管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionManageController
        (IPermissionManageAppService permissionManageAppService) : AdministratorControllerBase
    {
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public Task<R<List<GetAllPermissionDto>>> GetPermission()
        {
            return permissionManageAppService.GetPermission();
        }
        /// <summary>
        /// 获取指定角色权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("{role}")]
        public Task<R<List<GetAllPermissionDto>>> GetRolePermission(string role)
        {
            return permissionManageAppService.GetRolePermission(role);
        }
        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<R> UpdatePermission(UpdatePermissionDto dto)
        {
            return await permissionManageAppService.UpdatePermission(dto);
        }
    }
}
