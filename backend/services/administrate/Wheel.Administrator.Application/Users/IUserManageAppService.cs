using Wheel.Administrator.Users.Dtos;
using Wheel.Core.Dto;
using Wheel.DependencyInjection;

namespace Wheel.Administrator.Users
{
    public interface IUserManageAppService : ITransientDependency
    {
        Task<Page<UserDto>> GetUserPageList(UserPageRequest pageRequest);
        Task<R> CreateUser(CreateUserDto userDto);
        Task<R> UpdateUser(string userId, UpdateUserDto updateUserDto);
    }
}
