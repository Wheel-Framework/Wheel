using Wheel.Core.Dto;
using Wheel.Services.SupportData.Dtos;

namespace Wheel.Services.SupportData
{
    public class SupportDataAppService : WheelServiceBase, ISupportDataAppService
    {
        public ValueTask<R<List<LabelValueDto>>> GetEnums(string type, string? nameSpace = null)
        {
            var resultList = new List<LabelValueDto>();
            var enumType = Type.GetType($"{nameSpace ?? "Wheel.Enums"}.{type}");
            if (enumType is null)
            {
                return ValueTask.FromResult(new R<List<LabelValueDto>>(new()));
            }
            foreach (var e in Enum.GetValues(enumType))
            {
                resultList.Add(new LabelValueDto { Label = e.ToString()!, Value = e.GetHashCode() });
            }
            return ValueTask.FromResult(new R<List<LabelValueDto>>(resultList));
        }
    }
}
