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
        Task<R<List<MenuDto>>> GetList();
        Task<R<List<MenuDto>>> GetRoleMenuList(string roleId);
        Task<R<List<AntdMenuDto>>> GetCurrentMenu();
    }
}
