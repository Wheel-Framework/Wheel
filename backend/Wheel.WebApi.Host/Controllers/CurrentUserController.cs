using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wheel.Core.Dto;
using Wheel.Core.Users;

namespace Wheel.Controllers
{
    /// <summary>
    /// 当前登录用户
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CurrentUserController : WheelControllerBase
    {
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public Task<R<ICurrentUser>> GetCurrentUser()
        {
            return Task.FromResult(new R<ICurrentUser>(CurrentUser));
        }
    }
}
