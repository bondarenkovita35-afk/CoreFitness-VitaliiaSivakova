using Xunit;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Contexts;
using Domain.Entities;

namespace Tests.IntegrationTests;

public class BookingIntegrationTests
{
    private ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task Should_Create_Booking_InMemory()
    {
        // Arrange
        var context = CreateDbContext();

        var booking = new Booking("test-user", 10);

        // Act
        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        // Assert
        var result = await context.Bookings.FirstOrDefaultAsync();

        Assert.NotNull(result);
        var savedBooking = result!;

        Assert.Equal("test-user", savedBooking.UserId);
        Assert.Equal(10, savedBooking.TrainingClassId);
    }
}