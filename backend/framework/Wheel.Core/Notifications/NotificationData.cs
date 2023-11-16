namespace Wheel.Notifications
{
    public class NotificationData(NotificationType type)
    {
        public NotificationType Type { get; set; } = type;

        public IDictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

        public NotificationData WithData(string name, object value)
        {
            Data.Add(name, value);
            return this;
        }
    }
    public enum NotificationType
    {
        WellCome = 0,
        Info = 1,
        Warn = 2,
        Error = 3
    }
}
