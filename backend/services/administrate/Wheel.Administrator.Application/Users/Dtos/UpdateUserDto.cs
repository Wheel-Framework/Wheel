namespace Wheel.Administrator.Users.Dtos
{
    public class UpdateUserDto
    {
        public virtual string? Email { get; set; }

        public virtual string? PhoneNumber { get; set; }

        public virtual List<string> Roles { get; set; } = new();
    }
}
