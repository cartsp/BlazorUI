using BlazorEffects.MatrixRain;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.MatrixRain.Tests;

public class MatrixRainDescriptorTests
{
    private readonly MatrixRainDescriptor _sut = new();

    [Fact]
    public void ComponentType_ShouldBeMatrixRain()
    {
        _sut.ComponentType.Should().Be(typeof(MatrixRain));
    }

    [Fact]
    public void EffectName_ShouldBeMatrixRain()
    {
        _sut.EffectName.Should().Be("Matrix Rain");
    }

    [Fact]
    public void GetParameterDefinitions_ShouldReturnAllParameters()
    {
        var parameters = _sut.GetParameterDefinitions();

        parameters.Should().HaveCount(8);
        parameters.Should().Contain(p => p.PropertyName == nameof(MatrixRainConfig.Characters));
        parameters.Should().Contain(p => p.PropertyName == nameof(MatrixRainConfig.FontSize));
        parameters.Should().Contain(p => p.PropertyName == nameof(MatrixRainConfig.FontFamily));
        parameters.Should().Contain(p => p.PropertyName == nameof(MatrixRainConfig.Color));
        parameters.Should().Contain(p => p.PropertyName == nameof(MatrixRainConfig.FadeColor));
        parameters.Should().Contain(p => p.PropertyName == nameof(MatrixRainConfig.Speed));
        parameters.Should().Contain(p => p.PropertyName == nameof(MatrixRainConfig.Density));
        parameters.Should().Contain(p => p.PropertyName == nameof(MatrixRainConfig.Opacity));
    }

    [Fact]
    public void GetParameterDefinitions_ShouldHaveOrderedParameters()
    {
        var parameters = _sut.GetParameterDefinitions();

        parameters.Select(p => p.Order).Should().BeInAscendingOrder();
    }

    [Fact]
    public void GetParameterDefinitions_RangeParameters_ShouldHaveMinMaxStep()
    {
        var parameters = _sut.GetParameterDefinitions();
        var rangeParams = parameters.Where(p => p.Type == EffectParameterType.Range).ToList();

        rangeParams.Should().NotBeEmpty();
        foreach (var p in rangeParams)
        {
            p.MinValue.Should().NotBeNull();
            p.MaxValue.Should().NotBeNull();
            p.Step.Should().NotBeNull();
        }
    }

    [Fact]
    public void GetPresets_ShouldReturnFivePresets()
    {
        var presets = _sut.GetPresets();

        presets.Should().HaveCount(5);
        presets.Select(p => p.Name).Should().Contain(["Classic", "Cyberpunk", "Terminal", "Ghost", "Sunset"]);
    }

    [Fact]
    public void GetPresets_AllPresetsShouldHaveNameAndDescription()
    {
        var presets = _sut.GetPresets();

        foreach (var preset in presets)
        {
            preset.Name.Should().NotBeNullOrEmpty();
            preset.Description.Should().NotBeNullOrEmpty();
            preset.Config.Should().NotBeNull();
        }
    }

    [Fact]
    public void GetPresets_AllPresetsShouldHavePreviewGradient()
    {
        var presets = _sut.GetPresets();

        foreach (var preset in presets)
        {
            preset.PreviewGradient.Should().NotBeNullOrEmpty();
        }
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateColor()
    {
        var config = new MatrixRainConfig();
        var result = _sut.ApplyParameter(config, nameof(MatrixRainConfig.Color), "#ff0000");

        result.Color.Should().Be("#ff0000");
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateSpeed()
    {
        var config = new MatrixRainConfig();
        var result = _sut.ApplyParameter(config, nameof(MatrixRainConfig.Speed), 2.5);

        result.Speed.Should().Be(2.5);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateCharacters()
    {
        var config = new MatrixRainConfig();
        var result = _sut.ApplyParameter(config, nameof(MatrixRainConfig.Characters), MatrixRainPresets.Katakana);

        result.Characters.Should().Be(MatrixRainPresets.Katakana);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateFontSize()
    {
        var config = new MatrixRainConfig();
        var result = _sut.ApplyParameter(config, nameof(MatrixRainConfig.FontSize), 20.0);

        result.FontSize.Should().Be(20.0);
    }

    [Fact]
    public void ApplyParameter_WithUnknownProperty_ShouldReturnUnchanged()
    {
        var config = new MatrixRainConfig();
        var result = _sut.ApplyParameter(config, "UnknownProperty", "value");

        result.Should().Be(config);
    }

    [Fact]
    public void ApplyParameter_WithNullColor_ShouldUseDefault()
    {
        var config = new MatrixRainConfig();
        var result = _sut.ApplyParameter(config, nameof(MatrixRainConfig.Color), null);

        result.Color.Should().Be("#00ff41");
    }
}
