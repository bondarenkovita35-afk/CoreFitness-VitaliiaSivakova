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
    // Tester för Booking-entiteten
    [Fact]
    public void Booking_Should_Create_With_Correct_Values()
    {
        // Arrange
        var booking = new Booking
        {
            UserId = "user-456",
            TrainingClassId = 10
        };

        // Assert
        Assert.Equal("user-456", booking.UserId);
        Assert.Equal(10, booking.TrainingClassId);
    }

    // Tester för TrainingClass-entiteten
    [Fact]
    public void TrainingClass_Should_Create_With_Correct_Values()
    {
        // Arrange
        var trainingClass = new TrainingClass
        {
            Title = "Yoga Flow",
            Category = "Yoga",
            InstructorName = "Anna"
        };

        // Assert
        Assert.Equal("Yoga Flow", trainingClass.Title);
        Assert.Equal("Yoga", trainingClass.Category);
        Assert.Equal("Anna", trainingClass.InstructorName);
    }

    // Tester för att säkerställa att Membership kan ha inaktiv status
    [Fact]
    public void Membership_Should_Allow_Inactive_Status()
    {
        // Arrange
        var membership = new Membership
        {
            Name = "Silver",
            Price = 299,
            IsActive = false,
            UserId = "user-789"
        };

        // Assert
        Assert.Equal("Silver", membership.Name);
        Assert.Equal(299, membership.Price);
        Assert.False(membership.IsActive);
        Assert.Equal("user-789", membership.UserId);
    }
}


