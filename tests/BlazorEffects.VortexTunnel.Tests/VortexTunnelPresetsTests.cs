using BlazorEffects.VortexTunnel;
using AwesomeAssertions;

namespace BlazorEffects.VortexTunnel.Tests;

public class VortexTunnelPresetsTests
{
    [Fact]
    public void Default_ShouldHaveExpectedValues()
    {
        var preset = VortexTunnelPresets.Default;

        preset.RingCount.Should().Be(20);
        preset.RotationSpeed.Should().Be(0.02);
        preset.Color.Should().Be("#8b5cf6");
        preset.Shape.Should().Be("circle");
        preset.BackgroundColor.Should().Be("#030712");
    }

    [Fact]
    public void HypnoWheel_ShouldHaveMultiColorsAndPolygonShape()
    {
        var preset = VortexTunnelPresets.HypnoWheel;

        preset.Colors.Should().HaveCount(6);
        preset.Shape.Should().Be("polygon");
        preset.RingCount.Should().Be(30);
        preset.RotationSpeed.Should().Be(0.04);
    }

    [Fact]
    public void DeepSpace_ShouldBeSubtleAndBlue()
    {
        var preset = VortexTunnelPresets.DeepSpace;

        preset.Color.Should().Be("#1e40af");
        preset.RotationSpeed.Should().Be(0.01);
        preset.RingCount.Should().Be(25);
    }

    [Fact]
    public void CyberGrid_ShouldUseSquareShape()
    {
        var preset = VortexTunnelPresets.CyberGrid;

        preset.Shape.Should().Be("square");
        preset.Color.Should().Be("#00ff41");
        preset.ScaleFactor.Should().Be(0.88);
    }

    [Fact]
    public void WarmPortal_ShouldHaveWarmColors()
    {
        var preset = VortexTunnelPresets.WarmPortal;

        preset.Colors.Should().HaveCount(3);
        preset.Shape.Should().Be("polygon");
        preset.PolygonSides.Should().Be(8);
    }

    [Fact]
    public void IceCrystal_ShouldBeBlueAndHexagonal()
    {
        var preset = VortexTunnelPresets.IceCrystal;

        preset.Color.Should().Be("#67e8f9");
        preset.Shape.Should().Be("polygon");
        preset.PolygonSides.Should().Be(6);
    }

    [Fact]
    public void AllPresets_ShouldHaveValidTargetFps()
    {
        var presets = new[]
        {
            VortexTunnelPresets.Default,
            VortexTunnelPresets.HypnoWheel,
            VortexTunnelPresets.DeepSpace,
            VortexTunnelPresets.CyberGrid,
            VortexTunnelPresets.WarmPortal,
            VortexTunnelPresets.IceCrystal
        };

        foreach (var preset in presets)
        {
            preset.TargetFps.Should().BeGreaterThan(0);
            preset.TargetFps.Should().BeLessThanOrEqualTo(120);
        }
    }
}
