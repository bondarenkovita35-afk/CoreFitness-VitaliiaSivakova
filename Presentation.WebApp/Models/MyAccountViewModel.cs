namespace Presentation.WebApp.Models;

// ViewModel för sidan My Account
public class MyAccountViewModel
{
    // Förnamn
    public string FirstName { get; set; } = null!;

    // Efternamn
    public string LastName { get; set; } = null!;

    // E-postadress
    public string Email { get; set; } = null!;

    // Telefonnummer
    public string? PhoneNumber { get; set; }

    // Användarens medlemskap
    public MyMembershipViewModel? Membership { get; set; }

    // Lista med bokade träningspass
    public List<MyBookedTrainingViewModel> BookedTrainings { get; set; } = new();
}

// ViewModel för medlemskap
public class MyMembershipViewModel
{
    // Namn på medlemskapet
    public string Name { get; set; } = null!;

    // Pris
    public decimal Price { get; set; }

    // Om medlemskapet är aktivt
    public bool IsActive { get; set; }
}

// ViewModel för ett bokat träningspass
public class MyBookedTrainingViewModel
{
    // Bokningens ID
    public int BookingId { get; set; }

    // Titel på träningspasset
    public string Title { get; set; } = null!;

    // Kategori
    public string Category { get; set; } = null!;

    // Instruktörens namn
    public string InstructorName { get; set; } = null!;

    // Starttid
    public DateTime StartTime { get; set; }
}