using System.Diagnostics.CodeAnalysis;

namespace Wheel.EventBus
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EventNameAttribute : Attribute
    {
        public string Name { get; set; }

        public EventNameAttribute([NotNull] string name)
        {
            Name = name;
        }

        public static string? GetNameOrDefault<TEvent>()
        {
            return GetNameOrDefault(typeof(TEvent));
        }

        public static string? GetNameOrDefault([NotNull] Type eventType)
        {
            return eventType
                       .GetCustomAttributes(true)
                       .OfType<EventNameAttribute>()
                       .FirstOrDefault()
                       ?.GetName(eventType)
                   ?? eventType.FullName;
        }

        public string? GetName(Type eventType)
        {
            return Name;
        }
    }
}
