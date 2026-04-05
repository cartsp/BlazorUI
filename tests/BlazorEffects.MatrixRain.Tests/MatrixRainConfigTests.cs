using BlazorEffects.MatrixRain;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.MatrixRain.Tests;

public class MatrixRainConfigTests
{
    [Fact]
    public void DefaultConfig_ShouldHaveExpectedValues()
    {
        var config = new MatrixRainConfig();

        config.Characters.Should().Be(MatrixRainPresets.Classic);
        config.FontSize.Should().Be(16);
        config.FontFamily.Should().Be("monospace");
        config.Color.Should().Be("#00ff41");
        config.FadeColor.Should().Be("#003b00");
        config.Speed.Should().Be(1.0);
        config.Density.Should().Be(1.0);
        config.Opacity.Should().Be(0.8);
        config.TargetFps.Should().Be(30);
    }

    [Fact]
    public void Config_ShouldImplementIEffectConfig()
    {
        var config = new MatrixRainConfig();
        config.Should().BeAssignableTo<IEffectConfig>();
    }

    [Fact]
    public void Config_WithCustomValues_ShouldStoreCorrectly()
    {
        var config = new MatrixRainConfig
        {
            Characters = MatrixRainPresets.Binary,
            FontSize = 24,
            Color = "#ff0000",
            FadeColor = "#330000",
            Speed = 2.0,
            Density = 0.5,
            Opacity = 1.0,
            TargetFps = 60
        };

        config.Characters.Should().Be("01");
        config.FontSize.Should().Be(24);
        config.Color.Should().Be("#ff0000");
        config.FadeColor.Should().Be("#330000");
        config.Speed.Should().Be(2.0);
        config.Density.Should().Be(0.5);
        config.Opacity.Should().Be(1.0);
        config.TargetFps.Should().Be(60);
    }

    [Fact]
    public void ConfigRecords_WithSameValues_ShouldBeEqual()
    {
        var config1 = new MatrixRainConfig { Color = "#00ff41", Speed = 1.5 };
        var config2 = new MatrixRainConfig { Color = "#00ff41", Speed = 1.5 };

        config1.Should().Be(config2);
        config1.GetHashCode().Should().Be(config2.GetHashCode());
    }
}
