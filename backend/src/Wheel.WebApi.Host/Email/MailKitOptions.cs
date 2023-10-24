namespace Wheel.Email
{
    public class MailKitOptions
    {
        public string SenderName { get; set; }
        public string Host { get; set; }
        public int Prot { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
    }
}
