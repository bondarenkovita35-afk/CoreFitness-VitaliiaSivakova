using Domain.Entities;
using Domain.Exceptions;
using Xunit;

namespace Tests;

// Tester för domänentiteter
public class MembershipTests
{
    [Fact]
    public void Membership_Should_Create_With_Correct_Values()
    {
        // Arrange
        var userId = "user-123";
        var membership = new Membership("Gold", 499, userId);

        // Assert
        Assert.Equal("Gold", membership.Name);
        Assert.Equal(499, membership.Price);
        Assert.True(membership.IsActive);
        Assert.Equal(userId, membership.UserId);
    }

    [Fact]
    public void Membership_Should_Throw_Exception_When_Name_Is_Empty()
    {
        // Arrange + Assert
        Assert.Throws<InvalidMembershipException>(() =>
            new Membership("", 499, "user-123"));
    }

    [Fact]
    public void Membership_Should_Throw_Exception_When_Price_Is_Invalid()
    {
        // Arrange + Assert
        Assert.Throws<InvalidMembershipException>(() =>
            new Membership("Silver", 0, "user-123"));
    }

    [Fact]
    public void Membership_Should_Be_Deactivated()
    {
        // Arrange
        var membership = new Membership("Silver", 299, "user-789");

        // Act
        membership.Deactivate();

        // Assert
        Assert.False(membership.IsActive);
    }

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
}