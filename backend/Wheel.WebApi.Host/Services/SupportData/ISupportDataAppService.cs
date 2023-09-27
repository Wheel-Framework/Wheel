using Wheel.Core.Dto;
using Wheel.DependencyInjection;
using Wheel.Services.SupportData.Dtos;

namespace Wheel.Services.SupportData
{
    public interface ISupportDataAppService : ITransientDependency
    {
        ValueTask<R<List<LabelValueDto>>> GetEnums(string type);
    }
}
