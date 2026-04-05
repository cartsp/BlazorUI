using BlazorEffects.Noise;
using AwesomeAssertions;

namespace BlazorEffects.Noise.Tests;

public class NoiseFieldPresetsTests
{
    [Fact]
    public void Default_ShouldReturnExpectedColors()
    {
        var colors = NoiseFieldPresets.Default;

        colors.Should().BeEquivalentTo(["#0f172a", "#6366f1", "#a855f7", "#ec4899", "#0f172a"]);
    }

    [Fact]
    public void Aurora_ShouldReturnExpectedColors()
    {
        var colors = NoiseFieldPresets.Aurora;

        colors.Should().HaveCount(6);
        colors.Should().BeEquivalentTo(["#0c1445", "#1e3a5f", "#10b981", "#14b8a6", "#8b5cf6", "#0c1445"]);
    }

    [Fact]
    public void Sunset_ShouldReturnExpectedColors()
    {
        var colors = NoiseFieldPresets.Sunset;

        colors.Should().HaveCount(5);
        colors.Should().BeEquivalentTo(["#1a0a2e", "#f97316", "#f43f5e", "#a855f7", "#1a0a2e"]);
    }

    [Fact]
    public void Ocean_ShouldReturnExpectedColors()
    {
        var colors = NoiseFieldPresets.Ocean;

        colors.Should().HaveCount(6);
        colors.Should().BeEquivalentTo(["#0a1628", "#1e40af", "#0d9488", "#06b6d4", "#67e8f9", "#0a1628"]);
    }

    [Fact]
    public void Lava_ShouldReturnExpectedColors()
    {
        var colors = NoiseFieldPresets.Lava;

        colors.Should().HaveCount(6);
        colors.Should().BeEquivalentTo(["#1c0a00", "#991b1b", "#dc2626", "#f97316", "#facc15", "#1c0a00"]);
    }

    [Fact]
    public void Monochrome_ShouldReturnExpectedColors()
    {
        var colors = NoiseFieldPresets.Monochrome;

        colors.Should().HaveCount(8);
    }

    [Fact]
    public void Neon_ShouldReturnExpectedColors()
    {
        var colors = NoiseFieldPresets.Neon;

        colors.Should().HaveCount(5);
        colors.Should().BeEquivalentTo(["#0a0a1a", "#ec4899", "#06b6d4", "#a855f7", "#0a0a1a"]);
    }

    [Theory]
    [InlineData("Default")]
    [InlineData("Aurora")]
    [InlineData("Sunset")]
    [InlineData("Ocean")]
    [InlineData("Lava")]
    [InlineData("Monochrome")]
    [InlineData("Neon")]
    public void AllPresets_ShouldReturnNonNullArray(string presetName)
    {
        var colors = presetName switch
        {
            "Default" => NoiseFieldPresets.Default,
            "Aurora" => NoiseFieldPresets.Aurora,
            "Sunset" => NoiseFieldPresets.Sunset,
            "Ocean" => NoiseFieldPresets.Ocean,
            "Lava" => NoiseFieldPresets.Lava,
            "Monochrome" => NoiseFieldPresets.Monochrome,
            "Neon" => NoiseFieldPresets.Neon,
            _ => throw new ArgumentException($"Unknown preset: {presetName}")
        };

        colors.Should().NotBeNull();
        colors.Should().NotBeEmpty();
        colors.Should().OnlyContain(c => c.StartsWith('#') && c.Length == 7);
    }

    [Fact]
    public void AllPresets_ShouldHaveAtLeastTwoColorStops()
    {
        var allPresets = new[]
        {
            NoiseFieldPresets.Default,
            NoiseFieldPresets.Aurora,
            NoiseFieldPresets.Sunset,
            NoiseFieldPresets.Ocean,
            NoiseFieldPresets.Lava,
            NoiseFieldPresets.Monochrome,
            NoiseFieldPresets.Neon
        };

        foreach (var preset in allPresets)
        {
            preset.Length.Should().BeGreaterThanOrEqualTo(2);
        }
    }
}
