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

        var booking = new Booking
        {
            UserId = "test-user"
            // ❗ УБРАЛИ Guid и CreatedAt
        };

        // Act
        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        // Assert
        var result = await context.Bookings.FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.Equal("test-user", result.UserId);
    }
}