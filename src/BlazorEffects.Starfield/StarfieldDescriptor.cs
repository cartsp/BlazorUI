using BlazorEffects.Core.Animation;

namespace BlazorEffects.Starfield;

/// <summary>
/// Describes the Starfield effect's parameters and presets for the playground UI.
/// </summary>
public sealed class StarfieldDescriptor : IEffectDescriptor<StarfieldConfig>
{
    public Type ComponentType => typeof(Starfield);

    public string EffectName => "Starfield";

    public IReadOnlyList<EffectParameterDefinition> GetParameterDefinitions() =>
    [
        new()
        {
            Name = "Star Count",
            PropertyName = nameof(StarfieldConfig.StarCount),
            Type = EffectParameterType.Integer,
            DefaultValue = 800,
            MinValue = 50,
            MaxValue = 3000,
            Step = 50,
            Description = "Number of stars in the field",
            Order = 0
        },
        new()
        {
            Name = "Star Color",
            PropertyName = nameof(StarfieldConfig.StarColor),
            Type = EffectParameterType.Color,
            DefaultValue = "#ffffff",
            Description = "Color of the stars",
            Order = 1
        },
        new()
        {
            Name = "Star Size",
            PropertyName = nameof(StarfieldConfig.StarSize),
            Type = EffectParameterType.Range,
            DefaultValue = 2.0,
            MinValue = 0.5,
            MaxValue = 5.0,
            Step = 0.5,
            Description = "Maximum star radius",
            Order = 2
        },
        new()
        {
            Name = "Speed",
            PropertyName = nameof(StarfieldConfig.Speed),
            Type = EffectParameterType.Range,
            DefaultValue = 2.0,
            MinValue = 0.2,
            MaxValue = 8.0,
            Step = 0.2,
            Description = "Warp speed multiplier",
            Order = 3
        },
        new()
        {
            Name = "Trail Length",
            PropertyName = nameof(StarfieldConfig.TrailLength),
            Type = EffectParameterType.Range,
            DefaultValue = 0.6,
            MinValue = 0.0,
            MaxValue = 1.0,
            Step = 0.05,
            Description = "Length of speed trails behind each star",
            Order = 4
        },
        new()
        {
            Name = "Depth",
            PropertyName = nameof(StarfieldConfig.Depth),
            Type = EffectParameterType.Range,
            DefaultValue = 1000.0,
            MinValue = 200,
            MaxValue = 3000,
            Step = 100,
            Description = "Depth of the star field",
            Order = 5
        },
        new()
        {
            Name = "Background Color",
            PropertyName = nameof(StarfieldConfig.BackgroundColor),
            Type = EffectParameterType.Color,
            DefaultValue = "#000000",
            Description = "Background color",
            Order = 6
        },
        new()
        {
            Name = "Opacity",
            PropertyName = nameof(StarfieldConfig.Opacity),
            Type = EffectParameterType.Range,
            DefaultValue = 1.0,
            MinValue = 0.1,
            MaxValue = 1.0,
            Step = 0.05,
            Description = "Overall effect opacity",
            Order = 7
        }
    ];

    public IReadOnlyList<EffectPreset<StarfieldConfig>> GetPresets() =>
    [
        new()
        {
            Name = "Hyperspace",
            Description = "Classic white star warp speed",
            Config = StarfieldPresets.Default,
            PreviewGradient = "linear-gradient(90deg, #000000, #ffffff, #000000)"
        },
        new()
        {
            Name = "Warp Drive",
            Description = "Blue-tinted FTL warp",
            Config = StarfieldPresets.WarpDrive,
            PreviewGradient = "linear-gradient(90deg, #0c1445, #60a5fa, #1e40af)"
        },
        new()
        {
            Name = "Golden Drift",
            Description = "Warm gold ambient stars",
            Config = StarfieldPresets.GoldenDrift,
            PreviewGradient = "linear-gradient(90deg, #1c0a00, #fbbf24, #92400e)"
        },
        new()
        {
            Name = "Sparse",
            Description = "Minimalist bright stars",
            Config = StarfieldPresets.Sparse,
            PreviewGradient = "linear-gradient(90deg, #0f172a, #e2e8f0, #334155)"
        },
        new()
        {
            Name = "Blizzard",
            Description = "Intense dense star storm",
            Config = StarfieldPresets.Blizzard,
            PreviewGradient = "linear-gradient(90deg, #1e3a5f, #bfdbfe, #93c5fd)"
        },
        new()
        {
            Name = "Retro Terminal",
            Description = "Neon green terminal aesthetic",
            Config = StarfieldPresets.RetroTerminal,
            PreviewGradient = "linear-gradient(90deg, #0a0a0a, #00ff41, #003a00)"
        }
    ];

    public StarfieldConfig ApplyParameter(StarfieldConfig config, string propertyName, object? value)
    {
        return propertyName switch
        {
            nameof(StarfieldConfig.StarCount) => config with { StarCount = Convert.ToInt32(value) },
            nameof(StarfieldConfig.StarColor) => config with { StarColor = value?.ToString() ?? "#ffffff" },
            nameof(StarfieldConfig.StarSize) => config with { StarSize = Convert.ToDouble(value) },
            nameof(StarfieldConfig.Speed) => config with { Speed = Convert.ToDouble(value) },
            nameof(StarfieldConfig.TrailLength) => config with { TrailLength = Convert.ToDouble(value) },
            nameof(StarfieldConfig.Depth) => config with { Depth = Convert.ToDouble(value) },
            nameof(StarfieldConfig.BackgroundColor) => config with { BackgroundColor = value?.ToString() ?? "#000000" },
            nameof(StarfieldConfig.Opacity) => config with { Opacity = Convert.ToDouble(value) },
            _ => config
        };
    }
}
