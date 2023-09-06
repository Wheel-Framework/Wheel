using Wheel.Core.Dto;
using Wheel.DependencyInjection;
using Wheel.Services.Menus.Dtos;

namespace Wheel.Services.Menus
{
    public interface IMenuAppService : ITransientDependency
    {
        Task Create(CreateOrUpdateMenuDto dto);
        Task Update(Guid id, CreateOrUpdateMenuDto dto);
        Task Delete(Guid id);
        Task<R<MenuDto>> GetById(Guid id);
        Task<Page<MenuDto>> GetPageList(PageRequest pageRequest);
    }
}
