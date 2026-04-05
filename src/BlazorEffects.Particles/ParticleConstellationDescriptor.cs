using BlazorEffects.Core.Animation;

namespace BlazorEffects.Particles;

/// <summary>
/// Describes the Particle Constellation effect's parameters and presets for the playground UI.
/// </summary>
public sealed class ParticleConstellationDescriptor : IEffectDescriptor<ParticleConstellationConfig>
{
    public Type ComponentType => typeof(ParticleConstellation);

    public string EffectName => "Particle Constellation";

    public IReadOnlyList<EffectParameterDefinition> GetParameterDefinitions() =>
    [
        new()
        {
            Name = "Particle Count",
            PropertyName = nameof(ParticleConstellationConfig.ParticleCount),
            Type = EffectParameterType.Integer,
            DefaultValue = 150,
            MinValue = 20,
            MaxValue = 500,
            Step = 10,
            Description = "Number of particles in the constellation",
            Order = 0
        },
        new()
        {
            Name = "Particle Color",
            PropertyName = nameof(ParticleConstellationConfig.ParticleColor),
            Type = EffectParameterType.Color,
            DefaultValue = "#6366f1",
            Description = "Color of the particles",
            Order = 1
        },
        new()
        {
            Name = "Particle Size",
            PropertyName = nameof(ParticleConstellationConfig.ParticleSize),
            Type = EffectParameterType.Range,
            DefaultValue = 2.0,
            MinValue = 0.5,
            MaxValue = 6,
            Step = 0.5,
            Description = "Radius of each particle",
            Order = 2
        },
        new()
        {
            Name = "Connection Distance",
            PropertyName = nameof(ParticleConstellationConfig.ConnectionDistance),
            Type = EffectParameterType.Range,
            DefaultValue = 120.0,
            MinValue = 30,
            MaxValue = 300,
            Step = 10,
            Description = "Max distance to draw connection lines",
            Order = 3
        },
        new()
        {
            Name = "Connection Color",
            PropertyName = nameof(ParticleConstellationConfig.ConnectionColor),
            Type = EffectParameterType.Color,
            DefaultValue = "#6366f1",
            Description = "Color of connection lines between particles",
            Order = 4
        },
        new()
        {
            Name = "Speed",
            PropertyName = nameof(ParticleConstellationConfig.Speed),
            Type = EffectParameterType.Range,
            DefaultValue = 0.5,
            MinValue = 0.1,
            MaxValue = 2.0,
            Step = 0.1,
            Description = "Particle movement speed",
            Order = 5
        },
        new()
        {
            Name = "Mouse Interaction",
            PropertyName = nameof(ParticleConstellationConfig.MouseInteraction),
            Type = EffectParameterType.Toggle,
            DefaultValue = true,
            Description = "Whether particles react to mouse cursor",
            Order = 6
        },
        new()
        {
            Name = "Mouse Radius",
            PropertyName = nameof(ParticleConstellationConfig.MouseRadius),
            Type = EffectParameterType.Range,
            DefaultValue = 150.0,
            MinValue = 50,
            MaxValue = 300,
            Step = 10,
            Description = "Radius of mouse influence area",
            Order = 7
        },
        new()
        {
            Name = "Mouse Force",
            PropertyName = nameof(ParticleConstellationConfig.MouseForce),
            Type = EffectParameterType.Text,
            DefaultValue = "attract",
            Description = "Mouse force direction: attract or repel",
            Order = 8
        },
        new()
        {
            Name = "Opacity",
            PropertyName = nameof(ParticleConstellationConfig.Opacity),
            Type = EffectParameterType.Range,
            DefaultValue = 0.6,
            MinValue = 0.1,
            MaxValue = 1.0,
            Step = 0.05,
            Description = "Overall effect opacity",
            Order = 9
        }
    ];

    public IReadOnlyList<EffectPreset<ParticleConstellationConfig>> GetPresets() =>
    [
        new()
        {
            Name = "Starfield",
            Description = "Classic indigo starfield network",
            Config = ParticleConstellationPresets.Default,
            PreviewGradient = "linear-gradient(90deg, #1e1b4b, #6366f1, #3b82f6)"
        },
        new()
        {
            Name = "Cyberpunk",
            Description = "Neon green cyberpunk aesthetic",
            Config = ParticleConstellationPresets.Cyberpunk,
            PreviewGradient = "linear-gradient(90deg, #001a00, #00ff41, #00ff41)"
        },
        new()
        {
            Name = "Deep Space",
            Description = "Cool blue deep space constellation",
            Config = ParticleConstellationPresets.DeepSpace,
            PreviewGradient = "linear-gradient(90deg, #0c1445, #38bdf8, #1e40af)"
        },
        new()
        {
            Name = "Amber",
            Description = "Warm amber particle network",
            Config = ParticleConstellationPresets.Amber,
            PreviewGradient = "linear-gradient(90deg, #1c0a00, #f59e0b, #ea580c)"
        },
        new()
        {
            Name = "Dense",
            Description = "Dense purple mesh connectivity",
            Config = ParticleConstellationPresets.Dense,
            PreviewGradient = "linear-gradient(90deg, #2e1065, #a855f7, #7c3aed)"
        }
    ];

    public ParticleConstellationConfig ApplyParameter(ParticleConstellationConfig config, string propertyName, object? value)
    {
        return propertyName switch
        {
            nameof(ParticleConstellationConfig.ParticleCount) => config with { ParticleCount = Convert.ToInt32(value) },
            nameof(ParticleConstellationConfig.ParticleColor) => config with { ParticleColor = value?.ToString() ?? "#6366f1" },
            nameof(ParticleConstellationConfig.ParticleSize) => config with { ParticleSize = Convert.ToDouble(value) },
            nameof(ParticleConstellationConfig.ConnectionDistance) => config with { ConnectionDistance = Convert.ToDouble(value) },
            nameof(ParticleConstellationConfig.ConnectionColor) => config with { ConnectionColor = value?.ToString() ?? "#6366f1" },
            nameof(ParticleConstellationConfig.Speed) => config with { Speed = Convert.ToDouble(value) },
            nameof(ParticleConstellationConfig.MouseInteraction) => config with { MouseInteraction = Convert.ToBoolean(value) },
            nameof(ParticleConstellationConfig.MouseRadius) => config with { MouseRadius = Convert.ToDouble(value) },
            nameof(ParticleConstellationConfig.MouseForce) => config with { MouseForce = value?.ToString() ?? "attract" },
            nameof(ParticleConstellationConfig.Opacity) => config with { Opacity = Convert.ToDouble(value) },
            _ => config
        };
    }
}
