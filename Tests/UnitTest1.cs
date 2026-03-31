using Domain.Entities;
using Xunit;

namespace Tests;

// Tester för Membership-entiteten
public class MembershipTests
{
    [Fact]
    public void Membership_Should_Create_With_Correct_Values()
    {
        // Arrange
        var userId = "user-123";
        var membership = new Membership
        {
            Name = "Gold",
            Price = 499,
            IsActive = true,
            UserId = userId
        };

        // Assert
        Assert.Equal("Gold", membership.Name);
        Assert.Equal(499, membership.Price);
        Assert.True(membership.IsActive);
        Assert.Equal(userId, membership.UserId);
    }
}