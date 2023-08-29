namespace Wheel.Services.LocalizationManage.Dtos
{
    public class LocalizationCultureDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<LocalizationResourceDto> Resources { get; set; }
    }
}
