using System.Linq;
using Mttechne.Toolkit;

namespace Mttechne.Test;
public class StringTest
{
    [Theory]
    [InlineData("Has value!", false)]
    [InlineData("", true)]
    [InlineData(" ", false)]
    [InlineData("Also has value!", false)]
    [InlineData(null, true)]
    public void IsEmptyValueFunctionTest(string valueForTest, bool expectedValue)
    {
        //arrange
        //act
        var result = valueForTest.IsEmpty();

        //assert
        Assert.IsType<bool>(result);
        Assert.Equal(expectedValue, result);

        if (result)
            Assert.True(string.IsNullOrEmpty(valueForTest));
        else
            Assert.NotEmpty(valueForTest);
    }

    [Theory]
    [InlineData("Has value!", true)]
    [InlineData("", false)]
    [InlineData(" ", true)]
    [InlineData("Also has value!", true)]
    [InlineData(null, false)]
    public void IsFilledValueFunctionTest(string valueForTest, bool expectedValue)
    {
        //arrange
        //act
        var result = valueForTest.IsFilled();

        //assert
        Assert.IsType<bool>(result);
        Assert.Equal(expectedValue, result);

        if (result)
            Assert.NotEmpty(valueForTest);
        else
            Assert.True(string.IsNullOrEmpty(valueForTest));
    }

    [Theory]
    [InlineData("is lower!", "is lower!")]
    [InlineData("IS UPPER", "is upper")]
    [InlineData("iT's bOth", "it's both")]
    [InlineData("", "")]
    [InlineData(null, "")]
    public void SafeToLowerFunctionTest(string valueForTest, string expectedValue)
    {
        //arrange
        //act
        var result = valueForTest.SafeToLower();
        var resultOnlyLetters = new String(result.Where(c => Char.IsLetter(c)).ToArray());

        //assert
        Assert.IsType<string>(result);
        Assert.True(resultOnlyLetters.All(char.IsLower));
        Assert.Equal(expectedValue, result);
    }

    [Theory]
    [InlineData("is lower!", "IS LOWER!")]
    [InlineData("IS UPPER", "IS UPPER")]
    [InlineData("iT's bOth", "IT'S BOTH")]
    [InlineData("", "")]
    [InlineData(null, "")]
    public void SafeToUperFunctionTest(string valueForTest, string expectedValue)
    {
        //arrange
        //act
        var result = valueForTest.SafeToUpper();
        var resultOnlyLetters = new String(result.Where(c => Char.IsLetter(c)).ToArray());

        //assert
        Assert.IsType<string>(result);
        Assert.True(resultOnlyLetters.All(char.IsUpper));
        Assert.Equal(expectedValue, result);
    }

    [Theory]
    [InlineData("481.431.670-46")]
    [InlineData("890.564.540-21")]
    [InlineData("496.851.180-94")]
    [InlineData("692.144.560-70")]
    public void ValidCpfTest(string valueForTest)
    {
        //arrange
        //act
        var validDocument = valueForTest.IsValidCPF();

        //assert
        Assert.IsType<bool>(validDocument);
        Assert.True(validDocument);
    }

    [Theory]
    [InlineData("481.431.670")]
    [InlineData("190.564.540-21")]
    [InlineData("557.851.180-94")]
    [InlineData("")]
    [InlineData(null)]
    public void InvalidCpfTest(string valueForTest)
    {
        //arrange
        //act
        var validDocument = valueForTest.IsValidCPF();

        //assert
        Assert.IsType<bool>(validDocument);
        Assert.False(validDocument);
    }

    [Theory]
    [InlineData(-10)]
    [InlineData(-1)]
    public void RaiseExceptionWithNegativeLimitsTest(int limitValueForTest)
    {
        //arrange
        var valueForTest = "Lorem ipsum dolor sit blandit.";

        //act
        Action returnedValue = () => valueForTest.Limit(limitValueForTest);

        //assert
        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(returnedValue);
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Theory]
    [InlineData("Amet.", 20)]
    [InlineData("Lorem ipsum dolor sit amet accumsan.", 40)]
    [InlineData(null, 10)]
    public void InvalidLimitTest(string valueForTest, int limitValueForTest)
    {
        //arrange

        //act
        var returnedValue = valueForTest.Limit(limitValueForTest);

        //assert
        if (valueForTest is not null)
            Assert.IsType<string>(returnedValue);

        Assert.NotEqual(returnedValue?.Length, limitValueForTest);
    }

    [Theory]
    [InlineData("Lorem ipsum dolor sit blandit.", 10)]
    [InlineData("Lorem ipsum dolor sit blandit.", 20)]
    [InlineData("Lorem ipsum dolor sit blandit.", 30)]
    public void ValidLimitTest(string valueForTest, int limitValueForTest)
    {
        //arrange

        //act
        var returnedValue = valueForTest.Limit(limitValueForTest);

        //assert
        Assert.Equal(returnedValue.Length, limitValueForTest);
    }

    [Theory]
    [InlineData("I need a coffe", "SSBuZWVkIGEgY29mZmU=")]
    [InlineData("I'm making good code", "SSdtIG1ha2luZyBnb29kIGNvZGU=")]
    [InlineData(null, "")]
    public void ValidBase64EncodeTest(string valueForTest, string expectedValue)
    {
        //arrange

        //act
        var returnedValue = valueForTest.Base64Encode();

        //assert
        Assert.Equal(returnedValue, expectedValue);
    }

    [Theory]
    [InlineData("SSBuZWVkIGEgY29mZmU=", "I need a coffe")]
    [InlineData("SSdtIG1ha2luZyBnb29kIGNvZGU=", "I'm making good code")]
    [InlineData(null, "")]
    public void ValidBase64DecodeTest(string valueForTest, string expectedValue)
    {
        //arrange

        //act
        var returnedValue = valueForTest.Base64Decode();

        //assert
        Assert.Equal(returnedValue, expectedValue);
    }

    [Theory]
    [InlineData("Not a test", 1)]
    [InlineData("050", 50)]
    [InlineData(null, 0)]
    public void ValidToIntDefaultTest(string valueForTest, int expectedValue)
    {
        //arrange
        int defaultValue = expectedValue;

        //act
        var returnedValue = valueForTest.ToIntDefault(defaultValue);

        //assert
        Assert.IsType<int>(returnedValue);
        Assert.Equal(returnedValue, expectedValue);
    }

    [Theory]
    [InlineData("carro", 1)]
    [InlineData("cinquenta", 50)]
    [InlineData(null, 20)]
    public void InvalidToIntDefaultTest(string valueForTest, int defaultValue)
    {
        //arrange

        //act
        var returnedValue = valueForTest.ToIntDefault(defaultValue);

        //assert
        Assert.Equal(defaultValue, returnedValue);
    }
}