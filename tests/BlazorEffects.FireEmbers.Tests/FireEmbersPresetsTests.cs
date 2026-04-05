using BlazorEffects.FireEmbers;
using AwesomeAssertions;

namespace BlazorEffects.FireEmbers.Tests;

public class FireEmbersPresetsTests
{
    [Fact]
    public void Default_ShouldHaveExpectedValues()
    {
        var preset = FireEmbersPresets.Default;

        preset.ParticleCount.Should().Be(200);
        preset.FlameColor.Should().Be("#ff6600");
        preset.EmberColor.Should().Be("#ffcc00");
        preset.Speed.Should().Be(1.5);
        preset.Opacity.Should().Be(0.9);
    }

    [Fact]
    public void Bonfire_ShouldHaveHighParticleCount()
    {
        var preset = FireEmbersPresets.Bonfire;

        preset.ParticleCount.Should().Be(400);
        preset.Speed.Should().Be(2.0);
        preset.Turbulence.Should().Be(1.5);
    }

    [Fact]
    public void Candlelight_ShouldBeGentle()
    {
        var preset = FireEmbersPresets.Candlelight;

        preset.ParticleCount.Should().Be(60);
        preset.Speed.Should().Be(0.5);
        preset.EmberRatio.Should().Be(0.15);
    }

    [Fact]
    public void Inferno_ShouldBeAggressive()
    {
        var preset = FireEmbersPresets.Inferno;

        preset.ParticleCount.Should().Be(500);
        preset.Speed.Should().Be(3.0);
        preset.EmberRatio.Should().Be(0.5);
        preset.BackgroundColor.Should().Be("#1a0000");
    }

    [Fact]
    public void BlueFlame_ShouldHaveBlueColors()
    {
        var preset = FireEmbersPresets.BlueFlame;

        preset.FlameColor.Should().Be("#3b82f6");
        preset.EmberColor.Should().Be("#93c5fd");
        preset.BackgroundColor.Should().Be("#050510");
    }

    [Fact]
    public void EmbersOnly_ShouldHaveMaxEmberRatio()
    {
        var preset = FireEmbersPresets.EmbersOnly;

        preset.EmberRatio.Should().Be(1.0);
        preset.ParticleCount.Should().Be(100);
    }

    [Fact]
    public void AllPresets_ShouldHaveValidTargetFps()
    {
        var presets = new[]
        {
            FireEmbersPresets.Default,
            FireEmbersPresets.Bonfire,
            FireEmbersPresets.Candlelight,
            FireEmbersPresets.Inferno,
            FireEmbersPresets.BlueFlame,
            FireEmbersPresets.EmbersOnly
        };

        foreach (var preset in presets)
        {
            preset.TargetFps.Should().BeGreaterThan(0);
            preset.TargetFps.Should().BeLessThanOrEqualTo(120);
        }
    }
}
