namespace Wheel.Services.LocalizationManage.Dtos
{
    public class LocalizationResourceDto
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public virtual int CultureId { get; set; }
        public virtual LocalizationCultureDto Culture { get; set; }
    }
}
