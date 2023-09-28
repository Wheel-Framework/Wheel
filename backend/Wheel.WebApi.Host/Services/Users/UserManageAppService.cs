using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading;
using Wheel.Const;
using Wheel.Core.Dto;
using Wheel.Core.Exceptions;
using Wheel.Domain;
using Wheel.Domain.Identity;
using Wheel.Services.Users.Dtos;

namespace Wheel.Services.Users
{
    public class UserManageAppService : WheelServiceBase, IUserManageAppService
    {
        private readonly IBasicRepository<User, string> _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;

        public UserManageAppService(IBasicRepository<User, string> userRepository, UserManager<User> userManager, IUserStore<User> userStore)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _userStore = userStore;
        }

        public async Task<Page<UserDto>> GetUserPageList(UserPageRequest pageRequest)
        {
            var (items, total) = await _userRepository.SelectPageListAsync(
                _userRepository.BuildPredicate(
                    (!string.IsNullOrWhiteSpace(pageRequest.UserName), u => u.UserName.Contains(pageRequest.UserName)),
                    (!string.IsNullOrWhiteSpace(pageRequest.Email), u => u.Email.Contains(pageRequest.Email)),
                    (!string.IsNullOrWhiteSpace(pageRequest.PhoneNumber), u => u.PhoneNumber.Contains(pageRequest.PhoneNumber)),
                    (pageRequest.EmailConfirmed.HasValue, u => u.EmailConfirmed.Equals(pageRequest.EmailConfirmed)),
                    (pageRequest.PhoneNumberConfirmed.HasValue, u => u.PhoneNumberConfirmed.Equals(pageRequest.PhoneNumberConfirmed))
                    ),
                o => new UserDto
                {
                    UserName = o.UserName,
                    Email = o.Email,
                    PhoneNumber = o.PhoneNumber,
                    EmailConfirmed = o.EmailConfirmed,
                    PhoneNumberConfirmed = o.PhoneNumberConfirmed,
                    CreationTime = o.CreationTime
                },
                (pageRequest.PageIndex - 1) * pageRequest.PageSize,
                pageRequest.PageSize,
                pageRequest.OrderBy
                );

            return new Page<UserDto>(items, total);
        }
        public async Task<R> CreateUser(CreateUserDto userDto)
        {
            var user = new User();
            await _userManager.SetUserNameAsync(user, userDto.UserName);

            if(userDto.Email != null)
            {
                var emailStore = (IUserEmailStore<User>)_userStore;
                await emailStore.SetEmailAsync(user, userDto.Email, default);
            }

            var result = await _userManager.CreateAsync(user, userDto.Passowrd);
            if (result.Succeeded)
            {
                if (userDto.Roles.Count > 0)
                {
                    await _userManager.AddToRolesAsync(user, userDto.Roles);
                    await _userManager.UpdateAsync(user);
                }
                return new R();
            }
            else
                throw new BusinessException(ErrorCode.CreateUserError, string.Join("\r\n", result.Errors.Select(a => a.Description)));
        }
    }
}
