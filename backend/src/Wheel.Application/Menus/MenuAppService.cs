using StackExchange.Redis;
using System.Diagnostics;
using Wheel.Core.Dto;
using Wheel.Domain;
using Wheel.Domain.Identity;
using Wheel.Domain.Menus;
using Wheel.Services.Menus.Dtos;
using Role = Wheel.Domain.Identity.Role;

namespace Wheel.Services.Menus
{
    public class MenuAppService : WheelServiceBase, IMenuAppService
    {
        private readonly IBasicRepository<Menu, Guid> _menuRepository;
        private readonly IBasicRepository<Role, string> _roleRepository;
        private readonly IBasicRepository<RoleMenu> _roleMenuRepository;

        public MenuAppService(IBasicRepository<Menu, Guid> menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<R> Create(CreateOrUpdateMenuDto dto)
        {
            var menu = Mapper.Map<Menu>(dto);
            menu.Id = GuidGenerator.Create();
            await _menuRepository.InsertAsync(menu, true);
            return new R();
        }

        public async Task<R> Update(Guid id,CreateOrUpdateMenuDto dto)
        {
            var menu = await _menuRepository.FindAsync(id);
            if(menu != null) 
            {
                Mapper.Map(dto, menu);
                await _menuRepository.UpdateAsync(menu, true);
            }
            return new R();
        }
        public async Task<R> Delete(Guid id)
        {
            await _menuRepository.DeleteAsync(id, true);
            return new R();
        }
        public async Task<R<MenuDto>> GetById(Guid id)
        {
            var menu = await _menuRepository.FindAsync(id);

            var dto = Mapper.Map<MenuDto>(menu);
            return new R<MenuDto>(dto);
        }
        public async Task<R<List<MenuDto>>> GetList()
        {
            var items = await _menuRepository.GetListAsync(
                a => a.ParentId == null,
                propertySelectors: a=>a.Children
                );
            items.ForEach(a => a.Children = a.Children.OrderBy(b => b.Sort).ToList());
            items = items.OrderBy(a => a.Sort).ToList();
            var resultItems = Mapper.Map<List<MenuDto>>(items);
            return new R<List<MenuDto>>(resultItems);
        }
        public async Task<R> UpdateRoleMenu(string roleId, UpdateRoleMenuDto dto)
        {
            using (var uow = await UnitOfWork.BeginTransactionAsync())
            {
                if (await _roleMenuRepository.AnyAsync(a => a.RoleId == roleId))
                {
                    await _roleMenuRepository.DeleteAsync(a => a.RoleId == roleId);
                }
                if(dto.MenuIds.Any())
                {
                    var roleMenus = dto.MenuIds.Select(a => new RoleMenu { RoleId = roleId, MenuId = a });
                    await _roleMenuRepository.InsertManyAsync(roleMenus.ToList());
                }
                await uow.CommitAsync();
            }
            return new R();
        }
        public async Task<R<List<MenuDto>>> GetRoleMenuList(string roleId)
        {
            var items = await _roleMenuRepository.SelectListAsync(a => a.RoleId == roleId && a.Menu.ParentId == null, a => a.Menu, propertySelectors: a => a.Menu.Children);
            items.ForEach(a => a.Children = a.Children.OrderBy(b => b.Sort).ToList());
            items = items.OrderBy(a => a.Sort).ToList();
            var resultItems = Mapper.Map<List<MenuDto>>(items);
            return new R<List<MenuDto>>(resultItems);
        }

        public async Task<R<List<AntdMenuDto>>> GetCurrentMenu()
        {
            if (CurrentUser.IsInRoles("admin"))
            {
                var menus = await _menuRepository.GetListAsync(a => a.ParentId == null);
                return new R<List<AntdMenuDto>>(MaptoAntdMenu(menus));
            }
            else
            {
                var roleIds = await _roleRepository.SelectListAsync(a => CurrentUser.Roles.Contains(a.Name), a => a.Id);
                var menus = await _roleMenuRepository.SelectListAsync(a => roleIds.Contains(a.RoleId) && a.Menu.ParentId == null, a => a.Menu, propertySelectors: a => a.Menu.Children);

                return new R<List<AntdMenuDto>>(MaptoAntdMenu(menus.DistinctBy(a=>a.Id).ToList()));
            }
        }

        private List<AntdMenuDto> MaptoAntdMenu(List<Menu> menus)
        {
            return menus.OrderBy(m => m.Sort).Select(m =>
            {
                var result = new AntdMenuDto
                {
                    Name = m.Name,
                    Icon = m.Icon,
                    Path = m.Path,
                    Access = m.Permission
                };
                if(m.Children != null && m.Children.Count > 0)
                {
                    result.Children = MaptoAntdMenu(m.Children);
                }
                return result;
            }).ToList();
        }
    }
}
