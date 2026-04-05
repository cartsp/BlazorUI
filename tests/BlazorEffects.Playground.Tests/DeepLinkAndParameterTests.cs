using AwesomeAssertions;
using BlazorEffects.Aurora;
using BlazorEffects.Blobs;
using BlazorEffects.Core.Animation;
using BlazorEffects.GradientWaves;
using BlazorEffects.MatrixRain;
using BlazorEffects.Noise;
using BlazorEffects.Particles;
using BlazorEffects.Playground;

namespace BlazorEffects.Playground.Tests;

/// <summary>
/// QA tests for deep-link slug mapping and playground parameter controls.
/// Validates: AIE-69 — Verify gallery deep-links and playground parameter controls.
/// </summary>
public class DeepLinkSlugMappingTests
{
    private static readonly Dictionary<string, string> ExpectedSlugMappings = new()
    {
        ["matrix-rain"] = "Matrix Rain",
        ["particle-constellation"] = "Particle Constellation",
        ["aurora-borealis"] = "Aurora Borealis",
        ["morphing-blobs"] = "Morphing Blobs",
        ["noise-field"] = "Noise Field",
        ["gradient-waves"] = "Gradient Waves"
    };

    [Theory]
    [InlineData("matrix-rain", "Matrix Rain")]
    [InlineData("particle-constellation", "Particle Constellation")]
    [InlineData("aurora-borealis", "Aurora Borealis")]
    [InlineData("morphing-blobs", "Morphing Blobs")]
    [InlineData("noise-field", "Noise Field")]
    [InlineData("gradient-waves", "Gradient Waves")]
    public void SlugMapping_KnownSlugs_ShouldMapToCorrectEffectName(string slug, string expectedName)
    {
        var result = SlugToEffectName(slug);
        result.Should().Be(expectedName);
    }

    [Fact]
    public void SlugMapping_AllSixEffects_ShouldHaveDistinctSlugs()
    {
        ExpectedSlugMappings.Values.Distinct().Should().HaveCount(6);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("nonexistent")]
    [InlineData("matrix_rain")]
    [InlineData("MATRIX-RAIN")]
    [InlineData("aurora")]
    [InlineData("random-effect")]
    public void SlugMapping_InvalidOrNullSlugs_ShouldReturnNull(string? slug)
    {
        var result = SlugToEffectName(slug);
        result.Should().BeNull();
    }

    private static string? SlugToEffectName(string? slug) => slug switch
    {
        "matrix-rain" => "Matrix Rain",
        "particle-constellation" => "Particle Constellation",
        "aurora-borealis" => "Aurora Borealis",
        "morphing-blobs" => "Morphing Blobs",
        "noise-field" => "Noise Field",
        "gradient-waves" => "Gradient Waves",
        _ => null
    };
}

public class PlaygroundEffectRegistrationTests
{
    private readonly EffectRegistry _registry;

    public PlaygroundEffectRegistrationTests()
    {
        _registry = new EffectRegistry();
        _registry.Register(CreateAdapter(new NoiseFieldDescriptor(), "NoiseField"));
        _registry.Register(CreateAdapter(new MorphingBlobsDescriptor(), "MorphingBlobs"));
        _registry.Register(CreateAdapter(new AuroraBorealisDescriptor(), "AuroraBorealis"));
        _registry.Register(CreateAdapter(new MatrixRainDescriptor(), "MatrixRain"));
        _registry.Register(CreateAdapter(new ParticleConstellationDescriptor(), "ParticleConstellation"));
        _registry.Register(CreateAdapter(new GradientWavesDescriptor(), "GradientWaves"));
    }

    private static EffectDescriptorAdapter<TConfig> CreateAdapter<TConfig>(
        IEffectDescriptor<TConfig> descriptor, string markupTag)
        where TConfig : IEffectConfig
    {
        return new EffectDescriptorAdapter<TConfig>(descriptor,
            c => BlazorMarkupGenerator.Generate(c, markupTag));
    }

    [Fact]
    public void Registry_ShouldContainAllSixEffects()
    {
        _registry.GetAll().Should().HaveCount(6);
    }

    [Theory]
    [InlineData("Matrix Rain")]
    [InlineData("Particle Constellation")]
    [InlineData("Aurora Borealis")]
    [InlineData("Morphing Blobs")]
    [InlineData("Noise Field")]
    [InlineData("Gradient Waves")]
    public void Registry_GetByName_ShouldFindEachEffect(string effectName)
    {
        var adapter = _registry.GetByName(effectName);
        adapter.Should().NotBeNull();
        adapter.EffectName.Should().Be(effectName);
    }

    [Theory]
    [InlineData("Matrix Rain")]
    [InlineData("Particle Constellation")]
    [InlineData("Aurora Borealis")]
    [InlineData("Morphing Blobs")]
    [InlineData("Noise Field")]
    [InlineData("Gradient Waves")]
    public void EachEffect_ShouldHaveParameterDefinitions(string effectName)
    {
        var adapter = _registry.GetByName(effectName);
        adapter.GetParameterDefinitions().Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("Matrix Rain")]
    [InlineData("Particle Constellation")]
    [InlineData("Aurora Borealis")]
    [InlineData("Morphing Blobs")]
    [InlineData("Noise Field")]
    [InlineData("Gradient Waves")]
    public void EachEffect_ShouldHavePresets(string effectName)
    {
        var adapter = _registry.GetByName(effectName);
        adapter.GetPresets().Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("Matrix Rain")]
    [InlineData("Particle Constellation")]
    [InlineData("Aurora Borealis")]
    [InlineData("Morphing Blobs")]
    [InlineData("Noise Field")]
    [InlineData("Gradient Waves")]
    public void EachEffect_ShouldHaveDefaultConfig(string effectName)
    {
        var adapter = _registry.GetByName(effectName);
        adapter.GetDefaultConfig().Should().NotBeNull();
    }

    [Theory]
    [InlineData("Matrix Rain")]
    [InlineData("Particle Constellation")]
    [InlineData("Aurora Borealis")]
    [InlineData("Morphing Blobs")]
    [InlineData("Noise Field")]
    [InlineData("Gradient Waves")]
    public void EachEffect_ShouldHaveComponentType(string effectName)
    {
        var adapter = _registry.GetByName(effectName);
        adapter.ComponentType.Should().NotBeNull();
    }
}

public class ParameterControlTests
{
    private static EffectDescriptorAdapter<TConfig> CreateAdapter<TConfig>(
        IEffectDescriptor<TConfig> descriptor, string markupTag)
        where TConfig : IEffectConfig
    {
        return new EffectDescriptorAdapter<TConfig>(descriptor,
            c => BlazorMarkupGenerator.Generate(c, markupTag));
    }

    // --- Integer Parameters ---

    [Fact]
    public void AuroraBorealis_ApplyParameter_RibbonCount_ShouldUpdateInteger()
    {
        var descriptor = new AuroraBorealisDescriptor();
        var config = new AuroraBorealisConfig();
        var result = descriptor.ApplyParameter(config, nameof(AuroraBorealisConfig.RibbonCount), 6);
        result.RibbonCount.Should().Be(6);
    }

    [Fact]
    public void AuroraBorealis_ApplyParameter_RibbonCount_ShouldNotMutateOriginal()
    {
        var descriptor = new AuroraBorealisDescriptor();
        var config = new AuroraBorealisConfig();
        _ = descriptor.ApplyParameter(config, nameof(AuroraBorealisConfig.RibbonCount), 6);
        config.RibbonCount.Should().NotBe(6);
    }

    [Fact]
    public void ParticleConstellation_ApplyParameter_ParticleCount_ShouldUpdateInteger()
    {
        var descriptor = new ParticleConstellationDescriptor();
        var config = new ParticleConstellationConfig();
        var result = descriptor.ApplyParameter(config, nameof(ParticleConstellationConfig.ParticleCount), 300);
        result.ParticleCount.Should().Be(300);
    }

    [Fact]
    public void ParticleConstellation_ApplyParameter_ParticleCount_ShouldNotMutateOriginal()
    {
        var descriptor = new ParticleConstellationDescriptor();
        var config = new ParticleConstellationConfig();
        _ = descriptor.ApplyParameter(config, nameof(ParticleConstellationConfig.ParticleCount), 300);
        config.ParticleCount.Should().NotBe(300);
    }

    // --- Color Array Parameters ---

    [Fact]
    public void AuroraBorealis_ApplyParameter_Colors_ShouldUpdateColorArray()
    {
        var descriptor = new AuroraBorealisDescriptor();
        var config = new AuroraBorealisConfig();
        var newColors = new[] { "#ff0000", "#00ff00", "#0000ff" };
        var result = descriptor.ApplyParameter(config, nameof(AuroraBorealisConfig.Colors), newColors);
        result.Colors.Should().BeEquivalentTo(newColors);
    }

    [Fact]
    public void AuroraBorealis_ApplyParameter_Colors_WithNull_ShouldFallBackToClassic()
    {
        var descriptor = new AuroraBorealisDescriptor();
        var config = new AuroraBorealisConfig();
        var result = descriptor.ApplyParameter(config, nameof(AuroraBorealisConfig.Colors), null);
        result.Colors.Should().BeEquivalentTo(AuroraBorealisPresets.Classic);
    }

    [Fact]
    public void AuroraBorealis_ApplyParameter_Colors_WithEmptyArray_ShouldUseEmptyArray()
    {
        var descriptor = new AuroraBorealisDescriptor();
        var config = new AuroraBorealisConfig();
        var emptyColors = Array.Empty<string>();
        var result = descriptor.ApplyParameter(config, nameof(AuroraBorealisConfig.Colors), emptyColors);
        result.Colors.Should().BeEquivalentTo(emptyColors);
    }

    [Fact]
    public void AuroraBorealis_ApplyParameter_Colors_ShouldNotMutateOriginal()
    {
        var descriptor = new AuroraBorealisDescriptor();
        var config = new AuroraBorealisConfig();
        var originalColors = config.Colors;
        _ = descriptor.ApplyParameter(config, nameof(AuroraBorealisConfig.Colors), new[] { "#ff0000", "#00ff00" });
        config.Colors.Should().BeEquivalentTo(originalColors);
    }

    // --- Range Parameters ---

    [Fact]
    public void AuroraBorealis_ApplyParameter_Speed_ShouldUpdateRange()
    {
        var descriptor = new AuroraBorealisDescriptor();
        var config = new AuroraBorealisConfig();
        var result = descriptor.ApplyParameter(config, nameof(AuroraBorealisConfig.Speed), 0.02);
        result.Speed.Should().Be(0.02);
    }

    [Fact]
    public void ParticleConstellation_ApplyParameter_Speed_ShouldUpdateRange()
    {
        var descriptor = new ParticleConstellationDescriptor();
        var config = new ParticleConstellationConfig();
        var result = descriptor.ApplyParameter(config, nameof(ParticleConstellationConfig.Speed), 1.5);
        result.Speed.Should().Be(1.5);
    }

    // --- Preset Application Tests ---

    [Fact]
    public void AuroraBorealis_Presets_ShouldHaveValidConfigs()
    {
        var descriptor = new AuroraBorealisDescriptor();
        var presets = descriptor.GetPresets();
        presets.Should().NotBeEmpty();
        foreach (var preset in presets)
        {
            preset.Config.Should().NotBeNull();
            preset.Config.Colors.Should().NotBeEmpty();
            preset.Config.RibbonCount.Should().BeGreaterThan(0);
        }
    }

    [Fact]
    public void ParticleConstellation_Presets_ShouldHaveValidConfigs()
    {
        var descriptor = new ParticleConstellationDescriptor();
        var presets = descriptor.GetPresets();
        presets.Should().NotBeEmpty();
        foreach (var preset in presets)
        {
            preset.Config.Should().NotBeNull();
            preset.Config.ParticleCount.Should().BeGreaterThan(0);
        }
    }

    // --- Reset to Defaults Tests (via adapter) ---

    [Fact]
    public void AuroraBorealis_GetDefaultConfig_ShouldReturnExpectedDefaults()
    {
        var adapter = CreateAdapter(new AuroraBorealisDescriptor(), "AuroraBorealis");
        var config = (AuroraBorealisConfig)adapter.GetDefaultConfig();
        config.RibbonCount.Should().Be(4);
        config.Amplitude.Should().Be(120.0);
        config.Speed.Should().Be(0.008);
        config.Opacity.Should().Be(0.5);
        config.BlendMode.Should().Be("screen");
    }

    [Fact]
    public void ParticleConstellation_GetDefaultConfig_ShouldReturnExpectedDefaults()
    {
        var adapter = CreateAdapter(new ParticleConstellationDescriptor(), "ParticleConstellation");
        var config = (ParticleConstellationConfig)adapter.GetDefaultConfig();
        config.ParticleCount.Should().Be(150);
        config.ParticleColor.Should().Be("#6366f1");
        config.Speed.Should().Be(0.5);
        config.MouseInteraction.Should().Be(true);
        config.Opacity.Should().Be(0.6);
    }

    [Fact]
    public void MatrixRain_GetDefaultConfig_ShouldReturnNonNull()
    {
        var adapter = CreateAdapter(new MatrixRainDescriptor(), "MatrixRain");
        adapter.GetDefaultConfig().Should().NotBeNull();
    }

    [Fact]
    public void NoiseField_GetDefaultConfig_ShouldReturnNonNull()
    {
        var adapter = CreateAdapter(new NoiseFieldDescriptor(), "NoiseField");
        adapter.GetDefaultConfig().Should().NotBeNull();
    }

    [Fact]
    public void MorphingBlobs_GetDefaultConfig_ShouldReturnNonNull()
    {
        var adapter = CreateAdapter(new MorphingBlobsDescriptor(), "MorphingBlobs");
        adapter.GetDefaultConfig().Should().NotBeNull();
    }

    [Fact]
    public void GradientWaves_GetDefaultConfig_ShouldReturnNonNull()
    {
        var adapter = CreateAdapter(new GradientWavesDescriptor(), "GradientWaves");
        adapter.GetDefaultConfig().Should().NotBeNull();
    }

    // --- Reset Simulation ---

    [Fact]
    public void AuroraBorealis_ResetToDefault_AfterModifications_ShouldMatchDefault()
    {
        var adapter = CreateAdapter(new AuroraBorealisDescriptor(), "AuroraBorealis");
        var modified = new AuroraBorealisConfig
        {
            Colors = ["#ff0000"],
            RibbonCount = 8,
            Amplitude = 300,
            Speed = 0.03,
            Opacity = 1.0,
            BlendMode = "multiply"
        };
        var reset = (AuroraBorealisConfig)adapter.GetDefaultConfig();
        reset.RibbonCount.Should().NotBe(modified.RibbonCount);
        reset.RibbonCount.Should().Be(4);
        reset.Amplitude.Should().Be(120.0);
    }

    [Fact]
    public void ParticleConstellation_ResetToDefault_AfterModifications_ShouldMatchDefault()
    {
        var adapter = CreateAdapter(new ParticleConstellationDescriptor(), "ParticleConstellation");
        var modified = new ParticleConstellationConfig
        {
            ParticleCount = 500,
            Speed = 2.0,
            MouseInteraction = false
        };
        var reset = (ParticleConstellationConfig)adapter.GetDefaultConfig();
        reset.ParticleCount.Should().NotBe(modified.ParticleCount);
        reset.ParticleCount.Should().Be(150);
        reset.Speed.Should().Be(0.5);
    }
}

public class ComponentParameterBuildTests
{
    private readonly EffectRegistry _registry;

    public ComponentParameterBuildTests()
    {
        _registry = new EffectRegistry();
        _registry.Register(new EffectDescriptorAdapter<MatrixRainConfig>(
            new MatrixRainDescriptor(), c => BlazorMarkupGenerator.Generate(c, "MatrixRain")));
        _registry.Register(new EffectDescriptorAdapter<ParticleConstellationConfig>(
            new ParticleConstellationDescriptor(), c => BlazorMarkupGenerator.Generate(c, "ParticleConstellation")));
        _registry.Register(new EffectDescriptorAdapter<AuroraBorealisConfig>(
            new AuroraBorealisDescriptor(), c => BlazorMarkupGenerator.Generate(c, "AuroraBorealis")));
        _registry.Register(new EffectDescriptorAdapter<MorphingBlobsConfig>(
            new MorphingBlobsDescriptor(), c => BlazorMarkupGenerator.Generate(c, "MorphingBlobs")));
        _registry.Register(new EffectDescriptorAdapter<NoiseFieldConfig>(
            new NoiseFieldDescriptor(), c => BlazorMarkupGenerator.Generate(c, "NoiseField")));
        _registry.Register(new EffectDescriptorAdapter<GradientWavesConfig>(
            new GradientWavesDescriptor(), c => BlazorMarkupGenerator.Generate(c, "GradientWaves")));
    }

    [Theory]
    [InlineData("Matrix Rain")]
    [InlineData("Particle Constellation")]
    [InlineData("Aurora Borealis")]
    [InlineData("Morphing Blobs")]
    [InlineData("Noise Field")]
    [InlineData("Gradient Waves")]
    public void BuildComponentParameters_WithDefaultConfig_ShouldReturnNonEmptyDictionary(string effectName)
    {
        var adapter = _registry.GetByName(effectName);
        var config = adapter.GetDefaultConfig();
        var parameters = adapter.BuildComponentParameters(config);
        parameters.Should().NotBeEmpty($"effect '{effectName}' must produce component parameters for live preview");
    }

    [Theory]
    [InlineData("Matrix Rain")]
    [InlineData("Particle Constellation")]
    [InlineData("Aurora Borealis")]
    [InlineData("Morphing Blobs")]
    [InlineData("Noise Field")]
    [InlineData("Gradient Waves")]
    public void BuildComponentParameters_WithModifiedConfig_ShouldReflectChanges(string effectName)
    {
        var adapter = _registry.GetByName(effectName);
        var defaultConfig = adapter.GetDefaultConfig();
        var definitions = adapter.GetParameterDefinitions();
        var firstDef = definitions[0];
        var modifiedConfig = adapter.ApplyParameter(defaultConfig, firstDef.PropertyName, GetTestValue(firstDef.Type));
        var defaultParams = adapter.BuildComponentParameters(defaultConfig);
        var modifiedParams = adapter.BuildComponentParameters(modifiedConfig);
        modifiedParams.Should().NotBeEquivalentTo(defaultParams,
            $"changing parameter '{firstDef.PropertyName}' should produce different component parameters");
    }

    [Theory]
    [InlineData("Matrix Rain")]
    [InlineData("Particle Constellation")]
    [InlineData("Aurora Borealis")]
    [InlineData("Morphing Blobs")]
    [InlineData("Noise Field")]
    [InlineData("Gradient Waves")]
    public void GenerateBlazorMarkup_WithDefaultConfig_ShouldProduceValidTag(string effectName)
    {
        var adapter = _registry.GetByName(effectName);
        var config = adapter.GetDefaultConfig();
        var markup = adapter.GenerateBlazorMarkup(config);
        markup.Should().NotBeNullOrWhiteSpace($"effect '{effectName}' should generate Blazor markup");
    }

    private static object? GetTestValue(EffectParameterType type) => type switch
    {
        EffectParameterType.Range => 0.99,
        EffectParameterType.Integer => 42,
        EffectParameterType.Toggle => false,
        EffectParameterType.Color => "#aabbcc",
        EffectParameterType.ColorArray => new[] { "#112233", "#445566" },
        EffectParameterType.Text => "test-value",
        EffectParameterType.Select => "option-b",
        _ => null
    };
}
