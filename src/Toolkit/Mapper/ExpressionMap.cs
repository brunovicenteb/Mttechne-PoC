using System.Linq.Expressions;

namespace Mttechne.Toolkit.Mapper;

public class ExpressionMap<TSource, TDestination>
{
    public ExpressionMap(Expression<Func<TSource, object>> from, Expression<Func<TDestination, object>> to)
    {
        From = from;
        To = to;
    }

    public Expression<Func<TSource, object>> From { get; private set; }
    public Expression<Func<TDestination, object>> To { get; private set; }
}