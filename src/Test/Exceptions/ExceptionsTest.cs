using Mttechne.Toolkit.Exceptions;
using Mttechne.Test.Exceptions.Mock;

namespace Mttechne.Test.Exceptions;
public class ExceptionsTest
{
    [Theory]
    [ClassData(typeof(ExceptionsMock))]
    public void GetCustomExceptionTest(BaseException exception)
    {
        //arrange
        //act
        var excetionType = exception.GetType();
        var excetionName = exception.Message;

        //assert
        Assert.Equal(typeof(BaseException), excetionType.BaseType);
        Assert.Equal(typeof(Exception), excetionType.BaseType.BaseType);
        Assert.IsType(excetionType, exception);
        Assert.Contains(excetionType.Name, excetionName);
    }
}