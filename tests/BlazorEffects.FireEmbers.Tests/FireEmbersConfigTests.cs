using BlazorEffects.FireEmbers;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.FireEmbers.Tests;

public class FireEmbersConfigTests
{
    [Fact]
    public void DefaultConfig_ShouldHaveExpectedValues()
    {
        var config = new FireEmbersConfig();

        config.ParticleCount.Should().Be(200);
        config.FlameColor.Should().Be("#ff6600");
        config.EmberColor.Should().Be("#ffcc00");
        config.Speed.Should().Be(1.5);
        config.ParticleSize.Should().Be(4.0);
        config.Turbulence.Should().Be(1.0);
        config.EmberRatio.Should().Be(0.3);
        config.BackgroundColor.Should().Be("#0a0a0a");
        config.Opacity.Should().Be(0.9);
        config.TargetFps.Should().Be(60);
    }

    [Fact]
    public void Config_ShouldImplementIEffectConfig()
    {
        var config = new FireEmbersConfig();
        config.Should().BeAssignableTo<IEffectConfig>();
    }

    [Fact]
    public void Config_WithCustomValues_ShouldStoreCorrectly()
    {
        var config = new FireEmbersConfig
        {
            ParticleCount = 400,
            FlameColor = "#ff2200",
            EmberColor = "#ffff00",
            Speed = 3.0,
            ParticleSize = 6.0,
            Turbulence = 2.0,
            EmberRatio = 0.5,
            BackgroundColor = "#1a0000",
            Opacity = 0.8,
            TargetFps = 30
        };

        config.ParticleCount.Should().Be(400);
        config.FlameColor.Should().Be("#ff2200");
        config.EmberColor.Should().Be("#ffff00");
        config.Speed.Should().Be(3.0);
        config.ParticleSize.Should().Be(6.0);
        config.Turbulence.Should().Be(2.0);
        config.EmberRatio.Should().Be(0.5);
        config.BackgroundColor.Should().Be("#1a0000");
        config.Opacity.Should().Be(0.8);
        config.TargetFps.Should().Be(30);
    }

    [Fact]
    public void ConfigRecords_WithSameValues_ShouldBeEqual()
    {
        var config1 = new FireEmbersConfig { FlameColor = "#ff6600", Speed = 2.0 };
        var config2 = new FireEmbersConfig { FlameColor = "#ff6600", Speed = 2.0 };

        config1.Should().Be(config2);
        config1.GetHashCode().Should().Be(config2.GetHashCode());
    }

    [Fact]
    public void ConfigRecords_WithDifferentValues_ShouldNotBeEqual()
    {
        var config1 = new FireEmbersConfig { FlameColor = "#ff6600" };
        var config2 = new FireEmbersConfig { FlameColor = "#3b82f6" };

        config1.Should().NotBe(config2);
    }
}
