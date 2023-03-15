using Mttechne.Toolkit;

namespace Mttechne.Test;
public class ArraysTest
{
    [Theory]
    [InlineData("a dog", "a cat", "a bug")]
    [InlineData("a water", "aaah coffe")]
    [InlineData(null, "a bug")]
    [InlineData(null)]
    [InlineData(-20, -10 - 1, 0, 1, 10, 20)]
    [InlineData("1999-12-31 23:59:00", "2000-01-01 01:00:00")]
    public void SafeLengthArrayTest(params object[] arrayTest)
    {
        //arrange
        //act
        var numberOfPositionsInArray = arrayTest.SafeLength();

        //assert
        Assert.IsType<int>(numberOfPositionsInArray);
        if (arrayTest is null)
            Assert.Equal(0, numberOfPositionsInArray);

    }
}