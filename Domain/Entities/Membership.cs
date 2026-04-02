using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

// Representerar ett medlemskap
public class Membership : BaseEntity
{
    // Namn på medlemskapet
    public string Name { get; private set; } = null!;

    // Pris
    public decimal Price { get; private set; }

    // Om medlemskapet är aktivt
    public bool IsActive { get; private set; }

    // Koppling till användare
    public string UserId { get; private set; } = null!;

    // Tom konstruktor för EF Core
    private Membership()
    {
    }

    // Konstruktor med validering
    public Membership(string name, decimal price, string userId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidMembershipException("Membership name is required.");

        if (price <= 0)
            throw new InvalidMembershipException("Membership price must be greater than zero.");

        if (string.IsNullOrWhiteSpace(userId))
            throw new InvalidMembershipException("UserId is required.");

        Name = name;
        Price = price;
        UserId = userId;
        IsActive = true;
    }

    // Uppdaterar medlemskapet
    public void Update(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidMembershipException("Membership name is required.");

        if (price <= 0)
            throw new InvalidMembershipException("Membership price must be greater than zero.");

        Name = name;
        Price = price;
    }

    // Avaktiverar medlemskapet
    public void Deactivate()
    {
        IsActive = false;
    }

    // Aktiverar medlemskapet
    public void Activate()
    {
        IsActive = true;
    }
}