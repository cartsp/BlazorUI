using BlazorEffects.Ripple;
using AwesomeAssertions;

namespace BlazorEffects.Ripple.Tests;

public class RipplePresetsTests
{
    [Fact]
    public void Default_ShouldHaveExpectedValues()
    {
        var preset = RipplePresets.Default;

        preset.MaxRipples.Should().Be(20);
        preset.Speed.Should().Be(3.0);
        preset.Color.Should().Be("#60a5fa");
        preset.Trigger.Should().Be("auto");
        preset.BackgroundColor.Should().Be("#0f172a");
    }

    [Fact]
    public void NeonElectric_ShouldHaveCyanColorAndHighSpeed()
    {
        var preset = RipplePresets.NeonElectric;

        preset.Color.Should().Be("#22d3ee");
        preset.Speed.Should().Be(4.0);
        preset.LineWidth.Should().Be(3.0);
        preset.MaxRadius.Should().Be(400);
    }

    [Fact]
    public void Sunset_ShouldHaveWarmColorsAndSlowSpeed()
    {
        var preset = RipplePresets.Sunset;

        preset.Color.Should().Be("#fb923c");
        preset.Speed.Should().Be(2.0);
        preset.MaxRadius.Should().Be(250);
    }

    [Fact]
    public void Minimal_ShouldHaveSubtleSettings()
    {
        var preset = RipplePresets.Minimal;

        preset.Color.Should().Be("#e2e8f0");
        preset.Speed.Should().Be(1.5);
        preset.LineWidth.Should().Be(1.0);
        preset.AutoRippleCount.Should().Be(3);
    }

    [Fact]
    public void RainDrops_ShouldBeFastAndFrequent()
    {
        var preset = RipplePresets.RainDrops;

        preset.Color.Should().Be("#4ade80");
        preset.AutoRippleCount.Should().Be(12);
        preset.AutoInterval.Should().Be(300);
    }

    [Fact]
    public void Interactive_ShouldUseClickTrigger()
    {
        var preset = RipplePresets.Interactive;

        preset.Trigger.Should().Be("click");
        preset.Color.Should().Be("#a78bfa");
        preset.AutoRippleCount.Should().Be(0);
    }

    [Fact]
    public void AllPresets_ShouldHaveValidTargetFps()
    {
        var presets = new[]
        {
            RipplePresets.Default,
            RipplePresets.NeonElectric,
            RipplePresets.Sunset,
            RipplePresets.Minimal,
            RipplePresets.RainDrops,
            RipplePresets.Interactive
        };

        foreach (var preset in presets)
        {
            preset.TargetFps.Should().BeGreaterThan(0);
            preset.TargetFps.Should().BeLessThanOrEqualTo(120);
        }
    }
}
