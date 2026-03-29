using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
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

    // Konstruktor
    public MyAccountController(ApplicationDbContext context)
    {
        _context = context;
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

        // Hämtar användarens bokningar tillsammans med träningspass
        var bookings = await _context.Bookings
            .Include(b => b.TrainingClass)
            .Where(b => b.UserId == userId)
            .OrderBy(b => b.TrainingClass.StartTime)
            .ToListAsync();

        // Skapar ViewModel för sidan
        var model = new MyAccountViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? "",
            PhoneNumber = user.PhoneNumber,
            BookedTrainings = bookings.Select(b => new MyBookedTrainingViewModel
            {
                Title = b.TrainingClass.Title,
                Category = b.TrainingClass.Category,
                InstructorName = b.TrainingClass.InstructorName,
                StartTime = b.TrainingClass.StartTime
            }).ToList()
        };

        return View(model);
    }
}