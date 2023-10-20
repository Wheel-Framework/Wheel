using AutoMapper;
using Wheel.Domain.FileStorages;
using Wheel.Domain.Localization;
using Wheel.Domain.Menus;
using Wheel.Domain.Settings;
using Wheel.Services.FileStorageManage.Dtos;
using Wheel.Services.LocalizationManage.Dtos;
using Wheel.Services.Menus.Dtos;
using Wheel.Services.SettingManage.Dtos;

namespace Wheel.AutoMapper
{
    public class WheelProfile : Profile
    {
        public WheelProfile()
        {
            CreateMap<CreateLocalizationCultureDto, LocalizationCulture>(MemberList.Source);
            CreateMap<LocalizationCulture, LocalizationCultureDto>();

            CreateMap<CreateLocalizationResourceDto, LocalizationResource>(MemberList.Source);
            CreateMap<UpdateLocalizationResourceDto, LocalizationResource>(MemberList.Source);
            CreateMap<LocalizationResource, LocalizationResourceDto>();

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
