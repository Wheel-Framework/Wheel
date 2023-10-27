using Microsoft.AspNetCore.Mvc;
using Wheel.Core.Dto;
using Wheel.Services.Roles;
using Wheel.Services.Roles.Dtos;

namespace Wheel.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RoleManageController : WheelControllerBase
    {
        private readonly IRoleManageAppService _roleManageAppService;

        public RoleManageController(IRoleManageAppService roleManageAppService)
        {
            _roleManageAppService = roleManageAppService;
        }
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<R> CreateRole(CreateRoleDto dto)
        {
            return await _roleManageAppService.CreateRole(dto);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<R> DeleteRole(string roleName)
        {
            return await _roleManageAppService.DeleteRole(roleName);
        }
        /// <summary>
        /// 角色分页查询
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <returns></returns>
        [HttpGet("Page")]
        public async Task<Page<RoleDto>> GetRolePageList([FromQuery] PageRequest pageRequest)
        {
            return await _roleManageAppService.GetRolePageList(pageRequest);
        }
    }
}
