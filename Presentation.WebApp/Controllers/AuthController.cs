using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models;
using System.Security.Claims;

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
            // Lägger till rollen Member för ny användare
            await _userManager.AddToRoleAsync(user, "Member");
           
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ExternalLogin(string provider, string returnUrl = "/")
    {
        // Skapar redirect-url efter extern inloggning
        var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });

        // Startar extern inloggning med vald provider
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

        return Challenge(properties, provider);
    }

    [HttpGet]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = "/", string? remoteError = null)
    {
        if (remoteError != null)
        {
            TempData["AuthMessage"] = $"External login error: {remoteError}";
            return RedirectToAction("SignIn");
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            TempData["AuthMessage"] = "Could not load external login information.";
            return RedirectToAction("SignIn");
        }

        // Försöker logga in användaren med extern provider
        var signInResult = await _signInManager.ExternalLoginSignInAsync(
            info.LoginProvider,
            info.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true);

        if (signInResult.Succeeded)
        {
            return LocalRedirect(returnUrl ?? "/");
        }

        // Hämtar e-post från Google
        var email = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Email);
        var firstName = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.GivenName) ?? "Google";
        var lastName = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Surname) ?? "User";

        if (string.IsNullOrWhiteSpace(email))
        {
            TempData["AuthMessage"] = "Google did not return an email address.";
            return RedirectToAction("SignIn");
        }

        // Skapar ny användare om den inte finns
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                TempData["AuthMessage"] = "Could not create local user account.";
                return RedirectToAction("SignIn");
            }

            // Lägger till rollen Member för Google-användare
            if (!await _userManager.IsInRoleAsync(user, "Member"))
            {
                await _userManager.AddToRoleAsync(user, "Member");
            }

        }

        // Kopplar extern login till användaren
        var addLoginResult = await _userManager.AddLoginAsync(user, info);
        if (!addLoginResult.Succeeded)
        {
            TempData["AuthMessage"] = "Could not link Google login to local account.";
            return RedirectToAction("SignIn");
        }

        await _signInManager.SignInAsync(user, isPersistent: false);

        return LocalRedirect(returnUrl ?? "/");
    }
}