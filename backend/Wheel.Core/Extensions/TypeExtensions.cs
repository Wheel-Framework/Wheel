using JetBrains.Annotations;

namespace System
{
    public static class TypeExtensions
    {

        public static string GetFullNameWithAssemblyName(this Type type)
        {
            return type.FullName + ", " + type.Assembly.GetName().Name;
        }
        public static bool IsAssignableTo<TTarget>([NotNull] this Type type)
        {
            return type.IsAssignableTo(typeof(TTarget));
        }

        public static bool IsAssignableTo([NotNull] this Type type, [NotNull] Type targetType)
        {
            return targetType.IsAssignableFrom(type);
        }

        public static Type[] GetBaseClasses([NotNull] this Type type, bool includeObject = true)
        {
            var types = new List<Type>();
            AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject);
            return types.ToArray();
        }

        public static Type[] GetBaseClasses([NotNull] this Type type, Type stoppingType, bool includeObject = true)
        {
            var types = new List<Type>();
            AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject, stoppingType);
            return types.ToArray();
        }

        private static void AddTypeAndBaseTypesRecursively(
            [NotNull] List<Type> types,
            [CanBeNull] Type type,
        bool includeObject,
            [CanBeNull] Type stoppingType = null)
        {
            if (type == null || type == stoppingType)
            {
                return;
            }

            if (!includeObject && type == typeof(object))
            {
                return;
            }

            AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject, stoppingType);
            types.Add(type);
        }
    }
}
