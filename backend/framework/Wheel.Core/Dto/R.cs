namespace Wheel.Core.Dto
{
    public class R
    {
        public R()
        {
        }

        public R(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; set; } = "0";
        public string Message { get; set; } = "success";

    }
    public class R<TData>(TData data) : R
    {
        public TData Data { get; set; } = data;
    }
    public class Page<TData>(List<TData> data, long total) : R
    {
        public List<TData> Data { get; set; } = data;
        public long Total { get; set; } = total;
    }
}
