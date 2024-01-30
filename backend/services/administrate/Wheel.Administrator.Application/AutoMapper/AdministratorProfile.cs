using AutoMapper;
using Wheel.Administrator.Domain.FileStorages;
using Wheel.Administrator.Domain.Menus;
using Wheel.Administrator.Domain.Settings;
using Wheel.Administrator.Services.Menus.Dtos;
using Wheel.Administrator.Services.SettingManage.Dtos;
using Wheel.Services.FileStorageManage.Dtos;

namespace Wheel.Administrator.AutoMapper
{
    public class AdministratorProfile : Profile
    {
        public AdministratorProfile()
        {
            CreateMap<CreateOrUpdateMenuDto, Menu>(MemberList.Source);
            CreateMap<Menu, MenuDto>();

            CreateMap<SettingGroup, SettingGroupDto>();
            CreateMap<SettingValue, SettingValueDto>();
            CreateMap<SettingGroupDto, SettingGroup>(MemberList.Source);
            CreateMap<SettingValueDto, SettingValue>(MemberList.Source);

            CreateMap<FileStorage, FileStorageDto>(MemberList.Source);
        }
    }
}
