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

        [HttpGet()]
        public Task<R<List<SettingGroupDto>>> GetAllSettingGroup(SettingScope settingScope = SettingScope.Golbal, string? settingScopeKey = null)
        {
            return _settingManageAppService.GetAllSettingGroup(settingScope, settingScopeKey);
        }

        [HttpPut()]
        public Task<R> UpdateSettings(SettingGroupDto settingGroupDto, [FromQuery]SettingScope settingScope, [FromQuery]string? settingScopeKey)
        {
            return _settingManageAppService.UpdateSettings(settingGroupDto, settingScope, settingScopeKey);
        }
    }
}
