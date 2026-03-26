namespace Domain.Common;

// Basklass för alla entiteter i systemet
public abstract class BaseEntity
{
    // Primärnyckel (unik identifierare)
    public int Id { get; set; }
}