using BlazorEffects.Core.Animation;

namespace BlazorEffects.Blobs;

/// <summary>
/// Describes the Morphing Blobs effect's parameters and presets for the playground UI.
/// </summary>
public sealed class MorphingBlobsDescriptor : IEffectDescriptor<MorphingBlobsConfig>
{
    public Type ComponentType => typeof(MorphingBlobs);

    public string EffectName => "Morphing Blobs";

    public IReadOnlyList<EffectParameterDefinition> GetParameterDefinitions() =>
    [
        new()
        {
            Name = "Blob Count",
            PropertyName = nameof(MorphingBlobsConfig.BlobCount),
            Type = EffectParameterType.Integer,
            DefaultValue = 4,
            MinValue = 2,
            MaxValue = 8,
            Step = 1,
            Description = "Number of animated blobs",
            Order = 0
        },
        new()
        {
            Name = "Colors",
            PropertyName = nameof(MorphingBlobsConfig.Colors),
            Type = EffectParameterType.ColorArray,
            DefaultValue = MorphingBlobsPresets.Default,
            Description = "Color palette for the blobs",
            Order = 1
        },
        new()
        {
            Name = "Blob Size",
            PropertyName = nameof(MorphingBlobsConfig.BlobSize),
            Type = EffectParameterType.Range,
            DefaultValue = 300.0,
            MinValue = 100,
            MaxValue = 600,
            Step = 25,
            Description = "Base size of each blob",
            Order = 2
        },
        new()
        {
            Name = "Speed",
            PropertyName = nameof(MorphingBlobsConfig.Speed),
            Type = EffectParameterType.Range,
            DefaultValue = 0.005,
            MinValue = 0.001,
            MaxValue = 0.02,
            Step = 0.001,
            Description = "Animation speed of blob movement",
            Order = 3
        },
        new()
        {
            Name = "Morph Intensity",
            PropertyName = nameof(MorphingBlobsConfig.MorphIntensity),
            Type = EffectParameterType.Range,
            DefaultValue = 80.0,
            MinValue = 10,
            MaxValue = 200,
            Step = 5,
            Description = "Intensity of the morphing deformation",
            Order = 4
        },
        new()
        {
            Name = "Blend Mode",
            PropertyName = nameof(MorphingBlobsConfig.BlendMode),
            Type = EffectParameterType.Text,
            DefaultValue = "screen",
            Description = "CSS blend mode for overlapping blobs",
            Order = 5
        },
        new()
        {
            Name = "Opacity",
            PropertyName = nameof(MorphingBlobsConfig.Opacity),
            Type = EffectParameterType.Range,
            DefaultValue = 0.7,
            MinValue = 0.1,
            MaxValue = 1.0,
            Step = 0.05,
            Description = "Overall effect opacity",
            Order = 6
        }
    ];

    public IReadOnlyList<EffectPreset<MorphingBlobsConfig>> GetPresets() =>
    [
        new()
        {
            Name = "Lava Lamp",
            Description = "Warm, organic motion with reds and purples",
            Config = new() { Colors = ["#ff6b35", "#ff2d87", "#9333ea"], BlobCount = 4, BlobSize = 300, Speed = 0.005, MorphIntensity = 80 },
            PreviewGradient = "linear-gradient(90deg, #ff6b35, #ff2d87, #9333ea)"
        },
        new()
        {
            Name = "Ocean",
            Description = "Cool, flowing water blues and teals",
            Config = new() { Colors = ["#0066ff", "#00ccff", "#00ffcc", "#38bdf8"], BlobCount = 5, BlobSize = 350, Speed = 0.003, MorphIntensity = 100 },
            PreviewGradient = "linear-gradient(90deg, #0066ff, #00ccff, #00ffcc)"
        },
        new()
        {
            Name = "Neon",
            Description = "Electric, vivid neon colors",
            Config = new() { Colors = ["#ff00ff", "#00ffff", "#ffff00"], BlobCount = 3, BlobSize = 250, Speed = 0.008, MorphIntensity = 60 },
            PreviewGradient = "linear-gradient(90deg, #ff00ff, #00ffff, #ffff00)"
        },
        new()
        {
            Name = "Dawn",
            Description = "Soft, warm gradients of sunrise",
            Config = new() { Colors = ["#ff9a56", "#ff6b88", "#c471f5"], BlobCount = 4, BlobSize = 280, Speed = 0.004, MorphIntensity = 90 },
            PreviewGradient = "linear-gradient(90deg, #ff9a56, #ff6b88, #c471f5)"
        },
        new()
        {
            Name = "Deep",
            Description = "Dark, atmospheric deep tones",
            Config = new() { Colors = ["#1a1a2e", "#16213e", "#0f3460"], BlobCount = 3, BlobSize = 400, Speed = 0.002, MorphIntensity = 120 },
            PreviewGradient = "linear-gradient(90deg, #1a1a2e, #16213e, #0f3460)"
        }
    ];

    public MorphingBlobsConfig ApplyParameter(MorphingBlobsConfig config, string propertyName, object? value)
    {
        return propertyName switch
        {
            nameof(MorphingBlobsConfig.BlobCount) => config with { BlobCount = Convert.ToInt32(value) },
            nameof(MorphingBlobsConfig.Colors) => config with { Colors = value as string[] ?? MorphingBlobsPresets.Default },
            nameof(MorphingBlobsConfig.BlobSize) => config with { BlobSize = Convert.ToDouble(value) },
            nameof(MorphingBlobsConfig.Speed) => config with { Speed = Convert.ToDouble(value) },
            nameof(MorphingBlobsConfig.MorphIntensity) => config with { MorphIntensity = Convert.ToDouble(value) },
            nameof(MorphingBlobsConfig.BlendMode) => config with { BlendMode = value?.ToString() ?? "screen" },
            nameof(MorphingBlobsConfig.Opacity) => config with { Opacity = Convert.ToDouble(value) },
            _ => config
        };
    }
}
