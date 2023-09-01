namespace Wheel.Services.PermissionManage.Dtos
{
    public class GetAllPermissionDto
    {
        public string Group { get; set; } 
        public string Summary { get; set; } 

        public List<PermissionDto> Permissions { get; set; } = new ();
        
    }
    public class PermissionDto
    {
        public string Name { get; set; }
        public string Summary { get; set; }

        public bool IsGranted { get; set; }
    }
}
