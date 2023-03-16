using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mttechne.Application.Interfaces;

namespace Mttechne.UI.Web.Controllers;

public class ConsolidateController : Controller
{
    public ConsolidateController(IMovementAppService service)
    {
        _service = service;
    }

    private readonly IMovementAppService _service;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var consolidate =await  _service.GetTotalizersAsync();
        return View(consolidate);
    }
}