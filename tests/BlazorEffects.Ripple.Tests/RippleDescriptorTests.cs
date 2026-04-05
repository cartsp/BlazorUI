using BlazorEffects.Ripple;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.Ripple.Tests;

public class RippleDescriptorTests
{
    private readonly RippleDescriptor _sut = new();

    [Fact]
    public void ComponentType_ShouldBeRipple()
    {
        _sut.ComponentType.Should().Be(typeof(Ripple));
    }

    [Fact]
    public void EffectName_ShouldBeRipple()
    {
        _sut.EffectName.Should().Be("Ripple");
    }

    [Fact]
    public void GetParameterDefinitions_ShouldReturnAllParameters()
    {
        var parameters = _sut.GetParameterDefinitions();

        parameters.Should().HaveCount(8);
        parameters.Should().Contain(p => p.PropertyName == nameof(RippleConfig.MaxRipples));
        parameters.Should().Contain(p => p.PropertyName == nameof(RippleConfig.MaxRadius));
        parameters.Should().Contain(p => p.PropertyName == nameof(RippleConfig.Speed));
        parameters.Should().Contain(p => p.PropertyName == nameof(RippleConfig.Color));
        parameters.Should().Contain(p => p.PropertyName == nameof(RippleConfig.LineWidth));
        parameters.Should().Contain(p => p.PropertyName == nameof(RippleConfig.Decay));
        parameters.Should().Contain(p => p.PropertyName == nameof(RippleConfig.BackgroundColor));
        parameters.Should().Contain(p => p.PropertyName == nameof(RippleConfig.Opacity));
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
        presets.Select(p => p.Name).Should().Contain(
            ["Water Surface", "Neon Electric", "Sunset", "Minimal", "Rain Drops", "Interactive"]);
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
    public void ApplyParameter_ShouldUpdateMaxRipples()
    {
        var config = new RippleConfig();
        var result = _sut.ApplyParameter(config, nameof(RippleConfig.MaxRipples), 30);

        result.MaxRipples.Should().Be(30);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateColor()
    {
        var config = new RippleConfig();
        var result = _sut.ApplyParameter(config, nameof(RippleConfig.Color), "#ff0000");

        result.Color.Should().Be("#ff0000");
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateSpeed()
    {
        var config = new RippleConfig();
        var result = _sut.ApplyParameter(config, nameof(RippleConfig.Speed), 5.0);

        result.Speed.Should().Be(5.0);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateDecay()
    {
        var config = new RippleConfig();
        var result = _sut.ApplyParameter(config, nameof(RippleConfig.Decay), 0.05);

        result.Decay.Should().Be(0.05);
    }

    [Fact]
    public void ApplyParameter_WithUnknownProperty_ShouldReturnUnchanged()
    {
        var config = new RippleConfig();
        var result = _sut.ApplyParameter(config, "UnknownProperty", "value");

        result.Should().Be(config);
    }

    [Fact]
    public void ApplyParameter_WithNullColor_ShouldUseDefault()
    {
        var config = new RippleConfig();
        var result = _sut.ApplyParameter(config, nameof(RippleConfig.Color), null);

        result.Color.Should().Be("#60a5fa");
    }
}
