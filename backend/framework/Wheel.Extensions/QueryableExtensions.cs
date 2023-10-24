using JetBrains.Annotations;
using System.Linq.Expressions;

namespace System.Linq
{
    public static class QueryableExtensions
    {

        public static IQueryable<T> PageBy<T>([NotNull] this IQueryable<T> query, int skipCount, int maxResultCount)
        {
            return query.Skip(skipCount).Take(maxResultCount);
        }
        public static TQueryable PageBy<T, TQueryable>([NotNull] this TQueryable query, int skipCount, int maxResultCount)
            where TQueryable : IQueryable<T>
        {
            return (TQueryable)query.Skip(skipCount).Take(maxResultCount);
        }

        public static IQueryable<T> WhereIf<T>([NotNull] this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

        public static TQueryable WhereIf<T, TQueryable>([NotNull] this TQueryable query, bool condition, Expression<Func<T, bool>> predicate)
            where TQueryable : IQueryable<T>
        {
            return condition
                ? (TQueryable)query.Where(predicate)
                : query;
        }

        public static IQueryable<T> WhereIf<T>([NotNull] this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

        public static TQueryable WhereIf<T, TQueryable>([NotNull] this TQueryable query, bool condition, Expression<Func<T, int, bool>> predicate)
            where TQueryable : IQueryable<T>
        {
            return condition
                ? (TQueryable)query.Where(predicate)
                : query;
        }

        public static TQueryable OrderByIf<T, TQueryable>([NotNull] this TQueryable query, bool condition, string sorting)
            where TQueryable : IQueryable<T>
        {
            return condition
                ? (TQueryable)Dynamic.Core.DynamicQueryableExtensions.OrderBy(query, sorting)
                : query;
        }
    }
}
