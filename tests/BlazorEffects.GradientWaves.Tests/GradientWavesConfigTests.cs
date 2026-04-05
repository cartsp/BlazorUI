using BlazorEffects.GradientWaves;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.GradientWaves.Tests;

public class GradientWavesConfigTests
{
    [Fact]
    public void DefaultConfig_ShouldHaveExpectedValues()
    {
        var config = new GradientWavesConfig();

        config.Colors.Should().BeEquivalentTo(GradientWavesPresets.Stripe);
        config.PointCount.Should().Be(6);
        config.BlobSize.Should().Be(0.5);
        config.Speed.Should().Be(0.004);
        config.BlurAmount.Should().Be(80);
        config.BlendMode.Should().Be("normal");
        config.Opacity.Should().Be(1.0);
        config.TargetFps.Should().Be(60);
    }

    [Fact]
    public void Config_ShouldImplementIEffectConfig()
    {
        var config = new GradientWavesConfig();
        config.Should().BeAssignableTo<IEffectConfig>();
    }

    [Fact]
    public void Config_WithCustomValues_ShouldStoreCorrectly()
    {
        var config = new GradientWavesConfig
        {
            Colors = ["#ff0000", "#00ff00", "#0000ff"],
            PointCount = 4,
            BlobSize = 0.3,
            Speed = 0.01,
            BlurAmount = 120,
            BlendMode = "screen",
            Opacity = 0.5,
            TargetFps = 30
        };

        config.Colors.Should().BeEquivalentTo(["#ff0000", "#00ff00", "#0000ff"]);
        config.PointCount.Should().Be(4);
        config.BlobSize.Should().Be(0.3);
        config.Speed.Should().Be(0.01);
        config.BlurAmount.Should().Be(120);
        config.BlendMode.Should().Be("screen");
        config.Opacity.Should().Be(0.5);
        config.TargetFps.Should().Be(30);
    }

    [Fact]
    public void ConfigRecords_WithSameValues_ShouldMatch()
    {
        var config1 = new GradientWavesConfig { PointCount = 5, Speed = 0.008 };
        var config2 = new GradientWavesConfig { PointCount = 5, Speed = 0.008 };

        config1.PointCount.Should().Be(config2.PointCount);
        config1.Speed.Should().Be(config2.Speed);
        config1.BlendMode.Should().Be(config2.BlendMode);
        config1.Opacity.Should().Be(config2.Opacity);
    }

    [Fact]
    public void ConfigRecords_WithDifferentPointCount_ShouldHaveDifferentValues()
    {
        var config1 = new GradientWavesConfig { PointCount = 4 };
        var config2 = new GradientWavesConfig { PointCount = 8 };

        config1.PointCount.Should().NotBe(config2.PointCount);
    }

    [Fact]
    public void Config_WithSameColorArrays_ShouldMatchElementWise()
    {
        var colors1 = new[] { "#ff0000", "#00ff00" };
        var colors2 = new[] { "#ff0000", "#00ff00" };
        var config1 = new GradientWavesConfig { Colors = colors1 };
        var config2 = new GradientWavesConfig { Colors = colors2 };

        config1.Colors.Should().BeEquivalentTo(config2.Colors);
    }
}
