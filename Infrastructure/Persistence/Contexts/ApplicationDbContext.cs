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
    {
    }

    // Tabell för medlemskap
    public DbSet<Membership> Memberships { get; set; }

    // Tabell för träningspass
    public DbSet<TrainingClass> TrainingClasses { get; set; }

    // Tabell för bokningar
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Kör basklassens konfiguration först
        base.OnModelCreating(modelBuilder);

        // Anger precision för pris
        modelBuilder.Entity<Membership>()
            .Property(m => m.Price)
            .HasPrecision(10, 2);

        // Skapar relation mellan Booking och TrainingClass
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.TrainingClass)
            .WithMany()
            .HasForeignKey(b => b.TrainingClassId)
            .OnDelete(DeleteBehavior.Cascade);

        // Hindrar dubbelbokning av samma pass för samma användare
        modelBuilder.Entity<Booking>()
            .HasIndex(b => new { b.UserId, b.TrainingClassId })
            .IsUnique();
    }
}