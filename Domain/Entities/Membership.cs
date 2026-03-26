using Domain.Common;

namespace Domain.Entities;

// Representerar ett medlemskap
public class Membership : BaseEntity
{
    // Namn på medlemskapet
    public string Name { get; set; } = null!;

    // Pris
    public decimal Price { get; set; }

    // Om medlemskapet är aktivt
    public bool IsActive { get; set; }

    // Koppling till användare
    public string UserId { get; set; } = null!;
}