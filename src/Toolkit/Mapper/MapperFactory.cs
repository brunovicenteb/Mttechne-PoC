using AutoMapper;
using System.Linq.Expressions;
using Mttechne.Toolkit.Interfaces;

namespace Mttechne.Toolkit.Mapper;

public static class MapperFactory
{
    public static IGenericMapper Map<TSource, TDestination>()
    {
        return Nest<TSource, TDestination>() as IGenericMapper;
    }

    public static INestedMapper Nest<TSource, TDestination>()
    {
        var mapper = new NestedMapper();
        return mapper.Nest<TSource, TDestination>();
    }

    public static IExpressionMapper CreateExpression<TSource, TDestination>(List<ExpressionMap<TSource, TDestination>> expression)
    {
        var mapper = new NestedMapper();
        return mapper.CreateExpression(expression);
    }

    public static IExpressionMapper CreateExpression<TSource, TDestination>(List<ExpressionMap<TSource, TDestination>> expression, Action<TSource, TDestination> afterFunction)
    {
        var mapper = new NestedMapper();
        return mapper.CreateExpression(expression, afterFunction);
    }
}