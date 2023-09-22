namespace Wheel.Services.Menus.Dtos
{
    public class AntdMenuDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 菜单路径
        /// </summary>
        public string? Path { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string? Icon { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        public virtual List<AntdMenuDto> Children { get; set; }
    }
}
