using DynamicExpresso;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace Wheel.SmartQuery;

public static class WhereExtensions
{
    public static IQueryable<T> WhereObj<T>(this IQueryable<T> queryable, object parameterObject)
    {
        var interpreter = new Interpreter();
        interpreter = interpreter.SetVariable("o", parameterObject);
        var properties = parameterObject.GetType().GetProperties().Where(p => p.CustomAttributes.Any(a=>a.AttributeType == typeof(CompareAttribute)));
        var whereExpression = new StringBuilder();
        foreach (var property in properties)
        {
            if(property.GetValue(parameterObject) == null)
            {
                continue;
            }

            var compareAttribute = property.GetCustomAttribute<CompareAttribute>();

            var propertyName = compareAttribute!.CompareProperty ?? property.Name;

            if (typeof(T).GetProperty(propertyName) == null)
            {
                continue;
            }

            if (whereExpression.Length > 0)
            {
                whereExpression.Append(" && ");
            }

            whereExpression.Append(BuildCompareExpression(propertyName, property, compareAttribute.CompareType, compareAttribute.CompareSite));
        }

        if(whereExpression.Length > 0)
        {
            return queryable.Where(interpreter.ParseAsExpression<Func<T, bool>>(whereExpression.ToString(), "q"));
        }
        return queryable;
    }
    public static IEnumerable<T> WhereObj<T>(this IEnumerable<T> enumerable, object parameterObject)
    {
        var interpreter = new Interpreter();
        interpreter = interpreter.SetVariable("o", parameterObject);
        var properties = parameterObject.GetType().GetProperties().Where(p => p.CustomAttributes.Any(a=>a.AttributeType == typeof(CompareAttribute)));
        var whereExpression = new StringBuilder();
        foreach (var property in properties)
        {
            if(property.GetValue(parameterObject) == null)
            {
                continue;
            }

            var compareAttribute = property.GetCustomAttribute<CompareAttribute>();

            var propertyName = compareAttribute!.CompareProperty ?? property.Name;

            if (typeof(T).GetProperty(propertyName) == null)
            {
                continue;
            }

            if (whereExpression.Length > 0)
            {
                whereExpression.Append(" && ");
            }

            whereExpression.Append(BuildCompareExpression(propertyName, property, compareAttribute.CompareType, compareAttribute.CompareSite));
        }

        if(whereExpression.Length > 0)
        {
            return enumerable.Where(interpreter.ParseAsExpression<Func<T, bool>>(whereExpression.ToString(), "q").Compile());
        }
        return enumerable;
    }

    private static string BuildCompareExpression(string propertyName, PropertyInfo propertyInfo, CompareType compareType, CompareSite compareSite)
    {
        var source = $"q.{propertyName}";
        var target = $"o.{propertyInfo.Name}";
        return compareType switch
        {
            CompareType.Equal => compareSite == CompareSite.LEFT ? $"{source} == {target}" : $"{target} == {source}",
            CompareType.NotEqual => compareSite == CompareSite.LEFT ? $"{source} != {target}" : $"{target} != {source}",
            CompareType.GreaterThan => compareSite == CompareSite.LEFT ? $"{source} < {target}" : $"{target} > {source}",
            CompareType.GreaterThanOrEqual => compareSite == CompareSite.LEFT ? $"{source} <= {target}" : $"{target} >= {source}",
            CompareType.LessThan => compareSite == CompareSite.LEFT ? $"{source} > {target}" : $"{target} < {source}",
            CompareType.LessThanOrEqual => compareSite == CompareSite.LEFT ? $"{source} >= {target}" : $"{target} <= {source}",
            CompareType.Contains => compareSite == CompareSite.LEFT ? $"{source}.Contains({target})" : $"{target}.Contains({source})",
            CompareType.StartsWith => compareSite == CompareSite.LEFT ? $"{source}.StartsWith({target})" : $"{target}.StartsWith({source})",
            CompareType.EndsWith => compareSite == CompareSite.LEFT ? $"{source}.EndsWith({target})" : $"{target}.EndsWith({source})",
            CompareType.IsNull => $"{source} == null",
            CompareType.IsNotNull => $"{source} != null",
            _ => throw new NotSupportedException()
        };
    }
}
