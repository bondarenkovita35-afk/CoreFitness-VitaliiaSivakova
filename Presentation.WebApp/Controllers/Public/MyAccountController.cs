using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers.Public;

// Controller för användarens privata sida
[Authorize]
public class MyAccountController : Controller
{
    // Visar sidan My Account
    public IActionResult Index()
    {
        return View();
    }
}