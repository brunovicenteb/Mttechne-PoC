using System.Linq.Expressions;

namespace Mttechne.Toolkit.Mapper;

public abstract class BaseMap<TSource, TDestination>
{
    public List<ExpressionMap<TSource, TDestination>> Mappings { get; private set; } = new List<ExpressionMap<TSource, TDestination>>();
    public Action<TSource, TDestination> afterFunction;

    protected void AddMapExpression(Expression<Func<TSource, object>> from, Expression<Func<TDestination, object>> to)
    {
        Mappings.Add
        (
            new ExpressionMap<TSource, TDestination>
            (
               from,
               to
            )
        );
    }
}
