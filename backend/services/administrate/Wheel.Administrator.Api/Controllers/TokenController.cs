using Microsoft.AspNetCore.Mvc;
using Wheel.Authorization.Jwt;
using Wheel.Administrator.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Wheel.Administrator.Token;
using Wheel.Core.Dto;
using Wheel.Administrator.Token.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Wheel.Administrator.Controllers
{
    /// <summary>
    /// Token
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController(ITokenAppService tokenAppService) : AdministratorControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public Task<R<TokenResult>> Login(LoginDto loginDto)
        {
            return tokenAppService.Login(loginDto);
        }

        [AllowAnonymous]
        [HttpPost("Refresh")]
        public Task<R<TokenResult>> Refresh(RefreshTokenDto refreshTokenDto)
        {
            return tokenAppService.Refresh(refreshTokenDto);
        }
    }
}
