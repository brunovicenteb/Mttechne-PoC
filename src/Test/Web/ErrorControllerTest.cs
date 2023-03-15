using Mttechne.Toolkit.Web;
using Mttechne.Test.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mttechne.Test.Web;

public class ErrorControllerTest : StarterIoC<FakeContext>
{
    protected override void DoRegisterResources(WebApplicationBuilder builder)
    {
        base.DoRegisterResources(builder);
        builder.Services.AddScoped<ErrorController>();
    }

    [Fact]
    public void ErrorTest()
    {
        //arrange
        var cancelTokenSource = new CancellationTokenSource();
        var errorController = Provider.GetRequiredService<ErrorController>();
        errorController.ControllerContext.HttpContext = new DefaultHttpContext()
        {
            RequestAborted = cancelTokenSource.Token
        };

        //act
        var errorResponse = errorController.Error();

        //assert
        Assert.NotNull(errorResponse);
        Assert.Equal(StatusCodes.Status500InternalServerError, errorController.Response.StatusCode);
        Assert.Equal(errorController.HttpContext.TraceIdentifier, errorResponse.ID);
    }
}