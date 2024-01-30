namespace Wheel.Administrator.Services.SettingManage.Dtos
{
    public class SettingGroupDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? NormalizedName { get; set; }

        public virtual ICollection<SettingValueDto> SettingValues { get; set; }
    }
}
