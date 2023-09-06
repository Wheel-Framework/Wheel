namespace Wheel.Core.Dto
{
    public class PageRequest
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string OrderBy { get; set; } = "Id";
    }
}
