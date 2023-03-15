using AutoMapper;
using System.Linq.Expressions;
using Mttechne.Toolkit.Mapper;

namespace Mttechne.Toolkit.Interfaces;

public interface INestedMapper
{
    INestedMapper Nest<TSource, TDestination>();

    IGenericMapper Build<TSource, TDestination>();
}

public interface IGenericMapper
{
    TDestination Map<TSource, TDestination>(TSource source);

}
public interface IExpressionMapper
{
    IExpressionMapper CreateExpression<TSource, TDestination>(List<ExpressionMap<TSource, TDestination>> expression);
    IExpressionMapper CreateExpression<TSource, TDestination>(List<ExpressionMap<TSource, TDestination>> expressions, Action<TSource, TDestination> afterFunction);
    IGenericMapper BuildExpressions();
}