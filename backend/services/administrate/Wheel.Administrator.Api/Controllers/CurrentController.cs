using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wheel.Administrator.Controllers;
using Wheel.Administrator.Services.Menus;
using Wheel.Administrator.Services.Menus.Dtos;
using Wheel.Core.Dto;
using Wheel.Users;

namespace Wheel.Controllers
{
    /// <summary>
    /// 当前登录用户
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CurrentController(IMenuAppService menuAppService) : AdministratorControllerBase
    {
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
            return menuAppService.GetCurrentMenu();
        }
    }
}
