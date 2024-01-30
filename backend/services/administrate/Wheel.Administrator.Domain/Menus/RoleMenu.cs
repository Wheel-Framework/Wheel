using Wheel.Administrator.Domain.Identity;

namespace Wheel.Administrator.Domain.Menus
{
    public class RoleMenu
    {
        public virtual string RoleId { get; set; }
        public virtual BackendRole Role { get; set; }
        public virtual Guid MenuId { get; set; }
        public virtual Menu Menu { get; set; }
    }
}
