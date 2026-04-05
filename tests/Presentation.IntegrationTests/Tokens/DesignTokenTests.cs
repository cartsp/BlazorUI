using Presentation.Tokens;
using AwesomeAssertions;

namespace Presentation.IntegrationTests.Tokens;

public class DesignTokenTests
{
    [Fact]
    public void AllSpacing_ShouldContain13Values()
    {
        // Act
        var spacing = DesignTokens.AllSpacing;

        // Assert
        spacing.Should().HaveCount(13);
        spacing[0].Value.Should().Be("0px");
        spacing[^1].Value.Should().Be("6rem");
    }

    [Fact]
    public void Space4_ShouldBe1Rem()
    {
        // Assert
        DesignTokens.Space4.Name.Should().Be("4");
        DesignTokens.Space4.Value.Should().Be("1rem");
    }

    [Fact]
    public void AllTypography_ShouldContainFontSizesAndWeights()
    {
        // Act
        var typography = DesignTokens.AllTypography;

        // Assert
        typography.Should().HaveCount(12); // 8 sizes + 4 weights
    }

    [Fact]
    public void TextBase_ShouldBe1Rem()
    {
        // Assert
        DesignTokens.TextBase.Value.Should().Be("1rem");
    }

    [Fact]
    public void Text4xl_ShouldBe2_25Rem()
    {
        // Assert
        DesignTokens.Text4xl.Value.Should().Be("2.25rem");
    }

    [Fact]
    public void AllShadows_ShouldContain4Values()
    {
        // Act
        var shadows = DesignTokens.AllShadows;

        // Assert
        shadows.Should().HaveCount(4);
    }

    [Fact]
    public void ShadowSm_ShouldStartWithCorrectValue()
    {
        // Assert
        DesignTokens.ShadowSm.Name.Should().Be("sm");
        DesignTokens.ShadowSm.Value.Should().StartWith("0 1px");
    }

    [Fact]
    public void AllBorderRadius_ShouldContain6Values()
    {
        // Act
        var radius = DesignTokens.AllBorderRadius;

        // Assert
        radius.Should().HaveCount(6);
    }

    [Fact]
    public void RadiusFull_ShouldBe9999px()
    {
        // Assert
        DesignTokens.RadiusFull.Value.Should().Be("9999px");
    }

    [Fact]
    public void RadiusNone_ShouldBe0px()
    {
        // Assert
        DesignTokens.RadiusNone.Value.Should().Be("0px");
    }

    [Fact]
    public void AllTransitions_ShouldContain3Values()
    {
        // Act
        var transitions = DesignTokens.AllTransitions;

        // Assert
        transitions.Should().HaveCount(3);
    }

    [Fact]
    public void TransitionFast_ShouldBe150ms()
    {
        // Assert
        DesignTokens.TransitionFast.Value.Should().Be("150ms ease");
    }

    [Fact]
    public void TransitionSlow_ShouldBe300ms()
    {
        // Assert
        DesignTokens.TransitionSlow.Value.Should().Be("300ms ease");
    }

    [Fact]
    public void ColorTokenRecords_ShouldBeImmutable()
    {
        // Arrange
        var token = new ColorToken("test", "#000000", "test color");

        // Assert
        token.Name.Should().Be("test");
        token.Value.Should().Be("#000000");
        token.Description.Should().Be("test color");
    }

    [Fact]
    public void SpacingTokenRecords_ShouldSupportNullDescription()
    {
        // Arrange
        var token = new SpacingToken("4", "1rem");

        // Assert
        token.Description.Should().BeNull();
    }

    [Fact]
    public void Primary_ShouldBeValid()
    {
        // Assert
        DesignTokens.Primary.Name.Should().Be("primary");
        DesignTokens.Primary.Value.Should().Be("#3b82f6");
        DesignTokens.Primary.Description.Should().Be("Primary brand color");
    }

    [Fact]
    public void Error_ShouldBeValid()
    {
        // Assert
        DesignTokens.Error.Name.Should().Be("error");
        DesignTokens.Error.Value.Should().Be("#ef4444");
    }
}
