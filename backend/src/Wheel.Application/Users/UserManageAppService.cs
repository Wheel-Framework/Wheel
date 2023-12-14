using Microsoft.AspNetCore.Identity;
using Wheel.Const;
using Wheel.Core.Dto;
using Wheel.Core.Exceptions;
using Wheel.Domain;
using Wheel.Domain.Identity;
using Wheel.Services.Users.Dtos;

namespace Wheel.Services.Users
{
    public class UserManageAppService(IBasicRepository<User, string> userRepository, UserManager<User> userManager,
            IUserStore<User> userStore)
        : WheelServiceBase, IUserManageAppService
    {
        public async Task<Page<UserDto>> GetUserPageList(UserPageRequest pageRequest)
        {
            var (items, total) = await userRepository.SelectPageListAsync(
                userRepository.BuildPredicate(
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

            return Page(items, total);
        }
        public async Task<R> CreateUser(CreateUserDto userDto)
        {
            var user = new User();
            await userManager.SetUserNameAsync(user, userDto.UserName);

            if (userDto.Email != null)
            {
                var emailStore = (IUserEmailStore<User>)userStore;
                await emailStore.SetEmailAsync(user, userDto.Email, default);
            }

            var result = await userManager.CreateAsync(user, userDto.Passowrd);
            if (result.Succeeded)
            {
                if (userDto.Roles.Count > 0)
                {
                    await userManager.AddToRolesAsync(user, userDto.Roles);
                    await userManager.UpdateAsync(user);
                }
                return Success();
            }
            else
                throw new BusinessException(ErrorCode.CreateUserError, string.Join("\r\n", result.Errors.Select(a => a.Description)));
        }
        public async Task<R> UpdateUser(string userId, UpdateUserDto updateUserDto)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new BusinessException(ErrorCode.UserNotExist, L["UserNotExist"]);
            }
            if (updateUserDto.Email != null)
            {
                var emailStore = (IUserEmailStore<User>)userStore;
                await emailStore.SetEmailAsync(user, updateUserDto.Email, default);
            }
            if (updateUserDto.PhoneNumber != null)
            {
                await userManager.SetPhoneNumberAsync(user, updateUserDto.PhoneNumber);
            }
            if (updateUserDto.Roles.Count > 0)
            {
                var existRoles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, existRoles);
                await userManager.AddToRolesAsync(user, updateUserDto.Roles);
            }
            await userManager.UpdateAsync(user);
            return Success();
        }
    }
}
