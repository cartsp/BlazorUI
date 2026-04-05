using BlazorEffects.GradientWaves;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.GradientWaves.Tests;

public class GradientWavesDescriptorTests
{
    private readonly GradientWavesDescriptor _sut = new();

    [Fact]
    public void ComponentType_ShouldBeGradientWaves()
    {
        _sut.ComponentType.Should().Be(typeof(GradientWaves));
    }

    [Fact]
    public void EffectName_ShouldBeGradientWaves()
    {
        _sut.EffectName.Should().Be("Gradient Waves");
    }

    [Fact]
    public void GetParameterDefinitions_ShouldReturnAllParameters()
    {
        var parameters = _sut.GetParameterDefinitions();

        parameters.Should().HaveCount(7);
        parameters.Should().Contain(p => p.PropertyName == nameof(GradientWavesConfig.Colors));
        parameters.Should().Contain(p => p.PropertyName == nameof(GradientWavesConfig.PointCount));
        parameters.Should().Contain(p => p.PropertyName == nameof(GradientWavesConfig.BlobSize));
        parameters.Should().Contain(p => p.PropertyName == nameof(GradientWavesConfig.Speed));
        parameters.Should().Contain(p => p.PropertyName == nameof(GradientWavesConfig.BlurAmount));
        parameters.Should().Contain(p => p.PropertyName == nameof(GradientWavesConfig.BlendMode));
        parameters.Should().Contain(p => p.PropertyName == nameof(GradientWavesConfig.Opacity));
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
    public void GetPresets_ShouldReturnEightPresets()
    {
        var presets = _sut.GetPresets();

        presets.Should().HaveCount(8);
        presets.Select(p => p.Name).Should().Contain(
        [
            "Stripe", "Vibrant Sunset", "Ocean Deep", "Northern Lights",
            "Cyberpunk", "Rose Garden", "Minimal Mono", "Forest"
        ]);
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
    public void ApplyParameter_ShouldUpdatePointCount()
    {
        var config = new GradientWavesConfig();
        var result = _sut.ApplyParameter(config, nameof(GradientWavesConfig.PointCount), 8);

        result.PointCount.Should().Be(8);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateBlobSize()
    {
        var config = new GradientWavesConfig();
        var result = _sut.ApplyParameter(config, nameof(GradientWavesConfig.BlobSize), 0.8);

        result.BlobSize.Should().Be(0.8);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateBlurAmount()
    {
        var config = new GradientWavesConfig();
        var result = _sut.ApplyParameter(config, nameof(GradientWavesConfig.BlurAmount), 120.0);

        result.BlurAmount.Should().Be(120.0);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateSpeed()
    {
        var config = new GradientWavesConfig();
        var result = _sut.ApplyParameter(config, nameof(GradientWavesConfig.Speed), 0.01);

        result.Speed.Should().Be(0.01);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateColors()
    {
        var config = new GradientWavesConfig();
        var newColors = new[] { "#ff0000", "#00ff00", "#0000ff" };
        var result = _sut.ApplyParameter(config, nameof(GradientWavesConfig.Colors), newColors);

        result.Colors.Should().BeEquivalentTo(newColors);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateBlendMode()
    {
        var config = new GradientWavesConfig();
        var result = _sut.ApplyParameter(config, nameof(GradientWavesConfig.BlendMode), "screen");

        result.BlendMode.Should().Be("screen");
    }

    [Fact]
    public void ApplyParameter_WithUnknownProperty_ShouldReturnUnchanged()
    {
        var config = new GradientWavesConfig();
        var result = _sut.ApplyParameter(config, "UnknownProperty", "value");

        result.Should().Be(config);
    }

    [Fact]
    public void ApplyParameter_WithNullColors_ShouldUseDefault()
    {
        var config = new GradientWavesConfig();
        var result = _sut.ApplyParameter(config, nameof(GradientWavesConfig.Colors), null);

        result.Colors.Should().BeEquivalentTo(GradientWavesPresets.Stripe);
    }
}
