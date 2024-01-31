using Wheel.Administrator.Roles.Dtos;
using Wheel.Core.Dto;
using Wheel.DependencyInjection;

namespace Wheel.Administrator.Roles
{
    public interface IRoleManageAppService : ITransientDependency
    {
        Task<Page<RoleDto>> GetRolePageList(PageRequest pageRequest);
        Task<R> CreateRole(CreateRoleDto dto);
        Task<R> DeleteRole(string roleName);
    }
}
