using Wheel.Enums;

namespace Wheel.Services.Roles.Dtos
{
    public class CreateRoleDto
    {
        public string Name { get; set; }
        public RoleType RoleType { get; set; }
    }
}
