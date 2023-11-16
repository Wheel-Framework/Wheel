using Microsoft.AspNetCore.Mvc;
using Wheel.Core.Dto;
using Wheel.Services.Users;
using Wheel.Services.Users.Dtos;

namespace Wheel.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserManageController(IUserManageAppService userManageAppService) : WheelControllerBase
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<R> CreateUser(CreateUserDto userDto)
        {
            return userManageAppService.CreateUser(userDto);
        }
        /// <summary>
        /// 用户分页查询
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<Page<UserDto>> GetUserPageList([FromQuery] UserPageRequest pageRequest)
        {
            return userManageAppService.GetUserPageList(pageRequest);
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="updateUserDto"></param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        public Task<R> UpdateUser(string userId, UpdateUserDto updateUserDto)
        {
            return userManageAppService.UpdateUser(userId, updateUserDto);
        }
    }
}
