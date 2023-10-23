using Wheel.Core.Dto;
using Wheel.DependencyInjection;
using Wheel.Services.PermissionManage.Dtos;

namespace Wheel.Services.PermissionManage
{
    public interface IPermissionManageAppService : ITransientDependency
    {
        Task<R<List<GetAllPermissionDto>>> GetPermission();
        Task<R> UpdatePermission(UpdatePermissionDto dto);
        Task<R<List<GetAllPermissionDto>>> GetRolePermission(string RoleName);
    }
}
