using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

// Representerar en bokning av ett träningspass
public class Booking : BaseEntity
{
    public string UserId { get; private set; } = null!;
    public int TrainingClassId { get; private set; }
    public DateTime BookedAt { get; private set; }

    // Navigation property till träningspass
    public TrainingClass? TrainingClass { get; set; }

    private Booking()
    {
    }

    public Booking(string userId, int trainingClassId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new DomainException("UserId is required.");

        if (trainingClassId <= 0)
            throw new DomainException("TrainingClassId must be greater than zero.");

        UserId = userId;
        TrainingClassId = trainingClassId;
        BookedAt = DateTime.UtcNow;
    }
}