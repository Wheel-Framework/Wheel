using Microsoft.Extensions.DependencyInjection;
using Wheel.Administrator.Domain.Settings;
using Wheel.Administrator.Services.SettingManage.Dtos;
using Wheel.Core.Dto;
using Wheel.Domain;
using Wheel.Enums;
using Wheel.Settings;

namespace Wheel.Administrator.Services.SettingManage
{
    public class SettingManageAppService(IBasicRepository<SettingGroup, long> settingGroupRepository,
            IBasicRepository<SettingValue, long> settingValueRepository, SettingManager settingManager)
        : AdministratorServiceBase, ISettingManageAppService
    {
        private readonly IBasicRepository<SettingValue, long> _settingValueRepository = settingValueRepository;

        public async Task<R<List<SettingGroupDto>>> GetAllSettingGroup(SettingScope settingScope = SettingScope.Global)
        {
            var settingDefinitions = ServiceProvider.GetServices<ISettingDefinition>().Where(a => a.SettingScope == settingScope);
            var settingGroups = await settingGroupRepository.GetListAsync(a => a.SettingValues.Any(a => a.SettingScope == settingScope && (settingScope == SettingScope.User ? a.SettingScopeKey == CurrentUser.Id : a.SettingScopeKey == null)));
            foreach (var settingDefinition in settingDefinitions)
            {
                if (settingGroups.Any(a => a.Name == settingDefinition.GroupName))
                    continue;
                else
                {
                    var group = new SettingGroup
                    {
                        Name = settingDefinition.GroupName,
                        NormalizedName = settingDefinition.GroupName.ToUpper(),
                        SettingValues = new List<SettingValue>()
                    };
                    foreach (var settings in settingDefinition.Define())
                    {
                        group.SettingValues.Add(new SettingValue
                        {
                            Key = settings.Key,
                            Value = settings.Value.DefalutValue,
                            ValueType = settings.Value.SettingValueType,
                            SettingScopeKey = settings.Value.SettingScopeKey,
                            SettingScope = settingScope
                        });
                    }
                    settingGroups.Add(group);
                }
            }
            var settingGroupDtos = Mapper.Map<List<SettingGroupDto>>(settingGroups);
            return Success(settingGroupDtos);
        }

        public async Task<R> UpdateSettings(SettingGroupDto settingGroupDto, SettingScope settingScope = SettingScope.Global)
        {
            var settings = Mapper.Map<List<SettingValue>>(settingGroupDto.SettingValues);
            settings.ForEach(a =>
            {
                a.SettingScope = settingScope;
                a.SettingScopeKey = settingScope == SettingScope.User ? CurrentUser.Id : null;
            });
            await settingManager.SetSettingValues(settingGroupDto.Name, settings);
            return Success();
        }
    }
}
