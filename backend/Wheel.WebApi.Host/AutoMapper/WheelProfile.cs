using AutoMapper;
using Wheel.Domain.Localization;
using Wheel.Services.LocalizationManage.Dtos;

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
        }
    }
}
