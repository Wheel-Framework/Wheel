namespace Wheel.Administrator.Token.Dtos
{
    public class LoginDto
    {
        public string LoginType { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string? TwoFactorCode { get; set; }
        public bool IsPersistent { get; set; } = false;
        public bool LockoutOnFailure { get; set; } = false;
    }
}
