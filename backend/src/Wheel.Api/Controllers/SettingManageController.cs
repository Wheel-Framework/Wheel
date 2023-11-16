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
    public class SettingManageController(ISettingManageAppService settingManageAppService) : WheelControllerBase
    {
        /// <summary>
        /// 获取所有设置
        /// </summary>
        /// <param name="settingScope">设置范围</param>
        /// <returns></returns>
        [HttpGet()]
        public Task<R<List<SettingGroupDto>>> GetAllSettingGroup(SettingScope settingScope = SettingScope.Global)
        {
            return settingManageAppService.GetAllSettingGroup(settingScope);
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
            return settingManageAppService.UpdateSettings(settingGroupDto, settingScope);
        }
    }
}
