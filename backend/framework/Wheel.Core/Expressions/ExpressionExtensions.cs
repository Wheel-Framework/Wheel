namespace System.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var parameter = left.Parameters[0];

            var body = Expression.AndAlso(left.Body, RebindParameter(right.Body, right.Parameters[0], parameter));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var parameter = left.Parameters[0];

            var body = Expression.OrElse(left.Body, RebindParameter(right.Body, right.Parameters[0], parameter));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private static Expression RebindParameter(Expression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            return new ParameterRebinder(oldParameter, newParameter).Visit(expression);
        }

        private class ParameterRebinder(ParameterExpression oldParameter, ParameterExpression newParameter)
            : ExpressionVisitor
        {
            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (node == oldParameter)
                    return newParameter;

                return base.VisitParameter(node);
            }
        }
    }
}
