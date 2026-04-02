using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Presentation.WebApp.Controllers.Public;

// Controller för bokning av träningspass
[Authorize(Roles = "Member,Admin")]
public class BookingController : Controller
{
    private readonly ApplicationDbContext _context;

    // Konstruktor
    public BookingController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Avbokar ett träningspass
    [HttpPost]
    public async Task<IActionResult> Cancel(int bookingId)
    {
        // Hämtar ID för inloggad användare
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("SignIn", "Auth");
        }

        // Hämtar bokningen som tillhör användaren
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);

        if (booking == null)
        {
            TempData["BookingMessage"] = "Bokningen kunde inte hittas.";
            return RedirectToAction("Index", "MyAccount");
        }

        // Tar bort bokningen från databasen
        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();

        TempData["BookingMessage"] = "Bokningen har tagits bort.";
        return RedirectToAction("Index", "MyAccount");
    }

    // Bokar ett träningspass
    [HttpPost]
    public async Task<IActionResult> Book(int trainingClassId)
    {
        try
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
                throw new DuplicateBookingException();
            }

            // Skapar ny bokning via domänkonstruktor
            var booking = new Booking(userId, trainingClassId);

            // Sparar bokningen i databasen
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["BookingMessage"] = "Passet har bokats!";
            return RedirectToAction("Index", "Training");
        }
        catch (DuplicateBookingException ex)
        {
            TempData["BookingMessage"] = ex.Message;
            return RedirectToAction("Index", "Training");
        }
    }
}