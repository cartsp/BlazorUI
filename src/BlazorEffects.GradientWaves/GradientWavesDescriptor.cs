using BlazorEffects.Core.Animation;

namespace BlazorEffects.GradientWaves;

/// <summary>
/// Describes the Gradient Waves effect's parameters and presets for the playground UI.
/// </summary>
public sealed class GradientWavesDescriptor : IEffectDescriptor<GradientWavesConfig>
{
    public Type ComponentType => typeof(GradientWaves);

    public string EffectName => "Gradient Waves";

    public IReadOnlyList<EffectParameterDefinition> GetParameterDefinitions() =>
    [
        new()
        {
            Name = "Colors",
            PropertyName = nameof(GradientWavesConfig.Colors),
            Type = EffectParameterType.ColorArray,
            DefaultValue = GradientWavesPresets.Stripe,
            Description = "Color palette for the mesh gradient",
            Order = 0
        },
        new()
        {
            Name = "Point Count",
            PropertyName = nameof(GradientWavesConfig.PointCount),
            Type = EffectParameterType.Integer,
            DefaultValue = 6,
            MinValue = 2,
            MaxValue = 12,
            Step = 1,
            Description = "Number of drifting color control points",
            Order = 1
        },
        new()
        {
            Name = "Blob Size",
            PropertyName = nameof(GradientWavesConfig.BlobSize),
            Type = EffectParameterType.Range,
            DefaultValue = 0.5,
            MinValue = 0.1,
            MaxValue = 1.0,
            Step = 0.05,
            Description = "Size of each color blob (fraction of canvas diagonal)",
            Order = 2
        },
        new()
        {
            Name = "Speed",
            PropertyName = nameof(GradientWavesConfig.Speed),
            Type = EffectParameterType.Range,
            DefaultValue = 0.004,
            MinValue = 0.001,
            MaxValue = 0.02,
            Step = 0.001,
            Description = "Drift speed of color points",
            Order = 3
        },
        new()
        {
            Name = "Blur Amount",
            PropertyName = nameof(GradientWavesConfig.BlurAmount),
            Type = EffectParameterType.Range,
            DefaultValue = 80.0,
            MinValue = 10,
            MaxValue = 200,
            Step = 5,
            Description = "Gaussian blur radius for smooth gradient blending",
            Order = 4
        },
        new()
        {
            Name = "Blend Mode",
            PropertyName = nameof(GradientWavesConfig.BlendMode),
            Type = EffectParameterType.Text,
            DefaultValue = "normal",
            Description = "CSS blend mode for color mixing",
            Order = 5
        },
        new()
        {
            Name = "Opacity",
            PropertyName = nameof(GradientWavesConfig.Opacity),
            Type = EffectParameterType.Range,
            DefaultValue = 1.0,
            MinValue = 0.1,
            MaxValue = 1.0,
            Step = 0.05,
            Description = "Overall effect opacity",
            Order = 6
        }
    ];

    public IReadOnlyList<EffectPreset<GradientWavesConfig>> GetPresets() =>
    [
        new()
        {
            Name = "Stripe",
            Description = "Vibrant tech gradient inspired by Stripe",
            Config = new() { Colors = GradientWavesPresets.Stripe, PointCount = 6, BlobSize = 0.5, Speed = 0.004, BlurAmount = 80 },
            PreviewGradient = "linear-gradient(90deg, #635bff, #00d4ff, #ff6b9d, #a855f7)"
        },
        new()
        {
            Name = "Vibrant Sunset",
            Description = "Warm, dramatic sunset with orange and magenta",
            Config = new() { Colors = GradientWavesPresets.VibrantSunset, PointCount = 6, BlobSize = 0.55, Speed = 0.003, BlurAmount = 90 },
            PreviewGradient = "linear-gradient(90deg, #ff6b35, #ff2d87, #ffd23f, #ff7eb3)"
        },
        new()
        {
            Name = "Ocean Deep",
            Description = "Cool, deep ocean blues and teals",
            Config = new() { Colors = GradientWavesPresets.OceanDeep, PointCount = 5, BlobSize = 0.6, Speed = 0.002, BlurAmount = 100 },
            PreviewGradient = "linear-gradient(90deg, #0a2463, #1e6091, #34d399, #76e4f7)"
        },
        new()
        {
            Name = "Northern Lights",
            Description = "Aurora borealis with greens, violets and cyans",
            Config = new() { Colors = GradientWavesPresets.NorthernLights, PointCount = 6, BlobSize = 0.45, Speed = 0.005, BlurAmount = 75 },
            PreviewGradient = "linear-gradient(90deg, #10b981, #8b5cf6, #22d3ee, #d946ef)"
        },
        new()
        {
            Name = "Cyberpunk",
            Description = "Electric neon with green, magenta and blue",
            Config = new() { Colors = GradientWavesPresets.Cyberpunk, PointCount = 5, BlobSize = 0.4, Speed = 0.006, BlurAmount = 70 },
            PreviewGradient = "linear-gradient(90deg, #39ff14, #ff00ff, #00e5ff, #ffe600)"
        },
        new()
        {
            Name = "Rose Garden",
            Description = "Elegant pinks and roses for a soft, feminine feel",
            Config = new() { Colors = GradientWavesPresets.RoseGarden, PointCount = 5, BlobSize = 0.5, Speed = 0.003, BlurAmount = 90 },
            PreviewGradient = "linear-gradient(90deg, #fb7185, #f43f5e, #fecdd3, #be185d)"
        },
        new()
        {
            Name = "Minimal Mono",
            Description = "Subtle, professional grays for corporate elegance",
            Config = new() { Colors = GradientWavesPresets.MinimalMono, PointCount = 4, BlobSize = 0.55, Speed = 0.002, BlurAmount = 100 },
            PreviewGradient = "linear-gradient(90deg, #e5e5e5, #a3a3a3, #737373, #d4d4d4)"
        },
        new()
        {
            Name = "Forest",
            Description = "Deep greens and emeralds for a natural feel",
            Config = new() { Colors = GradientWavesPresets.Forest, PointCount = 5, BlobSize = 0.5, Speed = 0.003, BlurAmount = 85 },
            PreviewGradient = "linear-gradient(90deg, #064e3b, #059669, #34d399, #166534)"
        }
    ];

    public GradientWavesConfig ApplyParameter(GradientWavesConfig config, string propertyName, object? value)
    {
        return propertyName switch
        {
            nameof(GradientWavesConfig.Colors) => config with { Colors = value as string[] ?? GradientWavesPresets.Stripe },
            nameof(GradientWavesConfig.PointCount) => config with { PointCount = Convert.ToInt32(value) },
            nameof(GradientWavesConfig.BlobSize) => config with { BlobSize = Convert.ToDouble(value) },
            nameof(GradientWavesConfig.Speed) => config with { Speed = Convert.ToDouble(value) },
            nameof(GradientWavesConfig.BlurAmount) => config with { BlurAmount = Convert.ToDouble(value) },
            nameof(GradientWavesConfig.BlendMode) => config with { BlendMode = value?.ToString() ?? "normal" },
            nameof(GradientWavesConfig.Opacity) => config with { Opacity = Convert.ToDouble(value) },
            _ => config
        };
    }
}
