using Xunit;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Contexts;
using Domain.Entities;

namespace Tests.IntegrationTests;

public class MembershipIntegrationTests
{
    private ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task Should_Create_Membership_InMemory()
    {
        // Arrange
        var context = CreateDbContext();

        var membership = new Membership("Gold", 499, "test-user");

        // Act
        context.Memberships.Add(membership);
        await context.SaveChangesAsync();

        // Assert
        var result = await context.Memberships.FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.Equal("Gold", result.Name);
        Assert.Equal(499, result.Price);
        Assert.True(result.IsActive);
        Assert.Equal("test-user", result.UserId);
    }
}