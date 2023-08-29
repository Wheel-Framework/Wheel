namespace Wheel.Services.LocalizationManage.Dtos
{
    public class CreateLocalizationResourceDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public virtual int CultureId { get; set; }
    }
}
