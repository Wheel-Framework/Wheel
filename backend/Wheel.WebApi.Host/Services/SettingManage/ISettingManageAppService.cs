using Wheel.Core.Dto;
using Wheel.DependencyInjection;
using Wheel.Enums;
using Wheel.Services.SettingManage.Dtos;

namespace Wheel.Services.SettingManage
{
    public interface ISettingManageAppService : ITransientDependency
    {
        Task<R<List<SettingGroupDto>>> GetAllSettingGroup(SettingScope settingScope = SettingScope.Golbal);
        Task<R> UpdateSettings(SettingGroupDto settingGroupDto, SettingScope settingScope = SettingScope.Golbal);
    }
}
