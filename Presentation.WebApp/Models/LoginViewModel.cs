using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models;

// ViewModel för inloggning
public class LoginViewModel
{
    // E-postadress
    [Required(ErrorMessage = "E-post är obligatoriskt")]
    [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
    public string Email { get; set; } = null!;

    // Lösenord
    [Required(ErrorMessage = "Lösenord är obligatoriskt")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    // Kom ihåg användaren
    public bool RememberMe { get; set; }
}