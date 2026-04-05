using BlazorEffects.Noise;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.Noise.Tests;

public class NoiseFieldDescriptorTests
{
    private readonly NoiseFieldDescriptor _sut = new();

    [Fact]
    public void ComponentType_ShouldBeNoiseField()
    {
        _sut.ComponentType.Should().Be(typeof(NoiseField));
    }

    [Fact]
    public void EffectName_ShouldBeNoiseField()
    {
        _sut.EffectName.Should().Be("Noise Field");
    }

    [Fact]
    public void GetParameterDefinitions_ShouldReturnAllParameters()
    {
        var parameters = _sut.GetParameterDefinitions();

        parameters.Should().HaveCount(8);
        parameters.Should().Contain(p => p.PropertyName == nameof(NoiseFieldConfig.ColorStops));
        parameters.Should().Contain(p => p.PropertyName == nameof(NoiseFieldConfig.NoiseScale));
        parameters.Should().Contain(p => p.PropertyName == nameof(NoiseFieldConfig.Speed));
        parameters.Should().Contain(p => p.PropertyName == nameof(NoiseFieldConfig.Octaves));
        parameters.Should().Contain(p => p.PropertyName == nameof(NoiseFieldConfig.Persistence));
        parameters.Should().Contain(p => p.PropertyName == nameof(NoiseFieldConfig.Lacunarity));
        parameters.Should().Contain(p => p.PropertyName == nameof(NoiseFieldConfig.Brightness));
        parameters.Should().Contain(p => p.PropertyName == nameof(NoiseFieldConfig.Opacity));
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
    public void GetPresets_ShouldReturnAllPresets()
    {
        var presets = _sut.GetPresets();

        presets.Should().HaveCount(7);
        presets.Select(p => p.Name).Should().Contain(["Default", "Aurora", "Sunset", "Ocean", "Lava", "Monochrome", "Neon"]);
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
    public void ApplyParameter_ShouldUpdateNoiseScale()
    {
        var config = new NoiseFieldConfig();
        var result = _sut.ApplyParameter(config, nameof(NoiseFieldConfig.NoiseScale), 0.01);

        result.NoiseScale.Should().Be(0.01);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateOctaves()
    {
        var config = new NoiseFieldConfig();
        var result = _sut.ApplyParameter(config, nameof(NoiseFieldConfig.Octaves), 5);

        result.Octaves.Should().Be(5);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateColorStops()
    {
        var config = new NoiseFieldConfig();
        var newColors = new[] { "#000000", "#ffffff", "#000000" };
        var result = _sut.ApplyParameter(config, nameof(NoiseFieldConfig.ColorStops), newColors);

        result.ColorStops.Should().BeEquivalentTo(newColors);
    }

    [Fact]
    public void ApplyParameter_WithUnknownProperty_ShouldReturnUnchanged()
    {
        var config = new NoiseFieldConfig();
        var result = _sut.ApplyParameter(config, "UnknownProperty", "value");

        result.Should().Be(config);
    }

    [Fact]
    public void ApplyParameter_WithNullColorStops_ShouldUseDefault()
    {
        var config = new NoiseFieldConfig();
        var result = _sut.ApplyParameter(config, nameof(NoiseFieldConfig.ColorStops), null);

        result.ColorStops.Should().BeEquivalentTo(NoiseFieldPresets.Default);
    }
}
