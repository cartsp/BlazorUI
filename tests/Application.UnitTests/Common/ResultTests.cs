using AwesomeAssertions;

namespace Application.UnitTests.Common;

public class ResultTests
{
    [Fact]
    public void Success_ShouldReturnSuccessfulResult()
    {
        // Act
        var result = Domain.Common.Result<string>.Success("test");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be("test");
        result.Error.Should().BeNull();
    }

    [Fact]
    public void Failure_ShouldReturnFailedResult()
    {
        // Arrange
        var error = new Domain.Common.Error("TEST.ERROR", "Something went wrong");

        // Act
        var result = Domain.Common.Result<string>.Failure(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Value.Should().BeNull();
        result.Error.Should().Be(error);
    }
}
