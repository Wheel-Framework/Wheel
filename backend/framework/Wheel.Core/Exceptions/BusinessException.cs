namespace Wheel.Core.Exceptions
{
    public class BusinessException(string code, string? message = "") : Exception(message)
    {
        public string Code { get; set; } = code;

        public string[]? MessageData { get; set; }

        public BusinessException WithMessageDataData(params string[] messageData)
        {
            MessageData = messageData;
            return this;
        }
    }
}
