using BlazorEffects.Core.Animation;

namespace BlazorEffects.Aurora;

/// <summary>
/// Describes the Aurora Borealis effect's parameters and presets for the playground UI.
/// </summary>
public sealed class AuroraBorealisDescriptor : IEffectDescriptor<AuroraBorealisConfig>
{
    public Type ComponentType => typeof(AuroraBorealis);

    public string EffectName => "Aurora Borealis";

    public IReadOnlyList<EffectParameterDefinition> GetParameterDefinitions() =>
    [
        new()
        {
            Name = "Colors",
            PropertyName = nameof(AuroraBorealisConfig.Colors),
            Type = EffectParameterType.ColorArray,
            DefaultValue = AuroraBorealisPresets.Classic,
            Description = "Color palette for the aurora ribbons",
            Order = 0
        },
        new()
        {
            Name = "Ribbon Count",
            PropertyName = nameof(AuroraBorealisConfig.RibbonCount),
            Type = EffectParameterType.Integer,
            DefaultValue = 4,
            MinValue = 1,
            MaxValue = 8,
            Step = 1,
            Description = "Number of aurora ribbons",
            Order = 1
        },
        new()
        {
            Name = "Amplitude",
            PropertyName = nameof(AuroraBorealisConfig.Amplitude),
            Type = EffectParameterType.Range,
            DefaultValue = 120.0,
            MinValue = 20,
            MaxValue = 300,
            Step = 10,
            Description = "Vertical amplitude of ribbon waves",
            Order = 2
        },
        new()
        {
            Name = "Speed",
            PropertyName = nameof(AuroraBorealisConfig.Speed),
            Type = EffectParameterType.Range,
            DefaultValue = 0.008,
            MinValue = 0.001,
            MaxValue = 0.03,
            Step = 0.001,
            Description = "Animation speed of the aurora",
            Order = 3
        },
        new()
        {
            Name = "Opacity",
            PropertyName = nameof(AuroraBorealisConfig.Opacity),
            Type = EffectParameterType.Range,
            DefaultValue = 0.5,
            MinValue = 0.1,
            MaxValue = 1.0,
            Step = 0.05,
            Description = "Overall effect opacity",
            Order = 4
        },
        new()
        {
            Name = "Blend Mode",
            PropertyName = nameof(AuroraBorealisConfig.BlendMode),
            Type = EffectParameterType.Text,
            DefaultValue = "screen",
            Description = "CSS blend mode for overlapping ribbons",
            Order = 5
        }
    ];

    public IReadOnlyList<EffectPreset<AuroraBorealisConfig>> GetPresets() =>
    [
        new()
        {
            Name = "Northern Lights",
            Description = "Classic green-purple aurora with teal accents",
            Config = new() { Colors = AuroraBorealisPresets.Classic, RibbonCount = 4, Amplitude = 120, Speed = 0.008 },
            PreviewGradient = "linear-gradient(90deg, #00ff87, #7b2ff7, #00b4d8)"
        },
        new()
        {
            Name = "Arctic Dawn",
            Description = "Soft pastel ribbons with rose and cyan",
            Config = new() { Colors = ["#ff6b9d", "#c44dff", "#6ec6ff"], RibbonCount = 3, Amplitude = 80, Speed = 0.005 },
            PreviewGradient = "linear-gradient(90deg, #ff6b9d, #c44dff, #6ec6ff)"
        },
        new()
        {
            Name = "Deep Space",
            Description = "Dramatic wide bands in deep blue tones",
            Config = new() { Colors = ["#4a0e8f", "#00d4ff", "#0a3d62"], RibbonCount = 5, Amplitude = 200, Speed = 0.012 },
            PreviewGradient = "linear-gradient(90deg, #4a0e8f, #00d4ff, #0a3d62)"
        },
        new()
        {
            Name = "Emerald",
            Description = "Monochrome green aurora",
            Config = new() { Colors = AuroraBorealisPresets.Emerald, RibbonCount = 3, Amplitude = 100, Speed = 0.006 },
            PreviewGradient = "linear-gradient(90deg, #00ff87, #38b000, #70e000, #008000)"
        },
        new()
        {
            Name = "Twilight",
            Description = "Warm sunset aurora with pink and purple",
            Config = new() { Colors = ["#ff7eb3", "#ff758c", "#a855f7"], RibbonCount = 4, Amplitude = 150, Speed = 0.010 },
            PreviewGradient = "linear-gradient(90deg, #ff7eb3, #ff758c, #a855f7)"
        }
    ];

    public AuroraBorealisConfig ApplyParameter(AuroraBorealisConfig config, string propertyName, object? value)
    {
        return propertyName switch
        {
            nameof(AuroraBorealisConfig.Colors) => config with { Colors = value as string[] ?? AuroraBorealisPresets.Classic },
            nameof(AuroraBorealisConfig.RibbonCount) => config with { RibbonCount = Convert.ToInt32(value) },
            nameof(AuroraBorealisConfig.Amplitude) => config with { Amplitude = Convert.ToDouble(value) },
            nameof(AuroraBorealisConfig.Speed) => config with { Speed = Convert.ToDouble(value) },
            nameof(AuroraBorealisConfig.Opacity) => config with { Opacity = Convert.ToDouble(value) },
            nameof(AuroraBorealisConfig.BlendMode) => config with { BlendMode = value?.ToString() ?? "screen" },
            _ => config
        };
    }
}
