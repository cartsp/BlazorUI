using BlazorEffects.Blobs;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.Blobs.Tests;

public class MorphingBlobsDescriptorTests
{
    private readonly MorphingBlobsDescriptor _sut = new();

    [Fact]
    public void ComponentType_ShouldBeMorphingBlobs()
    {
        _sut.ComponentType.Should().Be(typeof(MorphingBlobs));
    }

    [Fact]
    public void EffectName_ShouldBeMorphingBlobs()
    {
        _sut.EffectName.Should().Be("Morphing Blobs");
    }

    [Fact]
    public void GetParameterDefinitions_ShouldReturnAllParameters()
    {
        var parameters = _sut.GetParameterDefinitions();

        parameters.Should().HaveCount(7);
        parameters.Should().Contain(p => p.PropertyName == nameof(MorphingBlobsConfig.BlobCount));
        parameters.Should().Contain(p => p.PropertyName == nameof(MorphingBlobsConfig.Colors));
        parameters.Should().Contain(p => p.PropertyName == nameof(MorphingBlobsConfig.BlobSize));
        parameters.Should().Contain(p => p.PropertyName == nameof(MorphingBlobsConfig.Speed));
        parameters.Should().Contain(p => p.PropertyName == nameof(MorphingBlobsConfig.MorphIntensity));
        parameters.Should().Contain(p => p.PropertyName == nameof(MorphingBlobsConfig.BlendMode));
        parameters.Should().Contain(p => p.PropertyName == nameof(MorphingBlobsConfig.Opacity));
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
        presets.Select(p => p.Name).Should().Contain(["Lava Lamp", "Ocean", "Neon", "Dawn", "Deep"]);
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
    public void ApplyParameter_ShouldUpdateBlobCount()
    {
        var config = new MorphingBlobsConfig();
        var result = _sut.ApplyParameter(config, nameof(MorphingBlobsConfig.BlobCount), 6);

        result.BlobCount.Should().Be(6);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateBlobSize()
    {
        var config = new MorphingBlobsConfig();
        var result = _sut.ApplyParameter(config, nameof(MorphingBlobsConfig.BlobSize), 400.0);

        result.BlobSize.Should().Be(400.0);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateColors()
    {
        var config = new MorphingBlobsConfig();
        var newColors = new[] { "#ff0000", "#00ff00", "#0000ff" };
        var result = _sut.ApplyParameter(config, nameof(MorphingBlobsConfig.Colors), newColors);

        result.Colors.Should().BeEquivalentTo(newColors);
    }

    [Fact]
    public void ApplyParameter_WithUnknownProperty_ShouldReturnUnchanged()
    {
        var config = new MorphingBlobsConfig();
        var result = _sut.ApplyParameter(config, "UnknownProperty", "value");

        result.Should().Be(config);
    }

    [Fact]
    public void ApplyParameter_WithNullColors_ShouldUseDefault()
    {
        var config = new MorphingBlobsConfig();
        var result = _sut.ApplyParameter(config, nameof(MorphingBlobsConfig.Colors), null);

        result.Colors.Should().BeEquivalentTo(MorphingBlobsPresets.Default);
    }
}
