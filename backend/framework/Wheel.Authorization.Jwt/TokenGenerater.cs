using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Wheel.DependencyInjection;

namespace Wheel.Authorization.Jwt
{
    public class TokenGenerater : ISingletonDependency
    {
        private readonly IDistributedCache _distributedCache;

        public TokenGenerater(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<TokenResult> GenerateJwtToken(GenerateJwtTokenModel generateJwtTokenModel)
        {
            //token到期时间
            DateTime expires = DateTime.UtcNow.AddSeconds(generateJwtTokenModel.ExpireSeconds);
            //取出配置文件的key
            byte[] keyBytes = Encoding.UTF8.GetBytes(generateJwtTokenModel.SecurityKey);
            //对称安全密钥
            var secKey = new SymmetricSecurityKey(keyBytes);
            //加密证书
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
            //jwt安全token
            var tokenDescriptor = new JwtSecurityToken(issuer: generateJwtTokenModel.Issuer, audience: generateJwtTokenModel.Audience, expires: expires, signingCredentials: credentials, claims: generateJwtTokenModel.Claims);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            var refreshToken = GenerateRefreshToken();
            await _distributedCache.SetAbsoluteExpirationRelativeToNowAsync($"RefreshToken:{refreshToken}", new CacheJwtTokenModel
            {
                Claims = generateJwtTokenModel.Claims.ToDictionary(a => a.Type, a => a.Value),
                Audience = generateJwtTokenModel.Audience,
                ExpireSeconds = generateJwtTokenModel.ExpireSeconds,
                Issuer = generateJwtTokenModel.Issuer,
                RefreshTokenExpireSeconds = generateJwtTokenModel.RefreshTokenExpireSeconds,
                SecurityKey = generateJwtTokenModel.SecurityKey,
            }, TimeSpan.FromSeconds(generateJwtTokenModel.RefreshTokenExpireSeconds));
            return new TokenResult
            {
                AccessToken = token,
                TokenType = "Bearer",
                AccessTokenExpiresIn = generateJwtTokenModel.ExpireSeconds,
                RefreshToken = refreshToken,
                RefreshTokenExpiresIn = generateJwtTokenModel.RefreshTokenExpireSeconds
            };
        }
        public async Task<TokenResult?> RefreshToken(string refreshToken)
        {
            var cache = await _distributedCache.GetAsync<CacheJwtTokenModel>($"RefreshToken:{refreshToken}");
            if (cache != null)
            {
                await _distributedCache.RemoveAsync($"RefreshToken:{refreshToken}");
                return await GenerateJwtToken(new GenerateJwtTokenModel
                {
                    Audience = cache.Audience,
                    ExpireSeconds = cache.ExpireSeconds,
                    Issuer = cache.Issuer,
                    RefreshTokenExpireSeconds = cache.RefreshTokenExpireSeconds,
                    SecurityKey = cache.SecurityKey,
                    Claims = cache.Claims.Select(a => new Claim(a.Key, a.Value))
                });
            }
            else
            {
                return null;
            }

        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal ValidateToken(string token, string securityKey, string issuer, string audience)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(securityKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
