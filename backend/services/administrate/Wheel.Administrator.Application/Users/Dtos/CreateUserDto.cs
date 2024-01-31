namespace Wheel.Administrator.Users.Dtos
{
    public class CreateUserDto
    {
        public virtual string UserName { get; set; }

        public virtual string Passowrd { get; set; }

        public virtual string? Email { get; set; }

        public virtual string? PhoneNumber { get; set; }

        public virtual List<string> Roles { get; set; } = new();
    }
}
