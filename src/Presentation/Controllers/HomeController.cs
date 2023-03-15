using Microsoft.AspNetCore.Mvc;

namespace Mttechne.UI.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}