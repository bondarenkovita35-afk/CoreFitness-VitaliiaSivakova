using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers.Public
{
    public class CustomerServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}