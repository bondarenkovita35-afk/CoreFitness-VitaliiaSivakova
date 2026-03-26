using Microsoft.AspNetCore.Mvc;

public class AuthController : Controller
{
    public IActionResult SignIn()
    {
        return View();
    }

    public IActionResult SignUp()
    {
        return View();
    }

    public IActionResult SetPassword()
    {
        return View();
    }
}