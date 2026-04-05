using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.IntegrationTests.Persistence;

public class BasicInfrastructureTests
{
    [Fact]
    public void ApplicationDbContext_ShouldBeCreatable()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<Infrastructure.Persistence.ApplicationDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        // Act
        using var context = new Infrastructure.Persistence.ApplicationDbContext(options);

        // Assert
        context.Should().NotBeNull();
    }
}
