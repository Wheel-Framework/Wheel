namespace Wheel
{
    public class EventBusChannelData<T>
    {
        public Type DataType { get; set; }
        public T Data { get; set; }
    }
}
