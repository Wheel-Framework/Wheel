namespace Wheel.Administrator.Services.PermissionManage.Dtos
{
    public class UpdatePermissionDto
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public List<string> Permissions { get; set; }
    }
}
