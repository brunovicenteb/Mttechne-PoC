using Mttechne.Toolkit;

namespace Mttechne.Test;
public class EnvironmentReaderTest
{
    #region EnvironmentReaderTestEnum
    public enum EnvironmentReaderTestEnum
    {
        One,
        Two,
        Three
    }
    #endregion

    [Theory]
    [InlineData("")]
    [InlineData("Ping")]
    public void TestReadEmptyStringVariableWitDefaultValue(string defaultValue)
    {
        //arrange
        var varName = nameof(TestReadStringVariableSucessWithDefaultValue);
        Environment.SetEnvironmentVariable(varName, defaultValue);
        //act
        string value = EnvironmentReader.Read(varName, defaultValue);
        //assert
        Assert.Equal(value, defaultValue);
    }

    [Theory]
    [InlineData(25)]
    [InlineData(45)]
    public void TestReadEmptyIntVariableWitDefaultValue(int defaultValue)
    {
        //arrange
        var varName = nameof(TestReadEmptyIntVariableWitDefaultValue);
        Environment.SetEnvironmentVariable(varName, string.Empty);
        //act
        int value = EnvironmentReader.Read(varName, defaultValue);
        //assert
        Assert.Equal(value, defaultValue);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]

    public void TestReadWithoutVarName(string varName)
    {
        //arrange
        //act
        var exception = Assert.Throws<ArgumentNullException>(() => EnvironmentReader.Read<string>(varName));
        //assert
        Assert.Equal("Value cannot be null. (Parameter 'varName')", exception.Message);
    }

    [Theory]
    [InlineData("Ping")]
    [InlineData("Pong")]
    public void TestReadStringVariableSucessWithValueEqualsDefault(string varValue)
    {
        var varName = nameof(TestReadStringVariableSucessWithDefaultValue);
        try
        {
            //arrange
            Environment.SetEnvironmentVariable(varName, varValue);
            //act
            string value = EnvironmentReader.Read<string>(varName, varValue);
            //assert
            Assert.Equal(varValue, value);
        }
        finally
        {
            Environment.SetEnvironmentVariable(varName, null);
        }
    }

    [Theory]
    [InlineData(1080)]
    [InlineData(7216)]
    public void TestReadIntVariableSucessWithValueEqualsDefault(int varValue)
    {
        var varName = nameof(TestReadStringVariableSucessWithDefaultValue);
        try
        {
            //arrange
            Environment.SetEnvironmentVariable(varName, varValue.ToString());
            //act
            var value = EnvironmentReader.Read<int>(varName, varValue);
            //assert
            Assert.Equal(varValue, value);
        }
        finally
        {
            Environment.SetEnvironmentVariable(varName, null);
        }
    }

    [Theory]
    [InlineData("Ping")]
    [InlineData("Pong")]
    public void TestReadStringVariableSucess(string varValue)
    {
        var varName = nameof(TestReadStringVariableSucessWithDefaultValue);
        try
        {
            //arrange
            Environment.SetEnvironmentVariable(varName, varValue);
            //act
            string value = EnvironmentReader.Read<string>(varName);
            //assert
            Assert.Equal(varValue, value);
        }
        finally
        {
            Environment.SetEnvironmentVariable(varName, null);
        }
    }

    [Theory]
    [InlineData("Ping", "Pont")]
    [InlineData("Tico", "Teco")]
    public void TestReadStringVariableSucessWithDefaultValue(string varValue, string defaultValue)
    {
        var varName = nameof(TestReadStringVariableSucessWithDefaultValue);
        try
        {
            //arrange
            Environment.SetEnvironmentVariable(varName, varValue);
            //act
            var value = EnvironmentReader.Read(varName, defaultValue);
            //assert
            Assert.Equal(varValue, value);
        }
        finally
        {
            Environment.SetEnvironmentVariable(varName, null);
        }
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    public void TestIntVariableSucess(int varValue)
    {
        var varName = nameof(TestIntVariableSucess);
        try
        {
            //arrange
            Environment.SetEnvironmentVariable(varName, varValue.ToString());
            //act
            var value = EnvironmentReader.Read<int>(varName);
            //assert
            Assert.Equal(varValue, value);
        }
        finally
        {
            Environment.SetEnvironmentVariable(varName, null);
        }
    }

    [Fact]
    public void TestRaiseExceptionWithUnassignedVariable()
    {
        //arrange
        var varName = nameof(TestRaiseExceptionWithUnassignedVariable);
        string message = $"Variable {varName} not assigned on environment";

        //act
        var exception = Assert.Throws<NullReferenceException>(() => EnvironmentReader.Read<string>(varName, varEmptyError: message));

        //assert
        Assert.Equal(message, exception.Message);
    }

    [Theory]
    [InlineData("Ping")]
    [InlineData("P0nG")]
    public void TestRaiseExceptionWithFormatException(string varValue)
    {
        var varName = nameof(TestRaiseExceptionWithFormatException);
        try
        {
            //arrange
            Environment.SetEnvironmentVariable(varName, varValue);
            //act
            var exception = Assert.Throws<FormatException>(() => EnvironmentReader.Read<int>(varName));
            //assert
            Assert.Equal($"The input string '{varValue}' was not in a correct format.", exception.Message);
        }
        finally
        {
            Environment.SetEnvironmentVariable(varName, null);
        }
    }

    [Theory]
    [InlineData("InvalidOne", EnvironmentReaderTestEnum.One)]
    [InlineData("InvalidTwo", EnvironmentReaderTestEnum.Two)]
    [InlineData("InvalidThree", EnvironmentReaderTestEnum.Three)]
    public void TestReadInvalidEnumVariableWitDefaultValue(string varValue, EnvironmentReaderTestEnum defaultValue)
    {
        //arrange
        var varName = $"{nameof(TestReadInvalidEnumVariableWitDefaultValue)}_{varValue}";
        try
        {
            Environment.SetEnvironmentVariable(varName, varValue);
            //act
            var value = EnvironmentReader.Read(varName, defaultValue);
            //assert
            Assert.Equal(value, defaultValue);
        }
        finally
        {
            Environment.SetEnvironmentVariable(varName, null);
        }
    }

    [Theory]
    [InlineData(EnvironmentReaderTestEnum.Two)]
    [InlineData(EnvironmentReaderTestEnum.Three)]
    public void TestReadEmptyEnumVariableWitDefaultValue(EnvironmentReaderTestEnum defaultValue)
    {
        //arrange
        var varName = $"{nameof(TestReadEmptyEnumVariableWitDefaultValue)}_{defaultValue}";
        Environment.SetEnvironmentVariable(varName, null);
        //act
        var value = EnvironmentReader.Read(varName, defaultValue);
        //assert
        Assert.Equal(defaultValue, value);
    }


    [Theory]
    [InlineData("One", EnvironmentReaderTestEnum.One)]
    [InlineData("Two", EnvironmentReaderTestEnum.Two)]
    [InlineData("Three", EnvironmentReaderTestEnum.Three)]
    public void TestReadEnumVariableSuccessfully(string varValue, EnvironmentReaderTestEnum correctValue)
    {
        //arrange
        var varName = $"{nameof(TestReadEnumVariableSuccessfully)}_{varValue}";
        try
        {
            Environment.SetEnvironmentVariable(varName, varValue);
            //act
            var value = EnvironmentReader.Read<EnvironmentReaderTestEnum>(varName);
            //assert
            Assert.Equal(value, correctValue);
        }
        finally
        {
            Environment.SetEnvironmentVariable(varName, null);
        }
    }
}