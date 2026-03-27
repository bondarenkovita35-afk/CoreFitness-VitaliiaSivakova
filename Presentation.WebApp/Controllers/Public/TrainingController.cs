using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.WebApp.Controllers.Public;

// Controller för träningssidan
public class TrainingController : Controller
{
    private readonly ApplicationDbContext _context;

    // Konstruktor
    public TrainingController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Visar alla träningspass
    public async Task<IActionResult> Index()
    {
        // Hämtar alla träningspass från databasen
        var trainingClasses = await _context.TrainingClasses
            .OrderBy(t => t.StartTime)
            .ToListAsync();

        return View(trainingClasses);
    }
}