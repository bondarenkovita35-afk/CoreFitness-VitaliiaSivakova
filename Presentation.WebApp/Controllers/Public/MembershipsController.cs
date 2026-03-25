using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers.Public
{
    public class MembershipsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}