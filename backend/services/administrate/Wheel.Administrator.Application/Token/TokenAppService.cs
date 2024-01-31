using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Wheel.Administrator.Domain.Identity;
using Wheel.Administrator.Identity;
using Wheel.Administrator.Services;
using Wheel.Administrator.Token.Dtos;
using Wheel.Authorization.Jwt;
using Wheel.Core.Dto;
using Wheel.Core.Exceptions;

namespace Wheel.Administrator.Token
{
    public class TokenAppService(TokenGenerater tokenGenerater, SignInManager<BackendUser> signInManager, BackendUserManager userManager, IOptionsMonitor<JwtSettingOptions> jwtSettingOptions) : AdministratorServiceBase, ITokenAppService
    {
        public async Task<R<TokenResult>> Login(LoginDto loginDto)
        {
            BackendUser? user = loginDto.LoginType switch
            {
                "Email" => await userManager.FindByEmailAsync(loginDto.Account),
                "UserName" => await userManager.FindByNameAsync(loginDto.Account),
                "PhoneNumber" => await userManager.FindByPhoneNumberAsync(loginDto.Account),
                _ => default,
            };
            if (user == null)
            {
                throw new BusinessException(ErrorCode.UserNotExist);
            }
            var signInResult = await signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.IsPersistent, loginDto.LockoutOnFailure);
            if (signInResult.Succeeded)
            {
                var claimsPrincipal = await signInManager.CreateUserPrincipalAsync(user);
                var tokenResult = await tokenGenerater.GenerateJwtToken(new GenerateJwtTokenModel
                {
                    Claims = claimsPrincipal.Claims,
                    Audience = jwtSettingOptions.CurrentValue.Audience,
                    Issuer = jwtSettingOptions.CurrentValue.Issuer,
                    ExpireSeconds = jwtSettingOptions.CurrentValue.ExpireSeconds,
                    RefreshTokenExpireSeconds = jwtSettingOptions.CurrentValue.RefreshTokenExpireSeconds,
                    SecurityKey = jwtSettingOptions.CurrentValue.SecurityKey
                });
                return Success(tokenResult);
            }
            else
            {
                throw new BusinessException(ErrorCode.LoginError);
            }
        }
    }
}
