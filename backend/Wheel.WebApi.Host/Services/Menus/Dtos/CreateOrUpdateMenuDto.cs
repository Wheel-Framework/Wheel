﻿using Wheel.Enums;

namespace Wheel.Services.Menus.Dtos
{
    public class CreateOrUpdateMenuDto
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
        public Guid? ParentId { get; set; }
    }
}
