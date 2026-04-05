using BlazorEffects.VortexTunnel;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.VortexTunnel.Tests;

public class VortexTunnelConfigTests
{
    [Fact]
    public void DefaultConfig_ShouldHaveExpectedValues()
    {
        var config = new VortexTunnelConfig();

        config.RingCount.Should().Be(20);
        config.RotationSpeed.Should().Be(0.02);
        config.Color.Should().Be("#8b5cf6");
        config.Colors.Should().BeEmpty();
        config.ScaleFactor.Should().Be(0.92);
        config.Shape.Should().Be("circle");
        config.PolygonSides.Should().Be(6);
        config.LineWidth.Should().Be(2.0);
        config.BackgroundColor.Should().Be("#030712");
        config.Opacity.Should().Be(1.0);
        config.TargetFps.Should().Be(60);
    }

    [Fact]
    public void Config_ShouldImplementIEffectConfig()
    {
        var config = new VortexTunnelConfig();
        config.Should().BeAssignableTo<IEffectConfig>();
    }

    [Fact]
    public void Config_WithCustomValues_ShouldStoreCorrectly()
    {
        var config = new VortexTunnelConfig
        {
            RingCount = 25,
            RotationSpeed = 0.04,
            Color = "#22d3ee",
            Colors = ["#ef4444", "#3b82f6"],
            ScaleFactor = 0.88,
            Shape = "polygon",
            PolygonSides = 8,
            LineWidth = 3.0,
            BackgroundColor = "#000510",
            Opacity = 0.7,
            TargetFps = 30
        };

        config.RingCount.Should().Be(25);
        config.RotationSpeed.Should().Be(0.04);
        config.Color.Should().Be("#22d3ee");
        config.Colors.Should().HaveCount(2);
        config.ScaleFactor.Should().Be(0.88);
        config.Shape.Should().Be("polygon");
        config.PolygonSides.Should().Be(8);
        config.LineWidth.Should().Be(3.0);
        config.BackgroundColor.Should().Be("#000510");
        config.Opacity.Should().Be(0.7);
        config.TargetFps.Should().Be(30);
    }

    [Fact]
    public void ConfigRecords_WithSameValues_ShouldBeEqual()
    {
        var config1 = new VortexTunnelConfig { Color = "#8b5cf6", RingCount = 15 };
        var config2 = new VortexTunnelConfig { Color = "#8b5cf6", RingCount = 15 };

        config1.Should().Be(config2);
        config1.GetHashCode().Should().Be(config2.GetHashCode());
    }

    [Fact]
    public void ConfigRecords_WithDifferentValues_ShouldNotBeEqual()
    {
        var config1 = new VortexTunnelConfig { Color = "#8b5cf6" };
        var config2 = new VortexTunnelConfig { Color = "#22d3ee" };

        config1.Should().NotBe(config2);
    }

    [Fact]
    public void Config_WithColorsArray_ShouldStoreCorrectly()
    {
        var colors = new[] { "#ef4444", "#f97316", "#eab308" };
        var config = new VortexTunnelConfig { Colors = colors };

        config.Colors.Should().BeEquivalentTo(colors);
    }
}
