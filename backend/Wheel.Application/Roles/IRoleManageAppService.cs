using Wheel.Core.Dto;
using Wheel.DependencyInjection;
using Wheel.Services.Roles.Dtos;

namespace Wheel.Services.Roles
{
    public interface IRoleManageAppService : ITransientDependency
    {
        Task<Page<RoleDto>> GetRolePageList(PageRequest pageRequest);
        Task<R> CreateRole(CreateRoleDto dto);
        Task<R> DeleteRole(string roleName);
    }
}
