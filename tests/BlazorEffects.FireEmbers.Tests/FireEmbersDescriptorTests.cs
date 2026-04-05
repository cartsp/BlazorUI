using BlazorEffects.FireEmbers;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.FireEmbers.Tests;

public class FireEmbersDescriptorTests
{
    private readonly FireEmbersDescriptor _sut = new();

    [Fact]
    public void ComponentType_ShouldBeFireEmbers()
    {
        _sut.ComponentType.Should().Be(typeof(FireEmbers));
    }

    [Fact]
    public void EffectName_ShouldBeFireAndEmbers()
    {
        _sut.EffectName.Should().Be("Fire & Embers");
    }

    [Fact]
    public void GetParameterDefinitions_ShouldReturnAllParameters()
    {
        var parameters = _sut.GetParameterDefinitions();

        parameters.Should().HaveCount(9);
        parameters.Should().Contain(p => p.PropertyName == nameof(FireEmbersConfig.ParticleCount));
        parameters.Should().Contain(p => p.PropertyName == nameof(FireEmbersConfig.FlameColor));
        parameters.Should().Contain(p => p.PropertyName == nameof(FireEmbersConfig.EmberColor));
        parameters.Should().Contain(p => p.PropertyName == nameof(FireEmbersConfig.Speed));
        parameters.Should().Contain(p => p.PropertyName == nameof(FireEmbersConfig.ParticleSize));
        parameters.Should().Contain(p => p.PropertyName == nameof(FireEmbersConfig.Turbulence));
        parameters.Should().Contain(p => p.PropertyName == nameof(FireEmbersConfig.EmberRatio));
        parameters.Should().Contain(p => p.PropertyName == nameof(FireEmbersConfig.BackgroundColor));
        parameters.Should().Contain(p => p.PropertyName == nameof(FireEmbersConfig.Opacity));
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
    public void GetPresets_ShouldReturnSixPresets()
    {
        var presets = _sut.GetPresets();

        presets.Should().HaveCount(6);
        presets.Select(p => p.Name).Should().Contain(["Campfire", "Bonfire", "Candlelight", "Inferno", "Blue Flame", "Embers Only"]);
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
    public void ApplyParameter_ShouldUpdateParticleCount()
    {
        var config = new FireEmbersConfig();
        var result = _sut.ApplyParameter(config, nameof(FireEmbersConfig.ParticleCount), 400);

        result.ParticleCount.Should().Be(400);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateFlameColor()
    {
        var config = new FireEmbersConfig();
        var result = _sut.ApplyParameter(config, nameof(FireEmbersConfig.FlameColor), "#3b82f6");

        result.FlameColor.Should().Be("#3b82f6");
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateTurbulence()
    {
        var config = new FireEmbersConfig();
        var result = _sut.ApplyParameter(config, nameof(FireEmbersConfig.Turbulence), 2.5);

        result.Turbulence.Should().Be(2.5);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateEmberRatio()
    {
        var config = new FireEmbersConfig();
        var result = _sut.ApplyParameter(config, nameof(FireEmbersConfig.EmberRatio), 0.8);

        result.EmberRatio.Should().Be(0.8);
    }

    [Fact]
    public void ApplyParameter_WithUnknownProperty_ShouldReturnUnchanged()
    {
        var config = new FireEmbersConfig();
        var result = _sut.ApplyParameter(config, "UnknownProperty", "value");

        result.Should().Be(config);
    }

    [Fact]
    public void ApplyParameter_WithNullFlameColor_ShouldUseDefault()
    {
        var config = new FireEmbersConfig();
        var result = _sut.ApplyParameter(config, nameof(FireEmbersConfig.FlameColor), null);

        result.FlameColor.Should().Be("#ff6600");
    }
}
