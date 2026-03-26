using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Contexts;

// Huvudklass för databas och Identity
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    // Konstruktor som tar emot options från Program.cs
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)

    { }
    

    // Tabell för medlemskap
    public DbSet<Membership> Memberships { get; set; }

    // Tabell för träningspass
    public DbSet<TrainingClass> TrainingClasses { get; set; }

    // Tabell för bokningar
    public DbSet<Booking> Bookings { get; set; }
}

