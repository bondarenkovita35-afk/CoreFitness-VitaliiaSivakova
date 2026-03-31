using Infrastructure.Identity;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.WebApp.Models;
using System.Security.Claims;

namespace Presentation.WebApp.Controllers.Public;

// Controller för användarens privata sida
[Authorize]
public class MyAccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    // Konstruktor
    public MyAccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Visar sidan My Account
    public async Task<IActionResult> Index()
    {
        // Hämtar ID för inloggad användare
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("SignIn", "Auth");
        }

        // Hämtar användaren från databasen
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return RedirectToAction("SignIn", "Auth");
        }

        // Hämtar användarens medlemskap
        var membership = await _context.Memberships
            .FirstOrDefaultAsync(m => m.UserId == userId);

        // Hämtar användarens bokningar och tillhörande träningspass
        var bookings = await _context.Bookings
            .Include(b => b.TrainingClass)
            .Where(b => b.UserId == userId)
            .OrderBy(b => b.TrainingClass.StartTime)
            .ToListAsync();

        // Skapar ViewModel
        var model = new MyAccountViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? "",
            PhoneNumber = user.PhoneNumber,
            Membership = membership == null
                ? null
                : new MyMembershipViewModel
                {
                    Name = membership.Name,
                    Price = membership.Price,
                    IsActive = membership.IsActive
                },
            BookedTrainings = bookings.Select(b => new MyBookedTrainingViewModel
            {
                BookingId = b.Id,
                Title = b.TrainingClass.Title,
                Category = b.TrainingClass.Category,
                InstructorName = b.TrainingClass.InstructorName,
                StartTime = b.TrainingClass.StartTime
            }).ToList()
        };

        return View(model);
    }

    // Visar formuläret för att redigera profil
    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        // Hämtar ID för inloggad användare
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("SignIn", "Auth");
        }

        // Hämtar användaren från databasen
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return RedirectToAction("SignIn", "Auth");
        }

        // Fyller formuläret med nuvarande data
        var model = new EditProfileViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber
        };

        return View(model);
    }

    // Sparar ändringar i användarens profil
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditProfileViewModel model)
    {
        // Kontrollerar om formuläret är giltigt
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Hämtar ID för inloggad användare
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("SignIn", "Auth");
        }

        // Hämtar användaren från databasen
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return RedirectToAction("SignIn", "Auth");
        }

        // Uppdaterar användarens uppgifter
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.PhoneNumber = model.PhoneNumber;

        // Sparar ändringarna i databasen
        await _context.SaveChangesAsync();

        TempData["ProfileMessage"] = "Your profile has been updated.";

        return RedirectToAction(nameof(Index));
    }
    // Visar sida för att ta bort konto
    [HttpGet]
    public IActionResult Delete()
    {
        return View();
    }

    // Tar bort konto och all data
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed()
    {
        // Hämtar användarens ID
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return RedirectToAction("SignIn", "Auth");

        // Hämtar användaren
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return RedirectToAction("SignIn", "Auth");

        // Tar bort alla bokningar
        var bookings = _context.Bookings.Where(b => b.UserId == userId);
        _context.Bookings.RemoveRange(bookings);

        // Tar bort medlemskap
        var membership = await _context.Memberships.FirstOrDefaultAsync(m => m.UserId == userId);
        if (membership != null)
        {
            _context.Memberships.Remove(membership);
        }

        await _context.SaveChangesAsync();

        // Tar bort användaren via Identity
        await _userManager.DeleteAsync(user);

        // Loggar ut användaren
        await HttpContext.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }
}