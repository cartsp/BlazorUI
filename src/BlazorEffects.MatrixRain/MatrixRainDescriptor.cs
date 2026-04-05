using BlazorEffects.Core.Animation;

namespace BlazorEffects.MatrixRain;

/// <summary>
/// Describes the Matrix Rain effect's parameters and presets for the playground UI.
/// </summary>
public sealed class MatrixRainDescriptor : IEffectDescriptor<MatrixRainConfig>
{
    public Type ComponentType => typeof(MatrixRain);

    public string EffectName => "Matrix Rain";

    public IReadOnlyList<EffectParameterDefinition> GetParameterDefinitions() =>
    [
        new()
        {
            Name = "Characters",
            PropertyName = nameof(MatrixRainConfig.Characters),
            Type = EffectParameterType.Text,
            DefaultValue = MatrixRainPresets.Classic,
            Description = "Character pool for the rain streams",
            Order = 0
        },
        new()
        {
            Name = "Font Size",
            PropertyName = nameof(MatrixRainConfig.FontSize),
            Type = EffectParameterType.Range,
            DefaultValue = 16.0,
            MinValue = 8,
            MaxValue = 32,
            Step = 1,
            Description = "Font size in px (also determines column width)",
            Order = 1
        },
        new()
        {
            Name = "Font Family",
            PropertyName = nameof(MatrixRainConfig.FontFamily),
            Type = EffectParameterType.Text,
            DefaultValue = "monospace",
            Description = "Canvas font family for the characters",
            Order = 2
        },
        new()
        {
            Name = "Color",
            PropertyName = nameof(MatrixRainConfig.Color),
            Type = EffectParameterType.Color,
            DefaultValue = "#00ff41",
            Description = "Lead character color",
            Order = 3
        },
        new()
        {
            Name = "Fade Color",
            PropertyName = nameof(MatrixRainConfig.FadeColor),
            Type = EffectParameterType.Color,
            DefaultValue = "#003b00",
            Description = "Background fade color for the trailing effect",
            Order = 4
        },
        new()
        {
            Name = "Speed",
            PropertyName = nameof(MatrixRainConfig.Speed),
            Type = EffectParameterType.Range,
            DefaultValue = 1.0,
            MinValue = 0.1,
            MaxValue = 3.0,
            Step = 0.1,
            Description = "Fall speed multiplier",
            Order = 5
        },
        new()
        {
            Name = "Density",
            PropertyName = nameof(MatrixRainConfig.Density),
            Type = EffectParameterType.Range,
            DefaultValue = 1.0,
            MinValue = 0.1,
            MaxValue = 1.0,
            Step = 0.1,
            Description = "Active column fraction (0.0-1.0)",
            Order = 6
        },
        new()
        {
            Name = "Opacity",
            PropertyName = nameof(MatrixRainConfig.Opacity),
            Type = EffectParameterType.Range,
            DefaultValue = 0.8,
            MinValue = 0.1,
            MaxValue = 1.0,
            Step = 0.05,
            Description = "Overall effect opacity",
            Order = 7
        }
    ];

    public IReadOnlyList<EffectPreset<MatrixRainConfig>> GetPresets() =>
    [
        new()
        {
            Name = "Classic",
            Description = "Iconic Matrix green code rain",
            Config = new() { Characters = MatrixRainPresets.Classic, Color = "#00ff41", FadeColor = "#003b00" },
            PreviewGradient = "linear-gradient(90deg, #003b00, #00ff41, #003b00)"
        },
        new()
        {
            Name = "Cyberpunk",
            Description = "Neon magenta with Katakana characters",
            Config = new() { Characters = MatrixRainPresets.Katakana, Color = "#ff00ff", FadeColor = "#1a0033" },
            PreviewGradient = "linear-gradient(90deg, #1a0033, #ff00ff, #1a0033)"
        },
        new()
        {
            Name = "Terminal",
            Description = "Hacker terminal green with binary",
            Config = new() { Characters = MatrixRainPresets.Binary, Color = "#00ff88", FadeColor = "#001a0d" },
            PreviewGradient = "linear-gradient(90deg, #001a0d, #00ff88, #001a0d)"
        },
        new()
        {
            Name = "Ghost",
            Description = "Ethereal white on black",
            Config = new() { Characters = MatrixRainPresets.Classic, Color = "#ffffff", FadeColor = "#0a0a0a" },
            PreviewGradient = "linear-gradient(90deg, #0a0a0a, #ffffff, #0a0a0a)"
        },
        new()
        {
            Name = "Sunset",
            Description = "Warm amber hex code rain",
            Config = new() { Characters = MatrixRainPresets.Hex, Color = "#ff6b35", FadeColor = "#1a0800" },
            PreviewGradient = "linear-gradient(90deg, #1a0800, #ff6b35, #1a0800)"
        }
    ];

    public MatrixRainConfig ApplyParameter(MatrixRainConfig config, string propertyName, object? value)
    {
        return propertyName switch
        {
            nameof(MatrixRainConfig.Characters) => config with { Characters = value?.ToString() ?? MatrixRainPresets.Classic },
            nameof(MatrixRainConfig.FontSize) => config with { FontSize = Convert.ToDouble(value) },
            nameof(MatrixRainConfig.FontFamily) => config with { FontFamily = value?.ToString() ?? "monospace" },
            nameof(MatrixRainConfig.Color) => config with { Color = value?.ToString() ?? "#00ff41" },
            nameof(MatrixRainConfig.FadeColor) => config with { FadeColor = value?.ToString() ?? "#003b00" },
            nameof(MatrixRainConfig.Speed) => config with { Speed = Convert.ToDouble(value) },
            nameof(MatrixRainConfig.Density) => config with { Density = Convert.ToDouble(value) },
            nameof(MatrixRainConfig.Opacity) => config with { Opacity = Convert.ToDouble(value) },
            _ => config
        };
    }
}
