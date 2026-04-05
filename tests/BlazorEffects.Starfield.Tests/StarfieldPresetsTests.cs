using BlazorEffects.Starfield;
using AwesomeAssertions;

namespace BlazorEffects.Starfield.Tests;

public class StarfieldPresetsTests
{
    [Fact]
    public void Default_ShouldHaveExpectedValues()
    {
        var preset = StarfieldPresets.Default;

        preset.StarCount.Should().Be(800);
        preset.StarColor.Should().Be("#ffffff");
        preset.Speed.Should().Be(2.0);
        preset.TrailLength.Should().Be(0.6);
        preset.Depth.Should().Be(1000);
        preset.BackgroundColor.Should().Be("#000000");
    }

    [Fact]
    public void WarpDrive_ShouldHaveBlueStarsAndHighSpeed()
    {
        var preset = StarfieldPresets.WarpDrive;

        preset.StarColor.Should().Be("#60a5fa");
        preset.StarCount.Should().Be(1200);
        preset.Speed.Should().Be(4.0);
        preset.TrailLength.Should().Be(0.8);
    }

    [Fact]
    public void GoldenDrift_ShouldHaveWarmColorsAndSlowSpeed()
    {
        var preset = StarfieldPresets.GoldenDrift;

        preset.StarColor.Should().Be("#fbbf24");
        preset.Speed.Should().Be(0.8);
        preset.StarCount.Should().Be(400);
    }

    [Fact]
    public void Sparse_ShouldHaveFewStars()
    {
        var preset = StarfieldPresets.Sparse;

        preset.StarCount.Should().Be(150);
        preset.Speed.Should().Be(1.0);
        preset.StarSize.Should().Be(3.0);
    }

    [Fact]
    public void Blizzard_ShouldBeDenseAndFast()
    {
        var preset = StarfieldPresets.Blizzard;

        preset.StarCount.Should().Be(2000);
        preset.Speed.Should().Be(5.0);
        preset.TrailLength.Should().Be(0.9);
    }

    [Fact]
    public void RetroTerminal_ShouldHaveGreenStars()
    {
        var preset = StarfieldPresets.RetroTerminal;

        preset.StarColor.Should().Be("#00ff41");
        preset.StarCount.Should().Be(600);
        preset.BackgroundColor.Should().Be("#0a0a0a");
    }

    [Fact]
    public void AllPresets_ShouldHaveValidTargetFps()
    {
        var presets = new[]
        {
            StarfieldPresets.Default,
            StarfieldPresets.WarpDrive,
            StarfieldPresets.GoldenDrift,
            StarfieldPresets.Sparse,
            StarfieldPresets.Blizzard,
            StarfieldPresets.RetroTerminal
        };

        foreach (var preset in presets)
        {
            preset.TargetFps.Should().BeGreaterThan(0);
            preset.TargetFps.Should().BeLessThanOrEqualTo(120);
        }
    }
}
