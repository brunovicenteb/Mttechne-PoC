using System.Linq;
using System.Threading.Tasks;
using AmbevWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmbevWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly AmbevContext _Context;

        public HomeController(AmbevContext pContext)
        {
            this._Context = pContext;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}