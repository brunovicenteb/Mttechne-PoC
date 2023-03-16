using Microsoft.AspNetCore.Mvc;

namespace Mttechne.UI.Web.Controllers;

public class ErrorsController : Controller
{
    [HttpGet]
    public IActionResult PageNotFound()
    {
        return View();
    }
}