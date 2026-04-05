using BlazorEffects.Aurora;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.Aurora.Tests;

public class AuroraBorealisConfigTests
{
    [Fact]
    public void DefaultConfig_ShouldHaveExpectedValues()
    {
        var config = new AuroraBorealisConfig();

        config.Colors.Should().BeEquivalentTo(["#00ff87", "#7b2ff7", "#00b4d8"]);
        config.RibbonCount.Should().Be(4);
        config.Amplitude.Should().Be(120);
        config.Speed.Should().Be(0.008);
        config.Opacity.Should().Be(0.5);
        config.BlendMode.Should().Be("screen");
        config.TargetFps.Should().Be(60);
    }

    [Fact]
    public void Config_ShouldImplementIEffectConfig()
    {
        var config = new AuroraBorealisConfig();
        config.Should().BeAssignableTo<IEffectConfig>();
    }

    [Fact]
    public void Config_WithCustomValues_ShouldStoreCorrectly()
    {
        var config = new AuroraBorealisConfig
        {
            Colors = ["#ff0000", "#00ff00"],
            RibbonCount = 6,
            Amplitude = 200,
            Speed = 0.02,
            Opacity = 0.8,
            BlendMode = "lighter",
            TargetFps = 30
        };

        config.Colors.Should().BeEquivalentTo(["#ff0000", "#00ff00"]);
        config.RibbonCount.Should().Be(6);
        config.Amplitude.Should().Be(200);
        config.Speed.Should().Be(0.02);
        config.Opacity.Should().Be(0.8);
        config.BlendMode.Should().Be("lighter");
        config.TargetFps.Should().Be(30);
    }

    [Fact]
    public void ConfigRecords_WithSameValues_ShouldHaveMatchingProperties()
    {
        var config1 = new AuroraBorealisConfig { RibbonCount = 5, Speed = 0.01 };
        var config2 = new AuroraBorealisConfig { RibbonCount = 5, Speed = 0.01 };

        config1.RibbonCount.Should().Be(config2.RibbonCount);
        config1.Speed.Should().Be(config2.Speed);
        config1.Amplitude.Should().Be(config2.Amplitude);
        config1.Opacity.Should().Be(config2.Opacity);
        config1.BlendMode.Should().Be(config2.BlendMode);
    }

    [Fact]
    public void ConfigRecords_WithDifferentValues_ShouldNotBeEqual()
    {
        var config1 = new AuroraBorealisConfig { RibbonCount = 4 };
        var config2 = new AuroraBorealisConfig { RibbonCount = 6 };

        config1.Should().NotBe(config2);
    }

    [Fact]
    public void Config_WithSameColorArrays_ShouldMatchElementWise()
    {
        var colors1 = new[] { "#ff0000", "#00ff00" };
        var colors2 = new[] { "#ff0000", "#00ff00" };
        var config1 = new AuroraBorealisConfig { Colors = colors1 };
        var config2 = new AuroraBorealisConfig { Colors = colors2 };

        config1.Colors.Should().BeEquivalentTo(config2.Colors);
    }
}
