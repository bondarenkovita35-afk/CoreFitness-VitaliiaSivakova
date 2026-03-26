using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

// Representerar användaren i systemet
public class ApplicationUser : IdentityUser
{
    // Förnamn
    public string FirstName { get; set; } = null!;

    // Efternamn
    public string LastName { get; set; } = null!;
}