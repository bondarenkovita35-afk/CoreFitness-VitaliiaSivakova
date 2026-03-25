using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers.Public
{
    public class TrainingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}