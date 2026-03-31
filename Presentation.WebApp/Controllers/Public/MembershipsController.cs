using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Presentation.WebApp.Controllers.Public;

// Controller för medlemskap
[Authorize]
public class MembershipsController : Controller
{
    private readonly ApplicationDbContext _context;

    // Konstruktor
    public MembershipsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Visar medlemskapssidan
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // Skapar eller uppdaterar medlemskap
    [HttpPost]
    public async Task<IActionResult> Create(string name, decimal price)
    {
        // Hämtar ID för inloggad användare
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            // Om ej inloggad → redirect till login
            return RedirectToAction("SignIn", "Auth");
        }
        // Säkerställer rätt pris om inget pris kom in
        if (price <= 0)
        {
            if (name == "Standard Membership")
            {
                price = 495.00m;
            }
            else if (name == "Premium Membership")
            {
                price = 595.00m;
            }
        }

                // Letar efter befintligt medlemskap
                var existingMembership = await _context.Memberships
                    .FirstOrDefaultAsync(m => m.UserId == userId);

        if (existingMembership != null)
        {
            // Uppdaterar befintligt medlemskap
            existingMembership.Name = name;
            existingMembership.Price = price;
            existingMembership.IsActive = true;
        }
        else
        {
            // Skapar nytt medlemskap
            var membership = new Membership
            {
                Name = name,
                Price = price,
                IsActive = true,
                UserId = userId
            };

            _context.Memberships.Add(membership);
        }

        // Sparar i databasen
        await _context.SaveChangesAsync();

        // Meddelande till användaren
        TempData["MembershipMessage"] = "Membership saved successfully";

        // Redirect till MyAccount
        return RedirectToAction("Index", "MyAccount");
    }
}