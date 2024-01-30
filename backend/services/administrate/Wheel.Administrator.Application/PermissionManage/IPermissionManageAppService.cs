using Wheel.Core.Dto;
using Wheel.DependencyInjection;
using Wheel.Administrator.Services.PermissionManage.Dtos;

namespace Wheel.Administrator.Services.PermissionManage
{
    public interface IPermissionManageAppService : ITransientDependency
    {
        Task<R<List<GetAllPermissionDto>>> GetPermission();
        Task<R> UpdatePermission(UpdatePermissionDto dto);
        Task<R<List<GetAllPermissionDto>>> GetRolePermission(string RoleName);
    }
}
