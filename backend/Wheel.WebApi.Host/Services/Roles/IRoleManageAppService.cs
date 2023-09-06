using Wheel.Core.Dto;
using Wheel.DependencyInjection;
using Wheel.Services.Roles.Dtos;

namespace Wheel.Services.Roles
{
    public interface IRoleManageAppService : ITransientDependency
    {
        Task<Page<RoleDto>> GetRolePageList(PageRequest pageRequest);
        Task CreateRole(CreateRoleDto dto);
        Task DeleteRole(string roleName);
    }
}
