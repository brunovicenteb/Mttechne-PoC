using Serilog;
using Mttechne.Toolkit.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Mttechne.Toolkit.Web;

public abstract class ManagedController : ControllerBase
{
    protected async Task<IActionResult> TryExecuteOK(Func<Task<object>> pExecute)
    {
        Func<object, IActionResult> action = delegate (object result)
        {
            return Ok(result);
        };
        return await TryExecute(action, pExecute);
    }

    protected async Task<IActionResult> TryExecuteDelete(Func<Task<object>> pExecute)
    {
        Func<object, IActionResult> action = delegate (object result)
        {
            bool sucess = (bool)result;
            return new ObjectResult(null) { StatusCode = sucess ? (int)HttpStatusCode.NoContent : (int)HttpStatusCode.NotFound };
        };
        return await TryExecute(action, pExecute);
    }

    protected async Task<IActionResult> TryExecute(Func<object, IActionResult> pResultFunc, Func<Task<object>> pExecute)
    {
        try
        {
            object result = await pExecute();
            return pResultFunc(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ForbidException ex)
        {
            return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.Forbidden };
        }
        catch (UnauthorizedException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (DomainRuleException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Unhandled error in running the Api.");
            return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.InternalServerError };
        }
    }
}