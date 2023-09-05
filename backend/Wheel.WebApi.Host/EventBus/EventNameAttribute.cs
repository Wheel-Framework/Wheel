namespace Wheel.EventBus
{
    public class EventNameAttribute : Attribute
    {
        public required string Name { get; set; }
    }
}
