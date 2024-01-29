using System.Security.Claims;

namespace Wheel.Authorization.Jwt
{
    public class GenerateJwtTokenModel
    {
        public IEnumerable<Claim> Claims { get; set; }
        public string SecurityKey { get; set; }
        public int ExpireSeconds { get; set; }
        public int RefreshTokenExpireSeconds { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
