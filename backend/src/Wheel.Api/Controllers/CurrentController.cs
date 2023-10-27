using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wheel.Core.Dto;
using Wheel.Services.Menus;
using Wheel.Services.Menus.Dtos;
using Wheel.Users;

namespace Wheel.Controllers
{
    /// <summary>
    /// 当前登录用户
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CurrentController : WheelControllerBase
    {
        private readonly IMenuAppService _menuAppService;

        public CurrentController(IMenuAppService menuAppService)
        {
            _menuAppService = menuAppService;
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("user")]
        public Task<R<ICurrentUser>> GetCurrentUser()
        {
            return Task.FromResult(new R<ICurrentUser>(CurrentUser));
        }

        /// <summary>
        /// 获取当前用户菜单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("menu")]
        public Task<R<List<AntdMenuDto>>> GetCurrentMenu()
        {
            return _menuAppService.GetCurrentMenu();
        }
    }
}
