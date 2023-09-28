using Wheel.Core.Dto;
using Wheel.DependencyInjection;
using Wheel.Services.Users.Dtos;

namespace Wheel.Services.Users
{
    public interface IUserManageAppService : ITransientDependency
    {
        Task<Page<UserDto>> GetUserPageList(UserPageRequest pageRequest);
        Task<R> CreateUser(CreateUserDto userDto);
    }
}
