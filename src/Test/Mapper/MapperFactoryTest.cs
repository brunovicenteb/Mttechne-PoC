using Mttechne.Toolkit.Interfaces;
using Mttechne.Toolkit.Mapper;

namespace Mttechne.Test.Mapper;
public class MapperFactoryTest
{
    #region ClassAux
    private class SimpleOne<T, V>
    {
        public T ValueOne { get; set; }
        public V ValueTwo { get; set; }
    }

    private class SimpleTwo<T, V>
    {
        public T ValueOne { get; set; }
        public V ValueTwo { get; set; }
    }
    private class SimpleThree<T, V>
    {
        public T AnotherNameOne { get; set; }
        public V AnotherNameTwo { get; set; }
    }

    private class ComplexOne<T, V>
    {
        public T ValueOne { get; set; }
        public V ValueTwo { get; set; }
        public SimpleOne<T, V> Child { get; set; }
    }

    private class ComplexTwo<T, V>
    {
        public T ValueOne { get; set; }
        public V ValueTwo { get; set; }
        public SimpleTwo<T, V> Child { get; set; }
    }

    private class SimpleThreeToSimpleOneMap : BaseMap<SimpleOne<int, string>, SimpleThree<int, string>>
    {
        public SimpleThreeToSimpleOneMap()
        {
            AddMapExpression(f => f.ValueOne, d => d.AnotherNameOne);
            AddMapExpression(f => f.ValueTwo, d => d.AnotherNameTwo);
        }
    }
    #endregion

    [Theory]
    [InlineData(0, "Zero")]
    [InlineData(1, "One")]
    [InlineData(-21, "-Twenty 1")]
    public void TestSimpleConvertionWithExpression(int valueOne, string valueTwo)
    {
        //arrange
        IGenericMapper mapping = MapperFactory
            .CreateExpression(new SimpleThreeToSimpleOneMap().Mappings)
            .BuildExpressions();

        var source = new SimpleOne<int, string> { ValueOne = valueOne, ValueTwo = valueTwo };

        //act
        var target = mapping.Map<SimpleOne<int, string>, SimpleThree<int, string>>(source);

        //assert
        Assert.NotNull(target);
        Assert.Equal(valueOne, target.AnotherNameOne);
        Assert.Equal(valueTwo, target.AnotherNameTwo);
    }

    [Theory]
    [InlineData(0, "Zero")]
    [InlineData(1, "One")]
    [InlineData(-21, "-Twenty 1")]
    public void TestSimpleConvertion(int valueOne, string valueTwo)
    {
        //arrange
        IGenericMapper mapping = MapperFactory.Map<SimpleOne<int, string>, SimpleTwo<int, string>>();
        var source = new SimpleOne<int, string> { ValueOne = valueOne, ValueTwo = valueTwo };

        //act
        var target = mapping.Map<SimpleOne<int, string>, SimpleTwo<int, string>>(source);

        //assert
        Assert.NotNull(target);
        Assert.Equal(valueOne, target.ValueOne);
        Assert.Equal(valueTwo, target.ValueTwo);
        Assert.IsType<SimpleTwo<int, string>>(target);
    }

    [Theory]
    [InlineData(0, "Zero")]
    [InlineData(2, "Two")]
    [InlineData(-22, "-Twenty Two")]
    public void TestNestedConvertionWithJustChild(int valueOne, string valueTwo)
    {
        //arrange
        IGenericMapper mapping = MapperFactory.Nest<ComplexOne<int, string>, ComplexTwo<int, string>>()
            .Build<SimpleOne<int, string>, SimpleTwo<int, string>>();
        var source = new SimpleOne<int, string> { ValueOne = valueOne, ValueTwo = valueTwo };

        //act
        var target = mapping.Map<SimpleOne<int, string>, SimpleTwo<int, string>>(source);

        //assert
        Assert.NotNull(target);
        Assert.Equal(valueOne, target.ValueOne);
        Assert.Equal(valueTwo, target.ValueTwo);
        Assert.IsType<SimpleTwo<int, string>>(target);
    }

    [Theory]
    [InlineData(0, "Zero")]
    [InlineData(3, "Three")]
    [InlineData(-23, "-Twenty Three")]
    public void TestNestedConvertionMappingDiferentTypes(int valueOne, string valueTwo)
    {
        //arrange
        IGenericMapper mapping = MapperFactory.Nest<ComplexOne<int, string>, ComplexTwo<int, string>>()
            .Build<SimpleOne<int, string>, SimpleTwo<int, string>>();
        var complexSource = new ComplexOne<int, string> { ValueOne = valueOne, ValueTwo = valueTwo };
        var simpleSource = new SimpleOne<int, string> { ValueOne = valueOne, ValueTwo = valueTwo };

        //act
        var complexTarget = mapping.Map<ComplexOne<int, string>, ComplexTwo<int, string>>(complexSource);
        var simpleTarget = mapping.Map<SimpleOne<int, string>, SimpleTwo<int, string>>(simpleSource);

        //assert
        Assert.NotNull(simpleTarget);
        Assert.Equal(valueOne, simpleTarget.ValueOne);
        Assert.Equal(valueTwo, simpleTarget.ValueTwo);
        Assert.IsType<SimpleTwo<int, string>>(simpleTarget);

        Assert.NotNull(complexTarget);
        Assert.Null(complexTarget.Child);
        Assert.Equal(valueOne, complexTarget.ValueOne);
        Assert.Equal(valueTwo, complexTarget.ValueTwo);
        Assert.IsType<ComplexTwo<int, string>>(complexTarget);
    }

    [Theory]
    [InlineData(0, "Zero")]
    [InlineData(4, "Four")]
    [InlineData(-24, "-Twenty Four")]
    public void TestComplexConvertionWithChild(int valueOne, string valueTwo)
    {
        //arrange
        IGenericMapper mapping = MapperFactory.Nest<ComplexOne<int, string>, ComplexTwo<int, string>>()
            .Build<SimpleOne<int, string>, SimpleTwo<int, string>>();
        var source = new ComplexOne<int, string> { ValueOne = valueOne, ValueTwo = valueTwo };
        source.Child = new SimpleOne<int, string> { ValueOne = valueOne, ValueTwo = valueTwo };

        //act
        var target = mapping.Map<ComplexOne<int, string>, ComplexTwo<int, string>>(source);

        //assert
        Assert.NotNull(target);
        Assert.NotNull(target.Child);
        Assert.Equal(valueOne, target.ValueOne);
        Assert.Equal(valueOne, target.Child.ValueOne);
        Assert.Equal(valueTwo, target.ValueTwo);
        Assert.Equal(valueTwo, target.Child.ValueTwo);
        Assert.IsType<ComplexTwo<int, string>>(target);
    }

    [Theory]
    [InlineData(0, "Zero")]
    [InlineData(5, "Five")]
    [InlineData(-25, "-Twenty Five")]
    public void TestComplexConvertionWithoutChild(int valueOne, string valueTwo)
    {
        //arrange
        IGenericMapper mapping = MapperFactory.Nest<ComplexOne<int, string>, ComplexTwo<int, string>>()
            .Build<SimpleOne<int, string>, SimpleTwo<int, string>>();
        var source = new ComplexOne<int, string> { ValueOne = valueOne, ValueTwo = valueTwo };

        //act
        var target = mapping.Map<ComplexOne<int, string>, ComplexTwo<int, string>>(source);

        //assert
        Assert.NotNull(target);
        Assert.Null(target.Child);
        Assert.Equal(valueOne, target.ValueOne);
        Assert.Equal(valueTwo, target.ValueTwo);
        Assert.IsType<ComplexTwo<int, string>>(target);
    }

    [Fact]
    public void TestMapperConvertError()
    {
        //arrange
        IGenericMapper genericMapping = MapperFactory.Nest<SimpleOne<string, int>, SimpleTwo<string, int>>()
            .Build<SimpleTwo<string, int>, SimpleOne<string, int>>();

        SimpleOne<string, int> letterPAndNumberOne = new SimpleOne<string, int> { ValueOne = "P", ValueTwo = 1 };

        //act
        var converted = genericMapping.Map<SimpleOne<string, int>, SimpleTwo<string, int>>(letterPAndNumberOne);

        Action convertionError = () => { genericMapping.Map<SimpleOne<string, int>, SimpleTwo<int, string>>(letterPAndNumberOne); };
        var exception = Assert.Throws<AutoMapper.AutoMapperMappingException>(convertionError);

        //assert
        Assert.NotNull(exception);
        Assert.IsType<SimpleTwo<string, int>>(converted);
    }
}