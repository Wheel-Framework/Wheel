﻿using Wheel.Enums;

namespace Wheel.Services.Roles.Dtos
{
    public class RoleDto
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public RoleType RoleType { get; set; }
    }
}
