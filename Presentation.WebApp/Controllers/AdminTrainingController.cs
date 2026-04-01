using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models;
using Domain.Entities;

namespace Presentation.WebApp.Controllers;

// Controller för admin och hantering av träningspass
[Authorize(Roles = "Admin")]
public class AdminTrainingController : Controller
{
    private readonly ApplicationDbContext _context;

    // Konstruktor
    public AdminTrainingController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Visar formulär för att skapa träningspass
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // Sparar nytt träningspass
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTrainingClassViewModel model)
    {
        // Kontrollerar formuläret
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Skapar nytt träningspass
        var trainingClass = new TrainingClass
        {
            Title = model.Title,
            Category = model.Category,
            InstructorName = model.InstructorName,
            StartTime = model.StartTime,
            Description = model.Description
        };

        _context.TrainingClasses.Add(trainingClass);
        await _context.SaveChangesAsync();

        TempData["TrainingMessage"] = "Training class created successfully.";

        return RedirectToAction("Index", "Training");
    }

    // Tar bort ett träningspass
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        // Hämtar träningspasset
        var trainingClass = await _context.TrainingClasses.FindAsync(id);

        if (trainingClass == null)
        {
            TempData["TrainingMessage"] = "Training class not found.";
            return RedirectToAction("Index", "Training");
        }

        // Tar först bort bokningar kopplade till passet
        var bookings = _context.Bookings.Where(b => b.TrainingClassId == id);
        _context.Bookings.RemoveRange(bookings);

        // Tar bort träningspasset
        _context.TrainingClasses.Remove(trainingClass);
        await _context.SaveChangesAsync();

        TempData["TrainingMessage"] = "Training class deleted successfully.";

        return RedirectToAction("Index", "Training");
    }
}