using Wheel.Domain;
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
            if (!(await _menuRepository.AnyAsync(cancellationToken)))
            {
                await _menuRepository.InsertAsync(new Menu
                {
                    Name = "SystemManage",
                    DisplayName = "系统管理",
                    Sort = 99,
                    Id = Guid.NewGuid(),
                    Icon = "SettingOutlined",
                    Path = "/System",
                    MenuType = Enums.MenuType.Menu,
                    Children = new List<Menu>
                    {
                        new Menu
                        {
                            Name = "UserManage",
                            DisplayName = "用户管理",
                            Sort = 0,
                            Id = Guid.NewGuid(),
                            Path = "/System/User",
                            MenuType = Enums.MenuType.Page
                        },
                        new Menu
                        {
                            Name = "RoleManage",
                            DisplayName = "角色管理",
                            Sort = 1,
                            Id = Guid.NewGuid(),
                            Path = "/System/Role",
                            MenuType = Enums.MenuType.Page,
                        },
                        new Menu
                        {
                            Name = "PermissionManage",
                            DisplayName = "权限管理",
                            Sort = 2,
                            Id = Guid.NewGuid(),
                            Path = "/System/Permission",
                            MenuType = Enums.MenuType.Page
                        },
                        new Menu
                        {
                            Name = "MenuManage",
                            DisplayName = "菜单管理",
                            Sort = 3,
                            Id = Guid.NewGuid(),
                            Path = "/System/Menu",
                            MenuType = Enums.MenuType.Page
                        },
                        new Menu
                        {
                            Name = "LocalizationManage",
                            DisplayName = "多语言管理",
                            Sort = 4,
                            Id = Guid.NewGuid(),
                            Path = "/System/Localization",
                            MenuType = Enums.MenuType.Page
                        },
                    }
                }, true, cancellationToken: cancellationToken);
            }
        }
    }
}
