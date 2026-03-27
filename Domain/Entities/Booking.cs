using Domain.Common;

namespace Domain.Entities;

// Representerar en bokning av ett träningspass
public class Booking : BaseEntity
{
    // Användarens ID
    public string UserId { get; set; } = null!;

    // ID för träningspasset
    public int TrainingClassId { get; set; }

    // Tidpunkt för bokning
    public DateTime BookedAt { get; set; } = DateTime.UtcNow;

    // Navigation till träningspasset
    public TrainingClass TrainingClass { get; set; } = null!;
}