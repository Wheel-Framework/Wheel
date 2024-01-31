using Microsoft.AspNetCore.Mvc;
using Wheel.Administrator.Roles;
using Wheel.Administrator.Roles.Dtos;
using Wheel.Core.Dto;

namespace Wheel.Administrator.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RoleManageController(IRoleManageAppService roleManageAppService) : AdministratorControllerBase
    {
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<R> CreateRole(CreateRoleDto dto)
        {
            return await roleManageAppService.CreateRole(dto);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<R> DeleteRole(string roleName)
        {
            return await roleManageAppService.DeleteRole(roleName);
        }
        /// <summary>
        /// 角色分页查询
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <returns></returns>
        [HttpGet("Page")]
        public async Task<Page<RoleDto>> GetRolePageList([FromQuery] PageRequest pageRequest)
        {
            return await roleManageAppService.GetRolePageList(pageRequest);
        }
    }
}
