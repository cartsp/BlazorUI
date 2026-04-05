using BlazorEffects.Noise;
using AwesomeAssertions;

namespace BlazorEffects.Noise.Tests;

public class NoiseFieldPresetsTests
{
    [Fact]
    public void Default_ShouldReturnExpectedColors()
    {
        var colors = NoiseFieldPresets.Default;

        colors.Should().BeEquivalentTo(["#0a0a2e", "#1e1b4b", "#6366f1", "#8b5cf6", "#c084fc", "#ec4899", "#f43f5e", "#1e1b4b", "#0a0a2e"]);
    }

    [Fact]
    public void Aurora_ShouldReturnExpectedColors()
    {
        var colors = NoiseFieldPresets.Aurora;

        colors.Should().HaveCount(9);
        colors.Should().BeEquivalentTo(["#020617", "#0c1445", "#0f4c3a", "#10b981", "#14b8a6", "#22d3ee", "#8b5cf6", "#0c1445", "#020617"]);
    }

    [Fact]
    public void Sunset_ShouldReturnExpectedColors()
    {
        var colors = NoiseFieldPresets.Sunset;

        colors.Should().HaveCount(9);
        colors.Should().BeEquivalentTo(["#1a0a2e", "#4c1d95", "#f97316", "#fb923c", "#f43f5e", "#e879f9", "#a855f7", "#4c1d95", "#1a0a2e"]);
    }

    [Fact]
    public void Ocean_ShouldReturnExpectedColors()
    {
        var colors = NoiseFieldPresets.Ocean;

        colors.Should().HaveCount(10);
        colors.Should().BeEquivalentTo(["#020617", "#0a1628", "#1e3a5f", "#1e40af", "#0d9488", "#06b6d4", "#22d3ee", "#67e8f9", "#0a1628", "#020617"]);
    }

    [Fact]
    public void Lava_ShouldReturnExpectedColors()
    {
        var colors = NoiseFieldPresets.Lava;

        colors.Should().HaveCount(10);
        colors.Should().BeEquivalentTo(["#0a0000", "#1c0a00", "#7f1d1d", "#991b1b", "#dc2626", "#f97316", "#facc15", "#991b1b", "#1c0a00", "#0a0000"]);
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

        colors.Should().HaveCount(10);
        colors.Should().BeEquivalentTo(["#05051a", "#0a0a2e", "#ec4899", "#f472b6", "#06b6d4", "#22d3ee", "#a855f7", "#c084fc", "#0a0a2e", "#05051a"]);
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
