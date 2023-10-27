using Microsoft.AspNetCore.Identity;
using Wheel.Domain.Common;
using Wheel.Enums;

namespace Wheel.Domain.Identity
{
    public class Role : IdentityRole, IEntity<string>
    {
        /// <summary>
        /// 角色类型，0管理台角色，1客户端角色
        /// </summary>
        public RoleType RoleType { get; set; }

        public Role(string roleName, RoleType roleType) : base(roleName)
        {
            RoleType = roleType;
        }

        public Role(string roleName) : base(roleName)
        {
        }

        public Role() : base()
        {
        }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
    }
}
