using Microsoft.AspNetCore.Http;
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
        public Task Create(CreateOrUpdateMenuDto dto)
        {
            return _menuAppService.Create(dto);
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task Delete(Guid id)
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
        /// 分页查询菜单
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<Page<MenuDto>> GetPageList([FromQuery]PageRequest pageRequest)
        {
            return _menuAppService.GetPageList(pageRequest);
        }
        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public Task Update(Guid id, CreateOrUpdateMenuDto dto)
        {
            return _menuAppService.Update(id, dto);
        }
    }
}
