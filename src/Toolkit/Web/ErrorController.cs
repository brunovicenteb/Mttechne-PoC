using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Mttechne.Toolkit.Web;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    [Route("error")]
    public ErrorResponse Error()
    {
        Response.StatusCode = StatusCodes.Status500InternalServerError;
        var id = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
        return new ErrorResponse(id);
    }
}