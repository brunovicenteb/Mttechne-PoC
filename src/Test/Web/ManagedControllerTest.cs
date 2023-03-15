using System.Net;
using Mttechne.Toolkit.Web;
using Mttechne.Test.Web.Mock;
using Microsoft.AspNetCore.Mvc;
using Mttechne.Toolkit.Exceptions;

namespace Mttechne.Test.Web;

public class ManagedControllerTest : ManagedController
{
    private List<string> _ObjectsToDelete = new List<string>(){ "Pineapple", "Orange", "Strawberry", "Coffe?" };

    [Theory]
    [InlineData("Hello World!")]
    [InlineData("Coffe")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task TryExecuteOkReturnStringTest(string printText)
    {
        //arrange
        //act
        IActionResult actionResult = await TryExecuteOkAct(printText);
        //assert
        OkObjectResult okResult = actionResult as OkObjectResult;

        Assert.NotNull(okResult);
        Assert.True(okResult is ObjectResult);
        Assert.IsAssignableFrom<ActionResult>(actionResult);
        Assert.Equal(printText, okResult.Value);
    }

    [Theory]
    [InlineData("Pineapple", (int)HttpStatusCode.NoContent)]
    [InlineData("Orange", (int)HttpStatusCode.NoContent)]
    [InlineData("", (int)HttpStatusCode.NotFound)]
    [InlineData(null, (int)HttpStatusCode.NotFound)]
    public async Task TryExecuteDeleteTest(string objectDelete, int expectedStatusCode)
    {
        //arrange
        int countListBeforeDeleted = _ObjectsToDelete.Count;
        //act
        IActionResult actionResult = await TryExecuteDeleteAct(objectDelete);
        //assert
        ObjectResult result = actionResult as ObjectResult;
        int countListAfterDeleted = _ObjectsToDelete.Count;

        Assert.NotNull(result);
        Assert.True(result is ObjectResult);
        Assert.IsAssignableFrom<ActionResult>(actionResult);
        Assert.Equal(expectedStatusCode, result.StatusCode);

        if (expectedStatusCode == (int)HttpStatusCode.NoContent)
            Assert.Equal((countListBeforeDeleted - 1), countListAfterDeleted);
        else
            Assert.Equal(countListBeforeDeleted, countListAfterDeleted);
    }

    [Theory]
    [ClassData(typeof(ExceptionsControllerMock))]
    public async Task TryExecuteOkExceptionsTest(BaseException exception, HttpStatusCode statusCode)
    {
        //arrange
        //act
        IActionResult actionResult = await TryExecuteOkAct("", exception);
        //assert
        ObjectResult result = actionResult as ObjectResult;

        Assert.NotNull(result);
        Assert.True(result is ObjectResult);
        Assert.IsAssignableFrom<ActionResult>(actionResult);
        Assert.Equal((int)statusCode, result.StatusCode);
    }

    private async Task<IActionResult> TryExecuteDeleteAct(string objectDelete)
    {
        Func<Task<object>> execute = async delegate
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (!_ObjectsToDelete.Any(a => a == objectDelete))
                        throw new NotFoundException("The object does not exist in the list");

                    _ObjectsToDelete.Remove(objectDelete);

                    return true;
                }
                catch
                {
                    throw;
                }
            });
        };

        return await TryExecuteDelete(execute);
    }

    private async Task<IActionResult> TryExecuteOkAct(string printText, BaseException exception = null)
    {
        Func<Task<object>> execute = async delegate
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (exception is not null)
                        throw exception;

                    return printText;
                }
                catch (Exception)
                {
                    throw;
                }
            });
        };

        return await TryExecuteOK(execute);
    }
}