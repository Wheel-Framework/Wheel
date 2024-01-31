using Wheel.Administrator.Token.Dtos;
using Wheel.Authorization.Jwt;
using Wheel.Core.Dto;
using Wheel.DependencyInjection;

namespace Wheel.Administrator.Token
{
    public interface ITokenAppService : ITransientDependency
    {
        Task<R<TokenResult>> Login(LoginDto loginDto);
    }
}
