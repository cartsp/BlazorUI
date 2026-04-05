using Presentation.Theming;
using AwesomeAssertions;

namespace Presentation.IntegrationTests.Theming;

public class ThemeTests
{
    [Fact]
    public void Light_ShouldHaveLightMode()
    {
        // Assert
        Theme.Light.Mode.Should().Be(ThemeMode.Light);
        Theme.Light.Name.Should().Be("Light");
    }

    [Fact]
    public void Dark_ShouldHaveDarkMode()
    {
        // Assert
        Theme.Dark.Mode.Should().Be(ThemeMode.Dark);
        Theme.Dark.Name.Should().Be("Dark");
    }

    [Fact]
    public void WithTokenOverride_ShouldReturnNewThemeWithOverride()
    {
        // Arrange
        var theme = Theme.Light;

        // Act
        var overridden = theme.WithTokenOverride("color-primary", "#ff0000");

        // Assert
        overridden.TokenOverrides["color-primary"].Should().Be("#ff0000");
        overridden.Mode.Should().Be(ThemeMode.Light);
        theme.TokenOverrides.Should().BeEmpty(); // original unchanged
    }

    [Fact]
    public void WithTokenOverride_ShouldUpdateExistingOverride()
    {
        // Arrange
        var theme = Theme.Light
            .WithTokenOverride("color-primary", "#ff0000")
            .WithTokenOverride("color-primary", "#00ff00");

        // Assert
        theme.TokenOverrides["color-primary"].Should().Be("#00ff00");
        theme.TokenOverrides.Should().HaveCount(1);
    }

    [Fact]
    public void ClearTokenOverrides_ShouldReturnThemeWithEmptyOverrides()
    {
        // Arrange
        var theme = Theme.Light
            .WithTokenOverride("color-primary", "#ff0000")
            .WithTokenOverride("color-secondary", "#00ff00");

        // Act
        var cleared = theme.ClearTokenOverrides();

        // Assert
        cleared.TokenOverrides.Should().BeEmpty();
        cleared.Mode.Should().Be(ThemeMode.Light);
    }

    [Fact]
    public void ThemeRecords_ShouldHaveValueEqualityForSameInstance()
    {
        // Arrange
        var theme = Theme.Light;

        // Assert
        theme.Should().Be(theme);
    }

    [Fact]
    public void DifferentThemes_ShouldNotBeEqual()
    {
        // Arrange
        var light = Theme.Light;
        var dark = Theme.Dark;

        // Assert
        light.Should().NotBe(dark);
    }
}
