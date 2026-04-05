using BlazorEffects.Particles;
using AwesomeAssertions;

namespace BlazorEffects.Particles.Tests;

public class ParticleConstellationPresetsTests
{
    [Fact]
    public void Default_ShouldHaveExpectedValues()
    {
        var preset = ParticleConstellationPresets.Default;

        preset.ParticleCount.Should().Be(150);
        preset.ParticleColor.Should().Be("#6366f1");
        preset.ParticleSize.Should().Be(2);
        preset.ConnectionDistance.Should().Be(120);
        preset.Speed.Should().Be(0.5);
        preset.MouseInteraction.Should().BeTrue();
        preset.Opacity.Should().Be(0.6);
    }

    [Fact]
    public void Cyberpunk_ShouldHaveGreenParticles()
    {
        var preset = ParticleConstellationPresets.Cyberpunk;

        preset.ParticleColor.Should().Be("#00ff41");
        preset.ConnectionColor.Should().Be("#00ff41");
        preset.Speed.Should().Be(0.8);
    }

    [Fact]
    public void DeepSpace_ShouldHaveMoreParticles()
    {
        var preset = ParticleConstellationPresets.DeepSpace;

        preset.ParticleCount.Should().Be(200);
        preset.ParticleColor.Should().Be("#38bdf8");
        preset.Speed.Should().Be(0.3);
    }

    [Fact]
    public void Amber_ShouldHaveWarmColors()
    {
        var preset = ParticleConstellationPresets.Amber;

        preset.ParticleColor.Should().Be("#f59e0b");
        preset.ConnectionColor.Should().Be("#ea580c");
        preset.ParticleSize.Should().Be(2.5);
    }

    [Fact]
    public void Minimal_ShouldBeSparse()
    {
        var preset = ParticleConstellationPresets.Minimal;

        preset.ParticleCount.Should().Be(50);
        preset.Opacity.Should().Be(0.3);
        preset.Speed.Should().Be(0.2);
    }

    [Fact]
    public void Dense_ShouldHaveManyParticles()
    {
        var preset = ParticleConstellationPresets.Dense;

        preset.ParticleCount.Should().Be(300);
        preset.ConnectionDistance.Should().Be(80);
    }

    [Fact]
    public void AllPresets_ShouldHaveValidTargetFps()
    {
        var presets = new[]
        {
            ParticleConstellationPresets.Default,
            ParticleConstellationPresets.Cyberpunk,
            ParticleConstellationPresets.DeepSpace,
            ParticleConstellationPresets.Amber,
            ParticleConstellationPresets.Minimal,
            ParticleConstellationPresets.Dense
        };

        foreach (var preset in presets)
        {
            preset.TargetFps.Should().BeGreaterThan(0);
            preset.TargetFps.Should().BeLessThanOrEqualTo(120);
        }
    }
}
