using BlazorEffects.GradientWaves;
using AwesomeAssertions;

namespace BlazorEffects.GradientWaves.Tests;

public class GradientWavesPresetsTests
{
    [Fact]
    public void Stripe_ShouldReturnExpectedColors()
    {
        var colors = GradientWavesPresets.Stripe;

        colors.Should().BeEquivalentTo(["#635bff", "#00d4ff", "#ff6b9d", "#a855f7", "#06b6d4", "#f472b6"]);
    }

    [Fact]
    public void VibrantSunset_ShouldReturnExpectedColors()
    {
        var colors = GradientWavesPresets.VibrantSunset;

        colors.Should().HaveCount(6);
        colors.Should().BeEquivalentTo(["#ff6b35", "#ff2d87", "#ffd23f", "#ff7eb3", "#c850c0", "#ff9a3c"]);
    }

    [Fact]
    public void OceanDeep_ShouldReturnExpectedColors()
    {
        var colors = GradientWavesPresets.OceanDeep;

        colors.Should().HaveCount(6);
        colors.Should().BeEquivalentTo(["#0a2463", "#1e6091", "#168aad", "#34d399", "#76e4f7", "#06b6d4"]);
    }

    [Fact]
    public void NorthernLights_ShouldReturnExpectedColors()
    {
        var colors = GradientWavesPresets.NorthernLights;

        colors.Should().HaveCount(6);
        colors.Should().BeEquivalentTo(["#10b981", "#8b5cf6", "#4f46e5", "#14b8a6", "#22d3ee", "#d946ef"]);
    }

    [Fact]
    public void MinimalMono_ShouldReturnExpectedColors()
    {
        var colors = GradientWavesPresets.MinimalMono;

        colors.Should().HaveCount(6);
        colors.Should().BeEquivalentTo(["#e5e5e5", "#a3a3a3", "#737373", "#d4d4d4", "#525252", "#f5f5f5"]);
    }

    [Fact]
    public void Cyberpunk_ShouldReturnExpectedColors()
    {
        var colors = GradientWavesPresets.Cyberpunk;

        colors.Should().HaveCount(6);
        colors.Should().BeEquivalentTo(["#39ff14", "#ff00ff", "#00e5ff", "#ffe600", "#ff1493", "#7b2ff7"]);
    }

    [Fact]
    public void RoseGarden_ShouldReturnExpectedColors()
    {
        var colors = GradientWavesPresets.RoseGarden;

        colors.Should().HaveCount(6);
        colors.Should().BeEquivalentTo(["#fb7185", "#f43f5e", "#fecdd3", "#be185d", "#fda4af", "#e11d48"]);
    }

    [Fact]
    public void Forest_ShouldReturnExpectedColors()
    {
        var colors = GradientWavesPresets.Forest;

        colors.Should().HaveCount(6);
        colors.Should().BeEquivalentTo(["#064e3b", "#059669", "#34d399", "#166534", "#6ee7b7", "#10b981"]);
    }

    [Theory]
    [InlineData("Stripe")]
    [InlineData("VibrantSunset")]
    [InlineData("OceanDeep")]
    [InlineData("NorthernLights")]
    [InlineData("MinimalMono")]
    [InlineData("Cyberpunk")]
    [InlineData("RoseGarden")]
    [InlineData("Forest")]
    public void AllPresets_ShouldReturnNonNullArray(string presetName)
    {
        var colors = presetName switch
        {
            "Stripe" => GradientWavesPresets.Stripe,
            "VibrantSunset" => GradientWavesPresets.VibrantSunset,
            "OceanDeep" => GradientWavesPresets.OceanDeep,
            "NorthernLights" => GradientWavesPresets.NorthernLights,
            "MinimalMono" => GradientWavesPresets.MinimalMono,
            "Cyberpunk" => GradientWavesPresets.Cyberpunk,
            "RoseGarden" => GradientWavesPresets.RoseGarden,
            "Forest" => GradientWavesPresets.Forest,
            _ => throw new ArgumentException($"Unknown preset: {presetName}")
        };

        colors.Should().NotBeNull();
        colors.Should().NotBeEmpty();
        colors.Should().OnlyContain(c => c.StartsWith('#') && c.Length == 7);
    }
}
