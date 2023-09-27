using Wheel.Core.Dto;
using Wheel.Enums;
using Wheel.Services.SupportData.Dtos;

namespace Wheel.Services.SupportData
{
    public class SupportDataAppService : WheelServiceBase, ISupportDataAppService
    {
        public ValueTask<R<List<LabelValueDto>>> GetEnums(string type)
        {
            var resultList = new List<LabelValueDto>();
            var enumType = EnumsDic[type];
            foreach (var e in Enum.GetValues(enumType))
            {
                resultList.Add(new LabelValueDto { Label = e.ToString()!, Value = e.GetHashCode() });
            }
            return ValueTask.FromResult(new R<List<LabelValueDto>>(resultList));
        }

        static Dictionary<string, Type> EnumsDic = new Dictionary<string, Type>
        {
            { "RoleType", typeof(RoleType) },
            { "MenuType", typeof(MenuType) },
        };
    }
}
