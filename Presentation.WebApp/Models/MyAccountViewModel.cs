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

    // Lista med bokade träningspass
    public List<MyBookedTrainingViewModel> BookedTrainings { get; set; } = new();
}

// ViewModel för en bokad träning
public class MyBookedTrainingViewModel
{
    // Titel på passet
    public string Title { get; set; } = null!;

    // Kategori
    public string Category { get; set; } = null!;

    // Instruktör
    public string InstructorName { get; set; } = null!;

    // Starttid
    public DateTime StartTime { get; set; }
}