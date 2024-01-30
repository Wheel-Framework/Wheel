using Wheel.Administrator.Domain.Menus;
using Wheel.Administrator.Services;
using Wheel.Administrator.Services.Menus;
using Wheel.Administrator.Services.Menus.Dtos;
using Wheel.Core.Dto;
using Wheel.Domain;
using Role = Wheel.Administrator.Domain.Identity.BackendRole;

namespace Wheel.Services.Menus
{
    public class MenuAppService(IBasicRepository<Menu, Guid> menuRepository, IBasicRepository<Role, string> roleRepository, IBasicRepository<RoleMenu> roleMenuRepository) : AdministratorServiceBase, IMenuAppService
    {

        public async Task<R> Create(CreateOrUpdateMenuDto dto)
        {
            var menu = Mapper.Map<Menu>(dto);
            menu.Id = GuidGenerator.Create();
            await menuRepository.InsertAsync(menu, true);
            return Success();
        }

        public async Task<R> Update(Guid id, CreateOrUpdateMenuDto dto)
        {
            var menu = await menuRepository.FindAsync(id);
            if (menu != null)
            {
                Mapper.Map(dto, menu);
                await menuRepository.UpdateAsync(menu, true);
            }
            return Success();
        }
        public async Task<R> Delete(Guid id)
        {
            await menuRepository.DeleteAsync(id, true);
            return Success();
        }
        public async Task<R<MenuDto>> GetById(Guid id)
        {
            var menu = await menuRepository.FindAsync(id);

            var dto = Mapper.Map<MenuDto>(menu);
            return Success(dto);
        }
        public async Task<R<List<MenuDto>>> GetList()
        {
            var items = await menuRepository.GetListAsync(
                a => a.ParentId == null,
                propertySelectors: a => a.Children
                );
            items.ForEach(a => a.Children = a.Children.OrderBy(b => b.Sort).ToList());
            items = items.OrderBy(a => a.Sort).ToList();
            var resultItems = Mapper.Map<List<MenuDto>>(items);
            return Success(resultItems);
        }
        public async Task<R> UpdateRoleMenu(string roleId, UpdateRoleMenuDto dto)
        {
            using (var uow = await UnitOfWork.BeginTransactionAsync())
            {
                if (await roleMenuRepository.AnyAsync(a => a.RoleId == roleId))
                {
                    await roleMenuRepository.DeleteAsync(a => a.RoleId == roleId);
                }
                if (dto.MenuIds.Any())
                {
                    var roleMenus = dto.MenuIds.Select(a => new RoleMenu { RoleId = roleId, MenuId = a });
                    await roleMenuRepository.InsertManyAsync(roleMenus.ToList());
                }
                await uow.CommitAsync();
            }
            return Success();
        }
        public async Task<R<List<MenuDto>>> GetRoleMenuList(string roleId)
        {
            var items = await roleMenuRepository.SelectListAsync(a => a.RoleId == roleId && a.Menu.ParentId == null, a => a.Menu, propertySelectors: a => a.Menu.Children);
            items.ForEach(a => a.Children = a.Children.OrderBy(b => b.Sort).ToList());
            items = items.OrderBy(a => a.Sort).ToList();
            var resultItems = Mapper.Map<List<MenuDto>>(items);
            return Success(resultItems);
        }

        public async Task<R<List<AntdMenuDto>>> GetCurrentMenu()
        {
            if (CurrentUser.IsInRoles("admin"))
            {
                var menus = await menuRepository.GetListAsync(a => a.ParentId == null);
                return Success(MaptoAntdMenu(menus));
            }
            else
            {
                var roleIds = await roleRepository.SelectListAsync(a => CurrentUser.Roles.Contains(a.Name), a => a.Id);
                var menus = await roleMenuRepository.SelectListAsync(a => roleIds.Contains(a.RoleId) && a.Menu.ParentId == null, a => a.Menu, propertySelectors: a => a.Menu.Children);

                return Success(MaptoAntdMenu(menus.DistinctBy(a => a.Id).ToList()));
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
                if (m.Children != null && m.Children.Count > 0)
                {
                    result.Children = MaptoAntdMenu(m.Children);
                }
                return result;
            }).ToList();
        }
    }
}
