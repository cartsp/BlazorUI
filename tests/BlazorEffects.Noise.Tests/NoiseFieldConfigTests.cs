using BlazorEffects.Noise;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.Noise.Tests;

public class NoiseFieldConfigTests
{
    [Fact]
    public void DefaultConfig_ShouldHaveExpectedValues()
    {
        var config = new NoiseFieldConfig();

        config.ColorStops.Should().BeEquivalentTo(["#0a0a2e", "#1e1b4b", "#6366f1", "#8b5cf6", "#c084fc", "#ec4899", "#f43f5e", "#1e1b4b", "#0a0a2e"]);
        config.NoiseScale.Should().Be(0.003);
        config.Speed.Should().Be(0.005);
        config.Octaves.Should().Be(4);
        config.Persistence.Should().Be(0.5);
        config.Lacunarity.Should().Be(2.0);
        config.Brightness.Should().Be(1.0);
        config.Opacity.Should().Be(0.85);
        config.TargetFps.Should().Be(60);
    }

    [Fact]
    public void Config_ShouldImplementIEffectConfig()
    {
        var config = new NoiseFieldConfig();
        config.Should().BeAssignableTo<IEffectConfig>();
    }

    [Fact]
    public void Config_WithCustomValues_ShouldStoreCorrectly()
    {
        var config = new NoiseFieldConfig
        {
            ColorStops = ["#000000", "#ffffff", "#000000"],
            NoiseScale = 0.01,
            Speed = 0.005,
            Octaves = 5,
            Persistence = 0.6,
            Lacunarity = 2.5,
            Brightness = 1.5,
            Opacity = 0.5,
            TargetFps = 30
        };

        config.ColorStops.Should().BeEquivalentTo(["#000000", "#ffffff", "#000000"]);
        config.NoiseScale.Should().Be(0.01);
        config.Speed.Should().Be(0.005);
        config.Octaves.Should().Be(5);
        config.Persistence.Should().Be(0.6);
        config.Lacunarity.Should().Be(2.5);
        config.Brightness.Should().Be(1.5);
        config.Opacity.Should().Be(0.5);
        config.TargetFps.Should().Be(30);
    }

    [Fact]
    public void ConfigRecords_WithSameValues_ShouldMatch()
    {
        var config1 = new NoiseFieldConfig { Octaves = 4, Speed = 0.005 };
        var config2 = new NoiseFieldConfig { Octaves = 4, Speed = 0.005 };

        config1.Octaves.Should().Be(config2.Octaves);
        config1.Speed.Should().Be(config2.Speed);
        config1.NoiseScale.Should().Be(config2.NoiseScale);
        config1.Opacity.Should().Be(config2.Opacity);
    }

    [Fact]
    public void ConfigRecords_WithDifferentOctaves_ShouldHaveDifferentValues()
    {
        var config1 = new NoiseFieldConfig { Octaves = 3 };
        var config2 = new NoiseFieldConfig { Octaves = 6 };

        config1.Octaves.Should().NotBe(config2.Octaves);
    }

    [Fact]
    public void Config_WithSameColorArrays_ShouldMatchElementWise()
    {
        var colors1 = new[] { "#ff0000", "#00ff00" };
        var colors2 = new[] { "#ff0000", "#00ff00" };
        var config1 = new NoiseFieldConfig { ColorStops = colors1 };
        var config2 = new NoiseFieldConfig { ColorStops = colors2 };

        config1.ColorStops.Should().BeEquivalentTo(config2.ColorStops);
    }

    [Fact]
    public void Config_PersistenceShouldBeValidRange()
    {
        var config = new NoiseFieldConfig { Persistence = 0.7 };

        config.Persistence.Should().BeGreaterThan(0);
        config.Persistence.Should().BeLessThanOrEqualTo(1);
    }

    [Fact]
    public void Config_LacunarityShouldBeGreaterThanOne()
    {
        var config = new NoiseFieldConfig { Lacunarity = 2.0 };

        config.Lacunarity.Should().BeGreaterThanOrEqualTo(1);
    }
}
