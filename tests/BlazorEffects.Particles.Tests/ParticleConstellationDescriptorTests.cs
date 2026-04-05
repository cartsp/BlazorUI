using BlazorEffects.Particles;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.Particles.Tests;

public class ParticleConstellationDescriptorTests
{
    private readonly ParticleConstellationDescriptor _sut = new();

    [Fact]
    public void ComponentType_ShouldBeParticleConstellation()
    {
        _sut.ComponentType.Should().Be(typeof(ParticleConstellation));
    }

    [Fact]
    public void EffectName_ShouldBeParticleConstellation()
    {
        _sut.EffectName.Should().Be("Particle Constellation");
    }

    [Fact]
    public void GetParameterDefinitions_ShouldReturnAllParameters()
    {
        var parameters = _sut.GetParameterDefinitions();

        parameters.Should().HaveCount(10);
        parameters.Should().Contain(p => p.PropertyName == nameof(ParticleConstellationConfig.ParticleCount));
        parameters.Should().Contain(p => p.PropertyName == nameof(ParticleConstellationConfig.ParticleColor));
        parameters.Should().Contain(p => p.PropertyName == nameof(ParticleConstellationConfig.ParticleSize));
        parameters.Should().Contain(p => p.PropertyName == nameof(ParticleConstellationConfig.ConnectionDistance));
        parameters.Should().Contain(p => p.PropertyName == nameof(ParticleConstellationConfig.ConnectionColor));
        parameters.Should().Contain(p => p.PropertyName == nameof(ParticleConstellationConfig.Speed));
        parameters.Should().Contain(p => p.PropertyName == nameof(ParticleConstellationConfig.MouseInteraction));
        parameters.Should().Contain(p => p.PropertyName == nameof(ParticleConstellationConfig.MouseRadius));
        parameters.Should().Contain(p => p.PropertyName == nameof(ParticleConstellationConfig.MouseForce));
        parameters.Should().Contain(p => p.PropertyName == nameof(ParticleConstellationConfig.Opacity));
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
        presets.Select(p => p.Name).Should().Contain(["Starfield", "Cyberpunk", "Deep Space", "Amber", "Dense"]);
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
        var config = new ParticleConstellationConfig();
        var result = _sut.ApplyParameter(config, nameof(ParticleConstellationConfig.ParticleCount), 300);

        result.ParticleCount.Should().Be(300);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateParticleColor()
    {
        var config = new ParticleConstellationConfig();
        var result = _sut.ApplyParameter(config, nameof(ParticleConstellationConfig.ParticleColor), "#ff0000");

        result.ParticleColor.Should().Be("#ff0000");
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateMouseInteraction()
    {
        var config = new ParticleConstellationConfig();
        var result = _sut.ApplyParameter(config, nameof(ParticleConstellationConfig.MouseInteraction), false);

        result.MouseInteraction.Should().BeFalse();
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateSpeed()
    {
        var config = new ParticleConstellationConfig();
        var result = _sut.ApplyParameter(config, nameof(ParticleConstellationConfig.Speed), 1.5);

        result.Speed.Should().Be(1.5);
    }

    [Fact]
    public void ApplyParameter_WithUnknownProperty_ShouldReturnUnchanged()
    {
        var config = new ParticleConstellationConfig();
        var result = _sut.ApplyParameter(config, "UnknownProperty", "value");

        result.Should().Be(config);
    }

    [Fact]
    public void ApplyParameter_WithNullParticleColor_ShouldUseDefault()
    {
        var config = new ParticleConstellationConfig();
        var result = _sut.ApplyParameter(config, nameof(ParticleConstellationConfig.ParticleColor), null);

        result.ParticleColor.Should().Be("#6366f1");
    }
}
