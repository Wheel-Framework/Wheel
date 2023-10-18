using Wheel.Core.Dto;
using Wheel.Domain.Settings;
using Wheel.Domain;
using Wheel.Enums;
using Wheel.Services.SettingManage.Dtos;

namespace Wheel.Services.SettingManage
{
    public class SettingManageAppService : WheelServiceBase, ISettingManageAppService
    {
        private readonly IBasicRepository<SettingGroup, long> _settingGroupRepository;
        private readonly IBasicRepository<SettingValue, long> _settingValueRepository;
        private readonly SettingManager _settingManager;

        public SettingManageAppService(IBasicRepository<SettingGroup, long> settingGroupRepository, IBasicRepository<SettingValue, long> settingValueRepository, SettingManager settingManager)
        {
            _settingGroupRepository = settingGroupRepository;
            _settingValueRepository = settingValueRepository;
            _settingManager = settingManager;
        }

        public async Task<R<List<SettingGroupDto>>> GetAllSettingGroup(SettingScope settingScope = SettingScope.Golbal, string? settingScopeKey = null)
        {
            var settingGroups = await _settingGroupRepository.GetListAsync(a => a.SettingValues.Any(a => a.SettingScope == settingScope && a.SettingScopeKey == settingScopeKey));
            var settingGroupDtos = Mapper.Map<List<SettingGroupDto>>(settingGroups);
            return new R<List<SettingGroupDto>>(settingGroupDtos);
        }

        public async Task<R> UpdateSettings(SettingGroupDto settingGroupDto, SettingScope settingScope = SettingScope.Golbal, string? settingScopeKey = null)
        {
            var settings = Mapper.Map<List<SettingValue>>(settingGroupDto.SettingValues);
            settings.ForEach(a =>
            {
                a.SettingScope = settingScope;
                a.SettingScopeKey = settingScopeKey;
            });
            await _settingManager.SetSettingValues(settingGroupDto.Name, settings);
            return new R();
        }
    }
}
