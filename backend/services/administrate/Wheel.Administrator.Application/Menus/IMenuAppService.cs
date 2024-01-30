using Wheel.Core.Dto;
using Wheel.DependencyInjection;
using Wheel.Administrator.Services.Menus.Dtos;

namespace Wheel.Administrator.Services.Menus
{
    public interface IMenuAppService : ITransientDependency
    {
        Task<R> Create(CreateOrUpdateMenuDto dto);
        Task<R> Update(Guid id, CreateOrUpdateMenuDto dto);
        Task<R> Delete(Guid id);
        Task<R<MenuDto>> GetById(Guid id);
        Task<R<List<MenuDto>>> GetList();
        Task<R<List<MenuDto>>> GetRoleMenuList(string roleId);
        Task<R<List<AntdMenuDto>>> GetCurrentMenu();
        Task<R> UpdateRoleMenu(string roleId, UpdateRoleMenuDto dto);
    }
}
