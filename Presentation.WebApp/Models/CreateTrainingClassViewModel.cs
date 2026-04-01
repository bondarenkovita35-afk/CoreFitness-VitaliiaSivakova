using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models;

// ViewModel för att skapa träningspass
public class CreateTrainingClassViewModel
{
    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Category is required.")]
    public string Category { get; set; } = null!;

    [Required(ErrorMessage = "Instructor name is required.")]
    [Display(Name = "Instructor Name")]
    public string InstructorName { get; set; } = null!;

    [Required(ErrorMessage = "Start time is required.")]
    [Display(Name = "Start Time")]

    public DateTime StartTime { get; set; }


    [Required(ErrorMessage = "Description is required.")]

    public string Description { get; set; } = null!;
}