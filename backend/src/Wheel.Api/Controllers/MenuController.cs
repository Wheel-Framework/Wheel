using Microsoft.AspNetCore.Mvc;
using Wheel.Core.Dto;
using Wheel.Services.Menus;
using Wheel.Services.Menus.Dtos;

namespace Wheel.Controllers
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : WheelControllerBase
    {
        private readonly IMenuAppService _menuAppService;

        public MenuController(IMenuAppService menuAppService)
        {
            _menuAppService = menuAppService;
        }
        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost()]
        public Task<R> Create(CreateOrUpdateMenuDto dto)
        {
            return _menuAppService.Create(dto);
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task<R> Delete(Guid id)
        {
            return _menuAppService.Delete(id);
        }
        /// <summary>
        /// 获取单个菜单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Task<R<MenuDto>> GetById(Guid id)
        {
            return _menuAppService.GetById(id);
        }
        /// <summary>
        /// 查询菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<R<List<MenuDto>>> GetList()
        {
            return _menuAppService.GetList();
        }
        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public Task<R> Update(Guid id, CreateOrUpdateMenuDto dto)
        {
            return _menuAppService.Update(id, dto);
        }
        /// <summary>
        /// 修改角色菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("role/{roleId}")]
        public Task<R> UpdateRoleMenu(string roleId, UpdateRoleMenuDto dto)
        {
            return _menuAppService.UpdateRoleMenu(roleId, dto);
        }
        /// <summary>
        /// 获取角色菜单列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("role/{roleId}")]
        public Task<R<List<MenuDto>>> GetRoleMenuList(string roleId)
        {
            return _menuAppService.GetRoleMenuList(roleId);
        }
    }
}
