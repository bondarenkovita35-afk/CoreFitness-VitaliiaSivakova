using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Presentation.WebApp.Controllers.Public;

// SVENSKA: Controller för medlemskap
[Authorize]
public class MembershipsController : Controller
{
    private readonly ApplicationDbContext _context;

    // SVENSKA: Konstruktor
    public MembershipsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // SVENSKA: Visar medlemskapssidan
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // SVENSKA: Skapar eller uppdaterar medlemskap
    [HttpPost]
    public async Task<IActionResult> Create(string name, decimal price)
    {
        // SVENSKA: Hämtar ID för inloggad användare
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            // SVENSKA: Om ej inloggad → redirect till login
            return RedirectToAction("SignIn", "Auth");
        }

        // SVENSKA: Letar efter befintligt medlemskap
        var existingMembership = await _context.Memberships
            .FirstOrDefaultAsync(m => m.UserId == userId);

        if (existingMembership != null)
        {
            // SVENSKA: Uppdaterar befintligt medlemskap
            existingMembership.Name = name;
            existingMembership.Price = price;
            existingMembership.IsActive = true;
        }
        else
        {
            // SVENSKA: Skapar nytt medlemskap
            var membership = new Membership
            {
                Name = name,
                Price = price,
                IsActive = true,
                UserId = userId
            };

            _context.Memberships.Add(membership);
        }

        // SVENSKA: Sparar i databasen
        await _context.SaveChangesAsync();

        // SVENSKA: Meddelande till användaren
        TempData["MembershipMessage"] = "Membership saved successfully";

        // SVENSKA: Redirect till MyAccount
        return RedirectToAction("Index", "MyAccount");
    }
}