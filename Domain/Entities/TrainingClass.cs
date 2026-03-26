using Domain.Common;

namespace Domain.Entities;

// Representerar ett träningspass
public class TrainingClass : BaseEntity
{
    // Titel på passet
    public string Title { get; set; } = null!;

    // Beskrivning
    public string Description { get; set; } = null!;

    // Starttid
    public DateTime StartTime { get; set; }

    // Instruktörens namn
    public string InstructorName { get; set; } = null!;

    // Typ av träning
    public string Category { get; set; } = null!;
}