using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models;

// ViewModel för registrering
public class RegisterViewModel
{
    // Förnamn
    [Required(ErrorMessage = "Förnamn är obligatoriskt")]
    public string FirstName { get; set; } = null!;

    // Efternamn
    [Required(ErrorMessage = "Efternamn är obligatoriskt")]
    public string LastName { get; set; } = null!;

    // E-postadress
    [Required(ErrorMessage = "E-post är obligatoriskt")]
    [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
    public string Email { get; set; } = null!;

    // Lösenord
    [Required(ErrorMessage = "Lösenord är obligatoriskt")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    // Bekräfta lösenord
    [Required(ErrorMessage = "Bekräfta lösenord är obligatoriskt")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Lösenorden matchar inte")]
    public string ConfirmPassword { get; set; } = null!;
}