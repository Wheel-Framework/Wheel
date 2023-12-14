using Microsoft.AspNetCore.Identity;
using Wheel.Const;
using Wheel.Core.Dto;
using Wheel.Core.Exceptions;
using Wheel.Domain;
using Wheel.Domain.Identity;
using Wheel.Services.Roles.Dtos;

namespace Wheel.Services.Roles
{
    public class RoleManageAppService(RoleManager<Role> roleManager, IBasicRepository<Role, string> roleRepository)
        : WheelServiceBase, IRoleManageAppService
    {
        public async Task<Page<RoleDto>> GetRolePageList(PageRequest pageRequest)
        {
            var (items, total) = await roleRepository.SelectPageListAsync(
                a => true,
                a => new RoleDto { Id = a.Id, Name = a.Name },
                (pageRequest.PageIndex - 1) * pageRequest.PageSize,
                pageRequest.PageSize,
                pageRequest.OrderBy
                );
            return Page(items, total);
        }

        public async Task<R> CreateRole(CreateRoleDto dto)
        {
            var exist = await roleManager.RoleExistsAsync(dto.Name);
            if (exist)
            {
                throw new BusinessException(ErrorCode.RoleExist, "RoleExist");
            }
            var result = await roleManager.CreateAsync(new Role(dto.Name, dto.RoleType));
            if (result.Succeeded)
            {
                return Success();
            }
            else
            {
                throw new BusinessException(ErrorCode.CreateRoleError, string.Join("\r\n", result.Errors.Select(a => a.Description)));
            }
        }

        public async Task<R> DeleteRole(string roleName)
        {
            var exist = await roleManager.RoleExistsAsync(roleName);
            if (exist)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                await roleManager.DeleteAsync(role);
            }
            else
            {
                throw new BusinessException(ErrorCode.RoleNotExist, "RoleNotExist");
            }
            return Success();
        }
    }
}
