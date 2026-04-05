using AwesomeAssertions;
using NSubstitute;

namespace BlazorEffects.Playground.Tests;

public class EffectRegistryTests
{
    private readonly EffectRegistry _sut;

    public EffectRegistryTests()
    {
        _sut = new EffectRegistry();
    }

    // --- Register and retrieve by name ---

    [Fact]
    public void GetByName_WithRegisteredEffect_ShouldReturnAdapter()
    {
        // Arrange
        var adapter = CreateTestAdapter("MyEffect");

        _sut.Register(adapter);

        // Act
        var result = _sut.GetByName("MyEffect");

        // Assert
        result.Should().BeSameAs(adapter);
    }

    [Fact]
    public void GetByName_WithUnknownEffect_ShouldThrowInvalidOperationException()
    {
        // Act
        var act = () => _sut.GetByName("NonExistent");

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*NonExistent*");
    }

    [Fact]
    public void GetByName_WithUnknownEffect_ShouldIncludeEffectNameInMessage()
    {
        // Act
        var act = () => _sut.GetByName("MissingEffect");

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*MissingEffect*");
    }

    // --- GetAll ---

    [Fact]
    public void GetAll_WithNoRegistrations_ShouldReturnEmptyList()
    {
        // Act
        var result = _sut.GetAll();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetAll_WithMultipleRegistrations_ShouldReturnAll()
    {
        // Arrange
        var adapter1 = CreateTestAdapter("Effect1");
        var adapter2 = CreateTestAdapter("Effect2");
        var adapter3 = CreateTestAdapter("Effect3");

        _sut.Register(adapter1);
        _sut.Register(adapter2);
        _sut.Register(adapter3);

        // Act
        var result = _sut.GetAll();

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(adapter1);
        result.Should().Contain(adapter2);
        result.Should().Contain(adapter3);
    }

    [Fact]
    public void GetAll_ShouldReturnInRegistrationOrder()
    {
        // Arrange
        var adapter1 = CreateTestAdapter("First");
        var adapter2 = CreateTestAdapter("Second");
        var adapter3 = CreateTestAdapter("Third");

        _sut.Register(adapter1);
        _sut.Register(adapter2);
        _sut.Register(adapter3);

        // Act
        var result = _sut.GetAll();

        // Assert
        result[0].EffectName.Should().Be("First");
        result[1].EffectName.Should().Be("Second");
        result[2].EffectName.Should().Be("Third");
    }

    // --- IReadOnlyList return type ---

    [Fact]
    public void GetAll_ShouldReturnReadOnlyList()
    {
        // Act
        var result = _sut.GetAll();

        // Assert
        result.Should().BeAssignableTo<IReadOnlyList<IEffectDescriptorAdapter>>();
    }

    // --- Helper ---

    private static IEffectDescriptorAdapter CreateTestAdapter(string effectName)
    {
        var descriptor = Substitute.For<IEffectDescriptorAdapter>();
        descriptor.EffectName.Returns(effectName);
        return descriptor;
    }
}
