using BlazorEffects.MatrixRain;
using Bunit;
using AwesomeAssertions;

namespace BlazorEffects.MatrixRain.Tests;

public class MatrixRainPresetsTests
{
    [Fact]
    public void Classic_ShouldContainExpectedCharacters()
    {
        MatrixRainPresets.Classic.Should().Be("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#$%&");
    }

    [Fact]
    public void Katakana_ShouldNotBeEmpty()
    {
        MatrixRainPresets.Katakana.Should().NotBeEmpty();
        MatrixRainPresets.Katakana.Should().Contain("ア");
    }

    [Fact]
    public void Binary_ShouldContainOnly0And1()
    {
        MatrixRainPresets.Binary.Should().Be("01");
    }

    [Fact]
    public void Hex_ShouldContainHexCharacters()
    {
        MatrixRainPresets.Hex.Should().Be("0123456789ABCDEF");
    }

    [Fact]
    public void Emojis_ShouldNotBeEmpty()
    {
        MatrixRainPresets.Emojis.Should().NotBeEmpty();
    }
}
