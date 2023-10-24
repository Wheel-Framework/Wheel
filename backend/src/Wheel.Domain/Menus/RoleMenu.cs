using Wheel.Domain.Identity;

namespace Wheel.Domain.Menus
{
    public class RoleMenu
    {
        public virtual string RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual Guid MenuId { get; set; }
        public virtual Menu Menu { get; set; }
    }
}
