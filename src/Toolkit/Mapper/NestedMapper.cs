using AutoMapper;
using Mttechne.Toolkit.Interfaces;

namespace Mttechne.Toolkit.Mapper;

internal class NestedMapper : IGenericMapper, INestedMapper, IExpressionMapper
{
    private AutoMapper.Mapper _Mapper;
    private MapperConfigurationExpression _Expression;

    public TDestination Map<TSource, TDestination>(TSource source)
    {
        CreateMap();
        return _Mapper.Map<TDestination>(source);
    }

    public IGenericMapper Build<TSource, TDestination>()
    {
        return Nest<TSource, TDestination>() as IGenericMapper;
    }

    public IGenericMapper BuildExpressions()
    {
        return this;
    }

    public INestedMapper Nest<TSource, TDestination>()
    {
        CreateExpression<TSource, TDestination>();
        return this;
    }

    private IMappingExpression<TSource, TDestination> CreateExpression<TSource, TDestination>()
    {
        if (_Expression == null)
            _Expression = new MapperConfigurationExpression();

        return _Expression.CreateMap<TSource, TDestination>();
    }

    public IExpressionMapper CreateExpression<TSource, TDestination>(List<ExpressionMap<TSource, TDestination>> expressions)
    {
        if (_Expression == null)
            _Expression = new MapperConfigurationExpression();

        var createdMap = _Expression.CreateMap<TSource, TDestination>()
                                    .IgnoreAllPropertiesWithAnInaccessibleSetter()
                                    .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                                    .ConstructUsingServiceLocator();

        foreach (var exp in expressions)
        {
            try
            {
                createdMap.ForMember(exp.To, opt => opt.MapFrom(exp.From));
            }
            catch (ArgumentException)
            {
                createdMap.ForPath(exp.To, opt => opt.MapFrom(exp.From));
            }
        }

        return this;
    }

    public IExpressionMapper CreateExpression<TSource, TDestination>(List<ExpressionMap<TSource, TDestination>> expressions, Action<TSource, TDestination> afterFunction)
    {
        if (_Expression == null)
            _Expression = new MapperConfigurationExpression();

        var createdMap = _Expression.CreateMap<TSource, TDestination>()
                                    .IgnoreAllPropertiesWithAnInaccessibleSetter()
                                    .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                                    .ConstructUsingServiceLocator();

        foreach (var exp in expressions)
        {
            try
            {
                createdMap.ForMember(exp.To, opt => opt.MapFrom(exp.From));
            }
            catch (ArgumentException)
            {
                createdMap.ForPath(exp.To, opt => opt.MapFrom(exp.From));
            }
        }

        createdMap.AfterMap(afterFunction);

        return this;
    }

    private void CreateMap()
    {
        if (_Mapper != null)
            return;
        var cfg = new MapperConfiguration(_Expression);
        _Mapper = new AutoMapper.Mapper(cfg);
    }
}