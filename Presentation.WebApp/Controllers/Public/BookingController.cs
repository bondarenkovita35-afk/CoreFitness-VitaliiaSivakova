using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Presentation.WebApp.Controllers.Public;

// Controller för bokning av träningspass
[Authorize]
public class BookingController : Controller
{
    private readonly ApplicationDbContext _context;

    // Konstruktor
    public BookingController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Bokar ett träningspass
    [HttpPost]
    public async Task<IActionResult> Book(int trainingClassId)
    {
        // Hämtar inloggad användares ID
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("SignIn", "Auth");
        }

        // Kontrollerar om användaren redan har bokat passet
        var existingBooking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.UserId == userId && b.TrainingClassId == trainingClassId);

        if (existingBooking != null)
        {
            TempData["BookingMessage"] = "Du har redan bokat detta pass.";
            return RedirectToAction("Index", "Training");
        }

        // Skapar ny bokning
        var booking = new Booking
        {
            UserId = userId,
            TrainingClassId = trainingClassId,
            BookedAt = DateTime.UtcNow
        };

        // Sparar bokningen i databasen
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        TempData["BookingMessage"] = "Passet har bokats!";
        return RedirectToAction("Index", "Training");
    }
}