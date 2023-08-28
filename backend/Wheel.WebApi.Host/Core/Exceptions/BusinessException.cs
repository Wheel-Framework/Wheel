namespace Wheel.Core.Exceptions
{
    public class BusinessException : Exception
    {
        public string Code { get; set; }

        public List<string>? Data { get; set; }

        public BusinessException(string code, string? message = "") : base(message)
        {
            Code = code;
        }

        public void WithData(List<string> data)
        {
            Data = data;
        }
    }
}
