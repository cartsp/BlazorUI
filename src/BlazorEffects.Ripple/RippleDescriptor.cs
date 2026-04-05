using BlazorEffects.Core.Animation;

namespace BlazorEffects.Ripple;

/// <summary>
/// Describes the Ripple effect's parameters and presets for the playground UI.
/// </summary>
public sealed class RippleDescriptor : IEffectDescriptor<RippleConfig>
{
    public Type ComponentType => typeof(Ripple);

    public string EffectName => "Ripple";

    public IReadOnlyList<EffectParameterDefinition> GetParameterDefinitions() =>
    [
        new()
        {
            Name = "Max Ripples",
            PropertyName = nameof(RippleConfig.MaxRipples),
            Type = EffectParameterType.Integer,
            DefaultValue = 20,
            MinValue = 1,
            MaxValue = 40,
            Step = 1,
            Description = "Maximum number of active ripples",
            Order = 0
        },
        new()
        {
            Name = "Max Radius",
            PropertyName = nameof(RippleConfig.MaxRadius),
            Type = EffectParameterType.Range,
            DefaultValue = 300.0,
            MinValue = 50,
            MaxValue = 800,
            Step = 25,
            Description = "Maximum radius before ripple is removed",
            Order = 1
        },
        new()
        {
            Name = "Speed",
            PropertyName = nameof(RippleConfig.Speed),
            Type = EffectParameterType.Range,
            DefaultValue = 3.0,
            MinValue = 0.5,
            MaxValue = 10.0,
            Step = 0.5,
            Description = "Expansion speed in px per frame",
            Order = 2
        },
        new()
        {
            Name = "Color",
            PropertyName = nameof(RippleConfig.Color),
            Type = EffectParameterType.Color,
            DefaultValue = "#60a5fa",
            Description = "Ripple ring stroke color",
            Order = 3
        },
        new()
        {
            Name = "Line Width",
            PropertyName = nameof(RippleConfig.LineWidth),
            Type = EffectParameterType.Range,
            DefaultValue = 2.0,
            MinValue = 0.5,
            MaxValue = 8.0,
            Step = 0.5,
            Description = "Stroke width of ripple rings",
            Order = 4
        },
        new()
        {
            Name = "Decay",
            PropertyName = nameof(RippleConfig.Decay),
            Type = EffectParameterType.Range,
            DefaultValue = 0.02,
            MinValue = 0.005,
            MaxValue = 0.1,
            Step = 0.005,
            Description = "Opacity decay rate per frame",
            Order = 5
        },
        new()
        {
            Name = "Background Color",
            PropertyName = nameof(RippleConfig.BackgroundColor),
            Type = EffectParameterType.Color,
            DefaultValue = "#0f172a",
            Description = "Background color",
            Order = 6
        },
        new()
        {
            Name = "Opacity",
            PropertyName = nameof(RippleConfig.Opacity),
            Type = EffectParameterType.Range,
            DefaultValue = 1.0,
            MinValue = 0.1,
            MaxValue = 1.0,
            Step = 0.05,
            Description = "Overall effect opacity",
            Order = 7
        }
    ];

    public IReadOnlyList<EffectPreset<RippleConfig>> GetPresets() =>
    [
        new()
        {
            Name = "Water Surface",
            Description = "Calming blue ripples on dark water",
            Config = RipplePresets.Default,
            PreviewGradient = "linear-gradient(90deg, #0f172a, #60a5fa, #1e3a5f)"
        },
        new()
        {
            Name = "Neon Electric",
            Description = "Bright cyan sci-fi ripples",
            Config = RipplePresets.NeonElectric,
            PreviewGradient = "linear-gradient(90deg, #020617, #22d3ee, #083344)"
        },
        new()
        {
            Name = "Sunset",
            Description = "Warm coral sunset ripples",
            Config = RipplePresets.Sunset,
            PreviewGradient = "linear-gradient(90deg, #1c0a00, #fb923c, #7c2d12)"
        },
        new()
        {
            Name = "Minimal",
            Description = "Clean subtle white ripples",
            Config = RipplePresets.Minimal,
            PreviewGradient = "linear-gradient(90deg, #0f172a, #e2e8f0, #334155)"
        },
        new()
        {
            Name = "Rain Drops",
            Description = "Green rain impact simulation",
            Config = RipplePresets.RainDrops,
            PreviewGradient = "linear-gradient(90deg, #052e16, #4ade80, #14532d)"
        },
        new()
        {
            Name = "Interactive",
            Description = "Click-to-ripple purple effect",
            Config = RipplePresets.Interactive,
            PreviewGradient = "linear-gradient(90deg, #0c0a1f, #a78bfa, #3b0764)"
        }
    ];

    public RippleConfig ApplyParameter(RippleConfig config, string propertyName, object? value)
    {
        return propertyName switch
        {
            nameof(RippleConfig.MaxRipples) => config with { MaxRipples = Convert.ToInt32(value) },
            nameof(RippleConfig.MaxRadius) => config with { MaxRadius = Convert.ToDouble(value) },
            nameof(RippleConfig.Speed) => config with { Speed = Convert.ToDouble(value) },
            nameof(RippleConfig.Color) => config with { Color = value?.ToString() ?? "#60a5fa" },
            nameof(RippleConfig.LineWidth) => config with { LineWidth = Convert.ToDouble(value) },
            nameof(RippleConfig.Decay) => config with { Decay = Convert.ToDouble(value) },
            nameof(RippleConfig.BackgroundColor) => config with { BackgroundColor = value?.ToString() ?? "#0f172a" },
            nameof(RippleConfig.Opacity) => config with { Opacity = Convert.ToDouble(value) },
            _ => config
        };
    }
}
