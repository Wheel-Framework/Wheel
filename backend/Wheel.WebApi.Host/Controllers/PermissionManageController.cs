using Microsoft.AspNetCore.Mvc;
using Wheel.Core.Http;
using Wheel.Services.PermissionManage;
using Wheel.Services.PermissionManage.Dtos;

namespace Wheel.Controllers
{
    /// <summary>
    /// 权限管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionManageController : WheelControllerBase
    {
        private readonly IPermissionManageAppService _permissionManageAppService;
        public PermissionManageController(IPermissionManageAppService permissionManageAppService)
        {
            _permissionManageAppService = permissionManageAppService;
        }
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<R<List<GetAllPermissionDto>>> GetPermission()
        {
            var result = await _permissionManageAppService.GetPermission();
            return new R<List<GetAllPermissionDto>>(result);
        }
        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<R> UpdatePermission(UpdatePermissionDto dto)
        {
            await _permissionManageAppService.UpdatePermission(dto);
            return new R();
        }
    }
}
