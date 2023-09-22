using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Wheel.Domain;
using Wheel.Domain.Identity;
using Wheel.Domain.Menus;

namespace Wheel.DataSeeders.Identity
{
    public class MenuDataSeeder : IDataSeeder
    {
        private readonly IBasicRepository<Menu, Guid> _menuRepository;

        public MenuDataSeeder(IBasicRepository<Menu, Guid> menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task Seed(CancellationToken cancellationToken = default)
        {
            if(!(await _menuRepository.AnyAsync(cancellationToken)))
            {
                await _menuRepository.InsertAsync(new Menu
                {
                    Name = "Menu",
                    DisplayName = "菜单",
                    Sort = 99,
                    Id = Guid.NewGuid(),
                    Icon = "MenuOutlined",
                    Path = "/menu",
                    MenuType = Enums.MenuType.Menu,
                    Children = new List<Menu>
                    {
                        new Menu
                        {
                            Name = "MenuManager",
                            DisplayName = "菜单管理",
                            Sort = 0,
                            Id = Guid.NewGuid(),
                            Path = "/menu/manager",
                            MenuType = Enums.MenuType.Page
                        },
                        new Menu
                        {
                            Name = "RoleMenuManager",
                            DisplayName = "角色菜单管理",
                            Sort = 1,
                            Id = Guid.NewGuid(),
                            Path = "/menu/role-menu-manager",
                            MenuType = Enums.MenuType.Page
                        }
                    }
                }, true, cancellationToken: cancellationToken);
            }
        }
    }
}
