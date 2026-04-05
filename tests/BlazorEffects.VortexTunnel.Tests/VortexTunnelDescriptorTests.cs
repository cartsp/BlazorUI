using BlazorEffects.VortexTunnel;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.VortexTunnel.Tests;

public class VortexTunnelDescriptorTests
{
    private readonly VortexTunnelDescriptor _sut = new();

    [Fact]
    public void ComponentType_ShouldBeVortexTunnel()
    {
        _sut.ComponentType.Should().Be(typeof(VortexTunnel));
    }

    [Fact]
    public void EffectName_ShouldBeVortexTunnel()
    {
        _sut.EffectName.Should().Be("VortexTunnel");
    }

    [Fact]
    public void GetParameterDefinitions_ShouldReturnAllParameters()
    {
        var parameters = _sut.GetParameterDefinitions();

        parameters.Should().HaveCount(9);
        parameters.Should().Contain(p => p.PropertyName == nameof(VortexTunnelConfig.RingCount));
        parameters.Should().Contain(p => p.PropertyName == nameof(VortexTunnelConfig.RotationSpeed));
        parameters.Should().Contain(p => p.PropertyName == nameof(VortexTunnelConfig.Color));
        parameters.Should().Contain(p => p.PropertyName == nameof(VortexTunnelConfig.ScaleFactor));
        parameters.Should().Contain(p => p.PropertyName == nameof(VortexTunnelConfig.Shape));
        parameters.Should().Contain(p => p.PropertyName == nameof(VortexTunnelConfig.PolygonSides));
        parameters.Should().Contain(p => p.PropertyName == nameof(VortexTunnelConfig.LineWidth));
        parameters.Should().Contain(p => p.PropertyName == nameof(VortexTunnelConfig.BackgroundColor));
        parameters.Should().Contain(p => p.PropertyName == nameof(VortexTunnelConfig.Opacity));
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
            ["Purple Vortex", "Hypno Wheel", "Deep Space", "Cyber Grid", "Warm Portal", "Ice Crystal"]);
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
    public void ApplyParameter_ShouldUpdateRingCount()
    {
        var config = new VortexTunnelConfig();
        var result = _sut.ApplyParameter(config, nameof(VortexTunnelConfig.RingCount), 25);

        result.RingCount.Should().Be(25);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateColor()
    {
        var config = new VortexTunnelConfig();
        var result = _sut.ApplyParameter(config, nameof(VortexTunnelConfig.Color), "#ff0000");

        result.Color.Should().Be("#ff0000");
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateRotationSpeed()
    {
        var config = new VortexTunnelConfig();
        var result = _sut.ApplyParameter(config, nameof(VortexTunnelConfig.RotationSpeed), 0.05);

        result.RotationSpeed.Should().Be(0.05);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateShape()
    {
        var config = new VortexTunnelConfig();
        var result = _sut.ApplyParameter(config, nameof(VortexTunnelConfig.Shape), "polygon");

        result.Shape.Should().Be("polygon");
    }

    [Fact]
    public void ApplyParameter_WithUnknownProperty_ShouldReturnUnchanged()
    {
        var config = new VortexTunnelConfig();
        var result = _sut.ApplyParameter(config, "UnknownProperty", "value");

        result.Should().Be(config);
    }

    [Fact]
    public void ApplyParameter_WithNullColor_ShouldUseDefault()
    {
        var config = new VortexTunnelConfig();
        var result = _sut.ApplyParameter(config, nameof(VortexTunnelConfig.Color), null);

        result.Color.Should().Be("#8b5cf6");
    }

    [Fact]
    public void ApplyParameter_WithNullShape_ShouldUseDefault()
    {
        var config = new VortexTunnelConfig();
        var result = _sut.ApplyParameter(config, nameof(VortexTunnelConfig.Shape), null);

        result.Shape.Should().Be("circle");
    }
}
