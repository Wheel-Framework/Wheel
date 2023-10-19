using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wheel.Core.Dto;
using Wheel.Enums;
using Wheel.Services.SettingManage;
using Wheel.Services.SettingManage.Dtos;

namespace Wheel.Controllers
{
    /// <summary>
    /// 设置管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class SettingManageController : WheelControllerBase
    {
        private readonly ISettingManageAppService _settingManageAppService;

        public SettingManageController(ISettingManageAppService settingManageAppService)
        {
            _settingManageAppService = settingManageAppService;
        }

        /// <summary>
        /// 获取所有设置
        /// </summary>
        /// <param name="settingScope">设置范围</param>
        /// <returns></returns>
        [HttpGet()]
        public Task<R<List<SettingGroupDto>>> GetAllSettingGroup(SettingScope settingScope = SettingScope.Golbal)
        {
            return _settingManageAppService.GetAllSettingGroup(settingScope);
        }
        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="settingGroupDto">设置组数据</param>
        /// <param name="settingScope">设置范围</param>
        /// <returns></returns>
        [HttpPut("{settingScope}")]
        public Task<R> UpdateSettings(SettingGroupDto settingGroupDto, SettingScope settingScope)
        {
            return _settingManageAppService.UpdateSettings(settingGroupDto, settingScope);
        }
    }
}
