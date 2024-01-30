using Wheel.Administrator.Domain.Menus;
using Wheel.Administrator.Enums;
using Wheel.Domain;

namespace Wheel.DataSeeders.Identity
{
    public class MenuDataSeeder(IBasicRepository<Menu, Guid> menuRepository) : IDataSeeder
    {
        public async Task Seed(CancellationToken cancellationToken = default)
        {
            if (!(await menuRepository.AnyAsync(cancellationToken)))
            {
                await menuRepository.InsertAsync(new Menu
                {
                    Name = "SystemManage",
                    DisplayName = "系统管理",
                    Sort = 99,
                    Id = Guid.NewGuid(),
                    Icon = "SettingOutlined",
                    Path = "/System",
                    MenuType = MenuType.Menu,
                    Children = new List<Menu>
                    {
                        new Menu
                        {
                            Name = "UserManage",
                            DisplayName = "用户管理",
                            Sort = 0,
                            Id = Guid.NewGuid(),
                            Path = "/System/User",
                            MenuType = MenuType.Page
                        },
                        new Menu
                        {
                            Name = "RoleManage",
                            DisplayName = "角色管理",
                            Sort = 1,
                            Id = Guid.NewGuid(),
                            Path = "/System/Role",
                            MenuType = MenuType.Page,
                        },
                        new Menu
                        {
                            Name = "PermissionManage",
                            DisplayName = "权限管理",
                            Sort = 2,
                            Id = Guid.NewGuid(),
                            Path = "/System/Permission",
                            MenuType = MenuType.Page
                        },
                        new Menu
                        {
                            Name = "MenuManage",
                            DisplayName = "菜单管理",
                            Sort = 3,
                            Id = Guid.NewGuid(),
                            Path = "/System/Menu",
                            MenuType = MenuType.Page
                        },
                    }
                }, true, cancellationToken: cancellationToken);
            }
        }
    }
}
