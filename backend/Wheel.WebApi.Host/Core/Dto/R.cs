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
    public class R<TData> : R
    {
        public R(TData data) : base()
        {
            Data = data;
        }

        public TData Data { get; set; }
    }
    public class Page<TData> : R
    {
        public Page(List<TData> data, long total) : base()
        {
            Data = data;
            Total = total;
        }

        public List<TData> Data { get; set; }
        public long Total { get; set; }
    }
}
