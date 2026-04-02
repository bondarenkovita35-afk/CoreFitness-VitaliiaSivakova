using Infrastructure.Identity;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Extensions;

// Här registrerar vi Infrastructure-tjänster
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        // Väljer databas beroende på miljö
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            // InMemory används i utvecklingsläge
            if (environment.IsDevelopment())
            {
                object value = options.UseInMemoryDatabase("CoreFitnessDevDb");
            }
            else
            {
                // SQL Server används i produktionsläge
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        });

        // Aktiverar Identity med vår egen användarklass
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            // Enkel lösenordskonfiguration för projektet
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // Konfigurerar vart användaren skickas om den inte är inloggad
        services.ConfigureApplicationCookie(options =>
        {
            // Sida för inloggning
            options.LoginPath = "/Auth/SignIn";

            // Sida efter logout
            options.LogoutPath = "/Auth/Logout";

            // Sida om åtkomst nekas
            options.AccessDeniedPath = "/Auth/SignIn";
        });

        return services;
    }
}