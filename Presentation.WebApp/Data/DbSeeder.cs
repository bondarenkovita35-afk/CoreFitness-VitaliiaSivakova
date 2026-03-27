using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Presentation.WebApp.Data;

// Klass för att lägga in testdata i databasen
public static class DbSeeder
{
    public static async Task SeedTrainingClassesAsync(ApplicationDbContext context)
    {
        // Ser till att databasen finns
        await context.Database.MigrateAsync();

        // Om det redan finns träningspass, gör inget
        if (await context.TrainingClasses.AnyAsync())
            return;

        // Skapar testdata för träningspass
        var trainingClasses = new List<TrainingClass>
        {
            new TrainingClass
            {
                Title = "Morning Strength",
                Description = "Styrkepass för hela kroppen.",
                StartTime = DateTime.Now.AddDays(1).Date.AddHours(8),
                InstructorName = "Anna Svensson",
                Category = "Strength"
            },
            new TrainingClass
            {
                Title = "Cardio Burn",
                Description = "Högintensivt konditionspass.",
                StartTime = DateTime.Now.AddDays(1).Date.AddHours(17),
                InstructorName = "Johan Eriksson",
                Category = "Cardio"
            },
            new TrainingClass
            {
                Title = "Yoga Flow",
                Description = "Lugn yoga med fokus på balans och flexibilitet.",
                StartTime = DateTime.Now.AddDays(2).Date.AddHours(10),
                InstructorName = "Sara Lind",
                Category = "Yoga"
            }
        };

        // Lägger till passen i databasen
        await context.TrainingClasses.AddRangeAsync(trainingClasses);
        await context.SaveChangesAsync();
    }
}