using Microsoft.AspNetCore.Mvc;
using Wheel.Authorization.Jwt;
using Wheel.Administrator.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Wheel.Administrator.Token;
using Wheel.Core.Dto;
using Wheel.Administrator.Token.Dtos;

namespace Wheel.Administrator.Controllers
{
    /// <summary>
    /// Token
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController(ITokenAppService tokenAppService) : AdministratorControllerBase
    {
        [HttpPost]
        public Task<R<TokenResult>> Login(LoginDto loginDto)
        {
            return tokenAppService.Login(loginDto);
        }
    }
}
