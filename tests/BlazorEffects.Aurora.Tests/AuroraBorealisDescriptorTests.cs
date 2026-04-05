using BlazorEffects.Aurora;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.Aurora.Tests;

public class AuroraBorealisDescriptorTests
{
    private readonly AuroraBorealisDescriptor _sut = new();

    [Fact]
    public void ComponentType_ShouldBeAuroraBorealis()
    {
        _sut.ComponentType.Should().Be(typeof(AuroraBorealis));
    }

    [Fact]
    public void EffectName_ShouldBeAuroraBorealis()
    {
        _sut.EffectName.Should().Be("Aurora Borealis");
    }

    [Fact]
    public void GetParameterDefinitions_ShouldReturnAllParameters()
    {
        var parameters = _sut.GetParameterDefinitions();

        parameters.Should().HaveCount(6);
        parameters.Should().Contain(p => p.PropertyName == nameof(AuroraBorealisConfig.Colors));
        parameters.Should().Contain(p => p.PropertyName == nameof(AuroraBorealisConfig.RibbonCount));
        parameters.Should().Contain(p => p.PropertyName == nameof(AuroraBorealisConfig.Amplitude));
        parameters.Should().Contain(p => p.PropertyName == nameof(AuroraBorealisConfig.Speed));
        parameters.Should().Contain(p => p.PropertyName == nameof(AuroraBorealisConfig.Opacity));
        parameters.Should().Contain(p => p.PropertyName == nameof(AuroraBorealisConfig.BlendMode));
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
        presets.Select(p => p.Name).Should().Contain(["Northern Lights", "Arctic Dawn", "Deep Space", "Emerald", "Twilight"]);
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
    public void ApplyParameter_ShouldUpdateAmplitude()
    {
        var config = new AuroraBorealisConfig();
        var result = _sut.ApplyParameter(config, nameof(AuroraBorealisConfig.Amplitude), 200.0);

        result.Amplitude.Should().Be(200.0);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateRibbonCount()
    {
        var config = new AuroraBorealisConfig();
        var result = _sut.ApplyParameter(config, nameof(AuroraBorealisConfig.RibbonCount), 6);

        result.RibbonCount.Should().Be(6);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateColors()
    {
        var config = new AuroraBorealisConfig();
        var newColors = new[] { "#ff0000", "#00ff00", "#0000ff" };
        var result = _sut.ApplyParameter(config, nameof(AuroraBorealisConfig.Colors), newColors);

        result.Colors.Should().BeEquivalentTo(newColors);
    }

    [Fact]
    public void ApplyParameter_WithUnknownProperty_ShouldReturnUnchanged()
    {
        var config = new AuroraBorealisConfig();
        var result = _sut.ApplyParameter(config, "UnknownProperty", "value");

        result.Should().Be(config);
    }

    [Fact]
    public void ApplyParameter_WithNullColors_ShouldUseDefault()
    {
        var config = new AuroraBorealisConfig();
        var result = _sut.ApplyParameter(config, nameof(AuroraBorealisConfig.Colors), null);

        result.Colors.Should().BeEquivalentTo(AuroraBorealisPresets.Classic);
    }
}
