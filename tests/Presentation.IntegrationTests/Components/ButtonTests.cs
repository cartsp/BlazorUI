using Presentation.Components;
using Bunit;
using AwesomeAssertions;

namespace Presentation.IntegrationTests.Components;

public class ButtonTests : BunitContext
{
    [Fact]
    public void Button_ShouldRenderChildContent()
    {
        // Arrange & Act
        var cut = Render<Button>(parameters => parameters
            .Add(p => p.ChildContent, "Click me"));

        // Assert
        cut.Markup.Should().Contain("Click me");
    }

    [Fact]
    public void Button_ShouldApplyPrimaryVariantClasses()
    {
        // Arrange & Act
        var cut = Render<Button>(parameters => parameters
            .Add(p => p.Variant, ButtonVariant.Primary)
            .Add(p => p.ChildContent, "Primary"));

        // Assert
        cut.Markup.Should().Contain("bg-blue-600");
    }

    [Fact]
    public void Button_ShouldApplySecondaryVariantClasses()
    {
        // Arrange & Act
        var cut = Render<Button>(parameters => parameters
            .Add(p => p.Variant, ButtonVariant.Secondary)
            .Add(p => p.ChildContent, "Secondary"));

        // Assert
        cut.Markup.Should().Contain("bg-gray-200");
    }

    [Fact]
    public void Button_ShouldApplyDangerVariantClasses()
    {
        // Arrange & Act
        var cut = Render<Button>(parameters => parameters
            .Add(p => p.Variant, ButtonVariant.Danger)
            .Add(p => p.ChildContent, "Danger"));

        // Assert
        cut.Markup.Should().Contain("bg-red-600");
    }

    [Fact]
    public void Button_ShouldApplySmallSizeClasses()
    {
        // Arrange & Act
        var cut = Render<Button>(parameters => parameters
            .Add(p => p.Size, ButtonSize.Small)
            .Add(p => p.ChildContent, "Small"));

        // Assert
        cut.Markup.Should().Contain("px-3 py-1.5 text-sm");
    }

    [Fact]
    public void Button_ShouldApplyMediumSizeClasses()
    {
        // Arrange & Act
        var cut = Render<Button>(parameters => parameters
            .Add(p => p.Size, ButtonSize.Medium)
            .Add(p => p.ChildContent, "Medium"));

        // Assert
        cut.Markup.Should().Contain("px-4 py-2 text-base");
    }

    [Fact]
    public void Button_ShouldApplyLargeSizeClasses()
    {
        // Arrange & Act
        var cut = Render<Button>(parameters => parameters
            .Add(p => p.Size, ButtonSize.Large)
            .Add(p => p.ChildContent, "Large"));

        // Assert
        cut.Markup.Should().Contain("px-6 py-3 text-lg");
    }

    [Fact]
    public void Button_ShouldBeDisabled_WhenDisabledIsTrue()
    {
        // Arrange & Act
        var cut = Render<Button>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.ChildContent, "Disabled"));

        // Assert
        cut.Find("button").HasAttribute("disabled").Should().BeTrue();
        cut.Markup.Should().Contain("disabled:opacity-50");
    }

    [Fact]
    public async Task Button_ShouldInvokeOnClick_WhenClicked()
    {
        // Arrange
        var clicked = false;
        var cut = Render<Button>(parameters => parameters
            .Add(p => p.ChildContent, "Click me")
            .Add(p => p.OnClick, () => clicked = true));

        // Act
        await cut.Find("button").ClickAsync();

        // Assert
        clicked.Should().BeTrue();
    }

    [Fact]
    public async Task Button_ShouldNotInvokeOnClick_WhenDisabled()
    {
        // Arrange
        var clicked = false;
        var cut = Render<Button>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.ChildContent, "Disabled")
            .Add(p => p.OnClick, () => clicked = true));

        // Act
        await cut.Find("button").ClickAsync();

        // Assert
        clicked.Should().BeFalse();
    }

    [Fact]
    public void Button_ShouldApplyAdditionalClass()
    {
        // Arrange & Act
        var cut = Render<Button>(parameters => parameters
            .Add(p => p.AdditionalClass, "my-custom-class")
            .Add(p => p.ChildContent, "Custom"));

        // Assert
        cut.Markup.Should().Contain("my-custom-class");
    }
}
