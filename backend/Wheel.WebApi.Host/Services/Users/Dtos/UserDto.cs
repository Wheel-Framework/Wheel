namespace Wheel.Services.Users.Dtos
{
    public class UserDto
    {
        public virtual DateTimeOffset CreationTime { get; set; }
        public virtual string? UserName { get; set; }

        public virtual string? Email { get; set; }

        public virtual bool? EmailConfirmed { get; set; }

        public virtual string? PhoneNumber { get; set; }
        public virtual bool? PhoneNumberConfirmed { get; set; }
    }
}
