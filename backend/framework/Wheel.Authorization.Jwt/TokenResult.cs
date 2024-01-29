namespace Wheel.Authorization.Jwt
{
    public class TokenResult
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
        public int AccessTokenExpiresIn { get; set; }
        public int RefreshTokenExpiresIn { get; set; }
    }
}
