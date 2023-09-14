using Microsoft.AspNetCore.Identity;
using Wheel.Const;
using Wheel.Core.Exceptions;
using Wheel.Core.Dto;
using Wheel.Domain.Identity;
using Wheel.Services.Roles.Dtos;
using Wheel.Domain;

namespace Wheel.Services.Roles
{
    public class RoleManageAppService : WheelServiceBase, IRoleManageAppService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IBasicRepository<Role, string> _roleRepository;

        public RoleManageAppService(RoleManager<Role> roleManager, IBasicRepository<Role, string> roleRepository)
        {
            _roleManager = roleManager;
            _roleRepository = roleRepository;
        }
        public async Task<Page<RoleDto>> GetRolePageList(PageRequest pageRequest)
        {
            var (items, total) = await _roleRepository.SelectPageListAsync(
                a => true,
                a => new RoleDto { Id = a.Id, Name = a.Name },
                (pageRequest.PageIndex - 1) * pageRequest.PageSize,
                pageRequest.PageSize,
                pageRequest.OrderBy
                );
            return new Page<RoleDto>(items, total);
        }

        public async Task CreateRole(CreateRoleDto dto)
        {
            var exist = await _roleManager.RoleExistsAsync(dto.Name);
            if (exist)
            {
                throw new BusinessException(ErrorCode.RoleExist, "RoleExist");
            }
            var result = await _roleManager.CreateAsync(new Role(dto.Name, dto.RoleType));
            if(result.Succeeded)
            {
                return;
            }
            else
            {
                throw new BusinessException(ErrorCode.CreateRoleError, string.Join("\r\n", result.Errors.Select(a => a.Description)));
            }
        }

        public async Task DeleteRole(string roleName)
        {
            var exist = await _roleManager.RoleExistsAsync(roleName);
            if (exist)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                await _roleManager.DeleteAsync(role);
            }
            else
            {
                throw new BusinessException(ErrorCode.RoleNotExist, "RoleNotExist");
            }
        }
    }
}
