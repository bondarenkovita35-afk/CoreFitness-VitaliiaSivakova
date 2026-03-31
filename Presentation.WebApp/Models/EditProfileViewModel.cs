using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models;

// ViewModel för att redigera användarens profil
public class EditProfileViewModel
{
    [Required(ErrorMessage = "First name is required.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = null!;

    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }
}