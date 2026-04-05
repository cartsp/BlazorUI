using BlazorEffects.Blobs;
using AwesomeAssertions;

namespace BlazorEffects.Blobs.Tests;

public class MorphingBlobsPresetsTests
{
    [Fact]
    public void Default_ShouldReturnExpectedColors()
    {
        var colors = MorphingBlobsPresets.Default;

        colors.Should().BeEquivalentTo(["#6366f1", "#ec4899", "#f97316", "#06b6d4"]);
    }

    [Fact]
    public void Sunset_ShouldReturnExpectedColors()
    {
        var colors = MorphingBlobsPresets.Sunset;

        colors.Should().HaveCount(4);
        colors.Should().BeEquivalentTo(["#7c3aed", "#f43f5e", "#f59e0b", "#ea580c"]);
    }

    [Fact]
    public void Ocean_ShouldReturnExpectedColors()
    {
        var colors = MorphingBlobsPresets.Ocean;

        colors.Should().HaveCount(4);
        colors.Should().BeEquivalentTo(["#1e40af", "#0d9488", "#06b6d4", "#38bdf8"]);
    }

    [Fact]
    public void Aurora_ShouldReturnExpectedColors()
    {
        var colors = MorphingBlobsPresets.Aurora;

        colors.Should().HaveCount(4);
        colors.Should().BeEquivalentTo(["#10b981", "#14b8a6", "#8b5cf6", "#3b82f6"]);
    }

    [Fact]
    public void Neon_ShouldReturnExpectedColors()
    {
        var colors = MorphingBlobsPresets.Neon;

        colors.Should().HaveCount(4);
        colors.Should().BeEquivalentTo(["#ec4899", "#3b82f6", "#84cc16", "#a855f7"]);
    }

    [Fact]
    public void Pastel_ShouldReturnExpectedColors()
    {
        var colors = MorphingBlobsPresets.Pastel;

        colors.Should().HaveCount(4);
        colors.Should().BeEquivalentTo(["#f9a8d4", "#c4b5fd", "#fdba74", "#6ee7b7"]);
    }

    [Fact]
    public void Monochrome_ShouldReturnExpectedColors()
    {
        var colors = MorphingBlobsPresets.Monochrome;

        colors.Should().HaveCount(4);
        colors.Should().BeEquivalentTo(["#64748b", "#475569", "#94a3b8", "#334155"]);
    }

    [Theory]
    [InlineData("Default")]
    [InlineData("Sunset")]
    [InlineData("Ocean")]
    [InlineData("Aurora")]
    [InlineData("Neon")]
    [InlineData("Pastel")]
    [InlineData("Monochrome")]
    public void AllPresets_ShouldReturnNonNullArray(string presetName)
    {
        var colors = presetName switch
        {
            "Default" => MorphingBlobsPresets.Default,
            "Sunset" => MorphingBlobsPresets.Sunset,
            "Ocean" => MorphingBlobsPresets.Ocean,
            "Aurora" => MorphingBlobsPresets.Aurora,
            "Neon" => MorphingBlobsPresets.Neon,
            "Pastel" => MorphingBlobsPresets.Pastel,
            "Monochrome" => MorphingBlobsPresets.Monochrome,
            _ => throw new ArgumentException($"Unknown preset: {presetName}")
        };

        colors.Should().NotBeNull();
        colors.Should().NotBeEmpty();
        colors.Should().OnlyContain(c => c.StartsWith('#') && c.Length == 7);
    }
}
