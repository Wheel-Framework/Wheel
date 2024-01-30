using Wheel.Core.Dto;
using Wheel.DependencyInjection;
using Wheel.Enums;
using Wheel.Administrator.Services.SettingManage.Dtos;

namespace Wheel.Administrator.Services.SettingManage
{
    public interface ISettingManageAppService : ITransientDependency
    {
        Task<R<List<SettingGroupDto>>> GetAllSettingGroup(SettingScope settingScope = SettingScope.Global);
        Task<R> UpdateSettings(SettingGroupDto settingGroupDto, SettingScope settingScope = SettingScope.Global);
    }
}
