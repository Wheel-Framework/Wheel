using System.Diagnostics.CodeAnalysis;

namespace Wheel.EventBus
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EventNameAttribute([NotNull] string name) : Attribute
    {
        public string Name { get; set; } = name;

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
