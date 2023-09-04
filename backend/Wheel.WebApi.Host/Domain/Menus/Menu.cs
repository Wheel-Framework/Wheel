using Wheel.Domain.Common;
using Wheel.Enums;

namespace Wheel.Domain.Menus
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class Menu : Entity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuType MenuType { get; set; }
        /// <summary>
        /// 菜单路径
        /// </summary>
        public string? Path { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string? Permission { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 上级菜单Id
        /// </summary>
        public virtual Guid? ParentId { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        public virtual List<Menu> Childs { get; set; }
    }
}
