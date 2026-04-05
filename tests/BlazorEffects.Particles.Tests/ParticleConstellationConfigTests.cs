using BlazorEffects.Particles;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.Particles.Tests;

public class ParticleConstellationConfigTests
{
    [Fact]
    public void DefaultConfig_ShouldHaveExpectedValues()
    {
        var config = new ParticleConstellationConfig();

        config.ParticleCount.Should().Be(150);
        config.ParticleColor.Should().Be("#6366f1");
        config.ParticleSize.Should().Be(2);
        config.ConnectionDistance.Should().Be(120);
        config.ConnectionColor.Should().Be("#6366f1");
        config.Speed.Should().Be(0.5);
        config.MouseInteraction.Should().BeTrue();
        config.MouseRadius.Should().Be(150);
        config.MouseForce.Should().Be("attract");
        config.Opacity.Should().Be(0.6);
        config.TargetFps.Should().Be(60);
    }

    [Fact]
    public void Config_ShouldImplementIEffectConfig()
    {
        var config = new ParticleConstellationConfig();
        config.Should().BeAssignableTo<IEffectConfig>();
    }

    [Fact]
    public void Config_WithCustomValues_ShouldStoreCorrectly()
    {
        var config = new ParticleConstellationConfig
        {
            ParticleCount = 300,
            ParticleColor = "#ff0000",
            ParticleSize = 3,
            ConnectionDistance = 200,
            ConnectionColor = "#00ff00",
            Speed = 1.5,
            MouseInteraction = false,
            MouseRadius = 200,
            MouseForce = "repel",
            Opacity = 0.9,
            TargetFps = 30
        };

        config.ParticleCount.Should().Be(300);
        config.ParticleColor.Should().Be("#ff0000");
        config.ParticleSize.Should().Be(3);
        config.ConnectionDistance.Should().Be(200);
        config.ConnectionColor.Should().Be("#00ff00");
        config.Speed.Should().Be(1.5);
        config.MouseInteraction.Should().BeFalse();
        config.MouseRadius.Should().Be(200);
        config.MouseForce.Should().Be("repel");
        config.Opacity.Should().Be(0.9);
        config.TargetFps.Should().Be(30);
    }

    [Fact]
    public void ConfigRecords_WithSameValues_ShouldBeEqual()
    {
        var config1 = new ParticleConstellationConfig { ParticleColor = "#6366f1", Speed = 1.5 };
        var config2 = new ParticleConstellationConfig { ParticleColor = "#6366f1", Speed = 1.5 };

        config1.Should().Be(config2);
        config1.GetHashCode().Should().Be(config2.GetHashCode());
    }

    [Fact]
    public void ConfigRecords_WithDifferentValues_ShouldNotBeEqual()
    {
        var config1 = new ParticleConstellationConfig { ParticleColor = "#6366f1" };
        var config2 = new ParticleConstellationConfig { ParticleColor = "#ff0000" };

        config1.Should().NotBe(config2);
    }
}
