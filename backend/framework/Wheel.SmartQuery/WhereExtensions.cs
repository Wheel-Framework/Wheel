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

            whereExpression.Append(BuildCompareExpression(propertyName, property, compareAttribute.CompareType));
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

            whereExpression.Append(BuildCompareExpression(propertyName, property, compareAttribute.CompareType));
        }

        if(whereExpression.Length > 0)
        {
            return enumerable.Where(interpreter.ParseAsExpression<Func<T, bool>>(whereExpression.ToString(), "q").Compile());
        }
        return enumerable;
    }

    private static string BuildCompareExpression(string propertyName, PropertyInfo propertyInfo, CompareType compareType)
    {
        return compareType switch
        {
            CompareType.Equal => $"q.{propertyName} == o.{propertyInfo.Name}",
            CompareType.NotEqual => $"q.{propertyName} != o.{propertyInfo.Name}",
            CompareType.GreaterThan => $"q.{propertyName} > o.{propertyInfo.Name}",
            CompareType.GreaterThanOrEqual => $"q.{propertyName} >= o.{propertyInfo.Name}",
            CompareType.LessThan => $"q.{propertyName} <= o.{propertyInfo.Name}",
            CompareType.LessThanOrEqual => $"q.{propertyName} <= o.{propertyInfo.Name}",
            CompareType.Contains => $"o.{propertyInfo.Name}.Contains(q.{propertyName})",
            CompareType.StartsWith => $"q.{propertyName}.StartsWith(o.{propertyInfo.Name})",
            CompareType.EndsWith => $"q.{propertyName}.EndsWith(o.{propertyInfo.Name})",
            _ => throw new NotSupportedException()
        };
    }
}
