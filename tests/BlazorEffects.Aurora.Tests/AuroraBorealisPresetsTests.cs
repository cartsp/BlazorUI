using BlazorEffects.Aurora;
using AwesomeAssertions;

namespace BlazorEffects.Aurora.Tests;

public class AuroraBorealisPresetsTests
{
    [Fact]
    public void Classic_ShouldReturnExpectedColors()
    {
        var colors = AuroraBorealisPresets.Classic;

        colors.Should().BeEquivalentTo(["#00ff87", "#7b2ff7", "#00b4d8"]);
    }

    [Fact]
    public void Arctic_ShouldReturnExpectedColors()
    {
        var colors = AuroraBorealisPresets.Arctic;

        colors.Should().HaveCount(4);
        colors.Should().BeEquivalentTo(["#00b4d8", "#0077b6", "#90e0ef", "#48cae4"]);
    }

    [Fact]
    public void Sunset_ShouldReturnExpectedColors()
    {
        var colors = AuroraBorealisPresets.Sunset;

        colors.Should().HaveCount(4);
        colors.Should().BeEquivalentTo(["#ff006e", "#fb5607", "#ff006e", "#8338ec"]);
    }

    [Fact]
    public void Emerald_ShouldReturnExpectedColors()
    {
        var colors = AuroraBorealisPresets.Emerald;

        colors.Should().HaveCount(4);
        colors.Should().BeEquivalentTo(["#00ff87", "#38b000", "#70e000", "#008000"]);
    }

    [Fact]
    public void Cosmic_ShouldReturnExpectedColors()
    {
        var colors = AuroraBorealisPresets.Cosmic;

        colors.Should().HaveCount(4);
        colors.Should().BeEquivalentTo(["#7b2ff7", "#c77dff", "#9d4edd", "#e040fb"]);
    }

    [Theory]
    [InlineData("Classic")]
    [InlineData("Arctic")]
    [InlineData("Sunset")]
    [InlineData("Emerald")]
    [InlineData("Cosmic")]
    public void AllPresets_ShouldReturnNonNullArray(string presetName)
    {
        var colors = presetName switch
        {
            "Classic" => AuroraBorealisPresets.Classic,
            "Arctic" => AuroraBorealisPresets.Arctic,
            "Sunset" => AuroraBorealisPresets.Sunset,
            "Emerald" => AuroraBorealisPresets.Emerald,
            "Cosmic" => AuroraBorealisPresets.Cosmic,
            _ => throw new ArgumentException($"Unknown preset: {presetName}")
        };

        colors.Should().NotBeNull();
        colors.Should().NotBeEmpty();
        colors.Should().OnlyContain(c => c.StartsWith('#') && c.Length == 7);
    }
}
