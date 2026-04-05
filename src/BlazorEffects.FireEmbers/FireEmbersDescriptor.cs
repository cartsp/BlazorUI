using BlazorEffects.Core.Animation;

namespace BlazorEffects.FireEmbers;

/// <summary>
/// Describes the Fire/Embers effect's parameters and presets for the playground UI.
/// </summary>
public sealed class FireEmbersDescriptor : IEffectDescriptor<FireEmbersConfig>
{
    public Type ComponentType => typeof(FireEmbers);

    public string EffectName => "Fire & Embers";

    public IReadOnlyList<EffectParameterDefinition> GetParameterDefinitions() =>
    [
        new()
        {
            Name = "Particle Count",
            PropertyName = nameof(FireEmbersConfig.ParticleCount),
            Type = EffectParameterType.Integer,
            DefaultValue = 200,
            MinValue = 20,
            MaxValue = 600,
            Step = 10,
            Description = "Number of flame/ember particles",
            Order = 0
        },
        new()
        {
            Name = "Flame Color",
            PropertyName = nameof(FireEmbersConfig.FlameColor),
            Type = EffectParameterType.Color,
            DefaultValue = "#ff6600",
            Description = "Base flame color",
            Order = 1
        },
        new()
        {
            Name = "Ember Color",
            PropertyName = nameof(FireEmbersConfig.EmberColor),
            Type = EffectParameterType.Color,
            DefaultValue = "#ffcc00",
            Description = "Color of ember sparks",
            Order = 2
        },
        new()
        {
            Name = "Speed",
            PropertyName = nameof(FireEmbersConfig.Speed),
            Type = EffectParameterType.Range,
            DefaultValue = 1.5,
            MinValue = 0.2,
            MaxValue = 5.0,
            Step = 0.1,
            Description = "Particle rise speed multiplier",
            Order = 3
        },
        new()
        {
            Name = "Particle Size",
            PropertyName = nameof(FireEmbersConfig.ParticleSize),
            Type = EffectParameterType.Range,
            DefaultValue = 4.0,
            MinValue = 1.0,
            MaxValue = 10.0,
            Step = 0.5,
            Description = "Maximum particle radius",
            Order = 4
        },
        new()
        {
            Name = "Turbulence",
            PropertyName = nameof(FireEmbersConfig.Turbulence),
            Type = EffectParameterType.Range,
            DefaultValue = 1.0,
            MinValue = 0.0,
            MaxValue = 3.0,
            Step = 0.1,
            Description = "Horizontal wobble intensity",
            Order = 5
        },
        new()
        {
            Name = "Ember Ratio",
            PropertyName = nameof(FireEmbersConfig.EmberRatio),
            Type = EffectParameterType.Range,
            DefaultValue = 0.3,
            MinValue = 0.0,
            MaxValue = 1.0,
            Step = 0.05,
            Description = "Percentage of particles that are embers vs flames",
            Order = 6
        },
        new()
        {
            Name = "Background Color",
            PropertyName = nameof(FireEmbersConfig.BackgroundColor),
            Type = EffectParameterType.Color,
            DefaultValue = "#0a0a0a",
            Description = "Background color",
            Order = 7
        },
        new()
        {
            Name = "Opacity",
            PropertyName = nameof(FireEmbersConfig.Opacity),
            Type = EffectParameterType.Range,
            DefaultValue = 0.9,
            MinValue = 0.1,
            MaxValue = 1.0,
            Step = 0.05,
            Description = "Overall effect opacity",
            Order = 8
        }
    ];

    public IReadOnlyList<EffectPreset<FireEmbersConfig>> GetPresets() =>
    [
        new()
        {
            Name = "Campfire",
            Description = "Warm orange campfire with golden embers",
            Config = FireEmbersPresets.Default,
            PreviewGradient = "linear-gradient(90deg, #0a0a0a, #ff6600, #ffcc00)"
        },
        new()
        {
            Name = "Bonfire",
            Description = "Large intense bonfire",
            Config = FireEmbersPresets.Bonfire,
            PreviewGradient = "linear-gradient(90deg, #1a0500, #ff4400, #ffaa00)"
        },
        new()
        {
            Name = "Candlelight",
            Description = "Gentle warm candle glow",
            Config = FireEmbersPresets.Candlelight,
            PreviewGradient = "linear-gradient(90deg, #0a0a0a, #ffaa33, #ffdd88)"
        },
        new()
        {
            Name = "Inferno",
            Description = "Aggressive fast burning inferno",
            Config = FireEmbersPresets.Inferno,
            PreviewGradient = "linear-gradient(90deg, #1a0000, #ff2200, #ffff00)"
        },
        new()
        {
            Name = "Blue Flame",
            Description = "Magical blue gas flame",
            Config = FireEmbersPresets.BlueFlame,
            PreviewGradient = "linear-gradient(90deg, #050510, #3b82f6, #93c5fd)"
        },
        new()
        {
            Name = "Embers Only",
            Description = "Sparse sparks rising from coals",
            Config = FireEmbersPresets.EmbersOnly,
            PreviewGradient = "linear-gradient(90deg, #0a0a0a, #cc3300, #ff8800)"
        }
    ];

    public FireEmbersConfig ApplyParameter(FireEmbersConfig config, string propertyName, object? value)
    {
        return propertyName switch
        {
            nameof(FireEmbersConfig.ParticleCount) => config with { ParticleCount = Convert.ToInt32(value) },
            nameof(FireEmbersConfig.FlameColor) => config with { FlameColor = value?.ToString() ?? "#ff6600" },
            nameof(FireEmbersConfig.EmberColor) => config with { EmberColor = value?.ToString() ?? "#ffcc00" },
            nameof(FireEmbersConfig.Speed) => config with { Speed = Convert.ToDouble(value) },
            nameof(FireEmbersConfig.ParticleSize) => config with { ParticleSize = Convert.ToDouble(value) },
            nameof(FireEmbersConfig.Turbulence) => config with { Turbulence = Convert.ToDouble(value) },
            nameof(FireEmbersConfig.EmberRatio) => config with { EmberRatio = Convert.ToDouble(value) },
            nameof(FireEmbersConfig.BackgroundColor) => config with { BackgroundColor = value?.ToString() ?? "#0a0a0a" },
            nameof(FireEmbersConfig.Opacity) => config with { Opacity = Convert.ToDouble(value) },
            _ => config
        };
    }
}
