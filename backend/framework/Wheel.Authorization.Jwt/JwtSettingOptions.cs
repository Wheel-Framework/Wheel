namespace Wheel.Authorization.Jwt
{
    public class JwtSettingOptions
    {
        public string SecurityKey { get; set; }
        public int ExpireSeconds { get; set; }
        public int RefreshTokenExpireSeconds { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
