using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models;

namespace Presentation.WebApp.Controllers;

// Controller för autentisering
public class AuthController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    // Konstruktor
    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // Visar registreringssidan
    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    // Tar emot registreringsformuläret
    [HttpPost]
    public async Task<IActionResult> SignUp(RegisterViewModel model)
    {
        // Kontrollerar om formuläret är giltigt
        if (!ModelState.IsValid)
            return View(model);

        // Kontrollerar om användaren redan finns
        var existingUser = await _userManager.FindByEmailAsync(model.Email);

        if (existingUser != null)
        {
            ModelState.AddModelError(string.Empty, "Användaren finns redan");
            return View(model);
        }

        // Skapar ny användare
        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        // Sparar användaren i databasen
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Loggar in användaren direkt efter registrering
            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }

        // Visar Identity-fel på sidan
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    // Visar inloggningssidan
    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }

    // Tar emot inloggningsformuläret
    [HttpPost]
    public async Task<IActionResult> SignIn(LoginViewModel model)
    {
        // Kontrollerar om formuläret är giltigt
        if (!ModelState.IsValid)
            return View(model);

        // Försöker logga in användaren
        var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        // Visar fel om inloggning misslyckas
        ModelState.AddModelError(string.Empty, "Fel e-post eller lösenord");

        return View(model);
    }

    // Loggar ut användaren
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    // Visar sidan för att sätta lösenord
    [HttpGet]
    public IActionResult SetPassword()
    {
        return View();
    }
}