using BlazorEffects.Starfield;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.Starfield.Tests;

public class StarfieldDescriptorTests
{
    private readonly StarfieldDescriptor _sut = new();

    [Fact]
    public void ComponentType_ShouldBeStarfield()
    {
        _sut.ComponentType.Should().Be(typeof(Starfield));
    }

    [Fact]
    public void EffectName_ShouldBeStarfield()
    {
        _sut.EffectName.Should().Be("Starfield");
    }

    [Fact]
    public void GetParameterDefinitions_ShouldReturnAllParameters()
    {
        var parameters = _sut.GetParameterDefinitions();

        parameters.Should().HaveCount(8);
        parameters.Should().Contain(p => p.PropertyName == nameof(StarfieldConfig.StarCount));
        parameters.Should().Contain(p => p.PropertyName == nameof(StarfieldConfig.StarColor));
        parameters.Should().Contain(p => p.PropertyName == nameof(StarfieldConfig.StarSize));
        parameters.Should().Contain(p => p.PropertyName == nameof(StarfieldConfig.Speed));
        parameters.Should().Contain(p => p.PropertyName == nameof(StarfieldConfig.TrailLength));
        parameters.Should().Contain(p => p.PropertyName == nameof(StarfieldConfig.Depth));
        parameters.Should().Contain(p => p.PropertyName == nameof(StarfieldConfig.BackgroundColor));
        parameters.Should().Contain(p => p.PropertyName == nameof(StarfieldConfig.Opacity));
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
        presets.Select(p => p.Name).Should().Contain(["Hyperspace", "Warp Drive", "Golden Drift", "Sparse", "Blizzard", "Retro Terminal"]);
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
    public void ApplyParameter_ShouldUpdateStarCount()
    {
        var config = new StarfieldConfig();
        var result = _sut.ApplyParameter(config, nameof(StarfieldConfig.StarCount), 1500);

        result.StarCount.Should().Be(1500);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateStarColor()
    {
        var config = new StarfieldConfig();
        var result = _sut.ApplyParameter(config, nameof(StarfieldConfig.StarColor), "#ff0000");

        result.StarColor.Should().Be("#ff0000");
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateSpeed()
    {
        var config = new StarfieldConfig();
        var result = _sut.ApplyParameter(config, nameof(StarfieldConfig.Speed), 5.0);

        result.Speed.Should().Be(5.0);
    }

    [Fact]
    public void ApplyParameter_ShouldUpdateTrailLength()
    {
        var config = new StarfieldConfig();
        var result = _sut.ApplyParameter(config, nameof(StarfieldConfig.TrailLength), 0.9);

        result.TrailLength.Should().Be(0.9);
    }

    [Fact]
    public void ApplyParameter_WithUnknownProperty_ShouldReturnUnchanged()
    {
        var config = new StarfieldConfig();
        var result = _sut.ApplyParameter(config, "UnknownProperty", "value");

        result.Should().Be(config);
    }

    [Fact]
    public void ApplyParameter_WithNullStarColor_ShouldUseDefault()
    {
        var config = new StarfieldConfig();
        var result = _sut.ApplyParameter(config, nameof(StarfieldConfig.StarColor), null);

        result.StarColor.Should().Be("#ffffff");
    }
}
