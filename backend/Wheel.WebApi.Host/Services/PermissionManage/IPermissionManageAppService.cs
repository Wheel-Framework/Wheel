using Wheel.DependencyInjection;
using Wheel.Services.PermissionManage.Dtos;

namespace Wheel.Services.PermissionManage
{
    public interface IPermissionManageAppService : ITransientDependency
    {
        Task<List<GetAllPermissionDto>> GetPermission();
        Task UpdatePermission(UpdatePermissionDto dto);
    }
}
