using BlazorEffects.Core.Animation;

namespace BlazorEffects.VortexTunnel;

/// <summary>
/// Describes the VortexTunnel effect's parameters and presets for the playground UI.
/// </summary>
public sealed class VortexTunnelDescriptor : IEffectDescriptor<VortexTunnelConfig>
{
    public Type ComponentType => typeof(VortexTunnel);

    public string EffectName => "VortexTunnel";

    public IReadOnlyList<EffectParameterDefinition> GetParameterDefinitions() =>
    [
        new()
        {
            Name = "Ring Count",
            PropertyName = nameof(VortexTunnelConfig.RingCount),
            Type = EffectParameterType.Integer,
            DefaultValue = 20,
            MinValue = 5,
            MaxValue = 30,
            Step = 1,
            Description = "Number of concentric rings",
            Order = 0
        },
        new()
        {
            Name = "Rotation Speed",
            PropertyName = nameof(VortexTunnelConfig.RotationSpeed),
            Type = EffectParameterType.Range,
            DefaultValue = 0.02,
            MinValue = 0.005,
            MaxValue = 0.1,
            Step = 0.005,
            Description = "Rotation speed in radians per frame",
            Order = 1
        },
        new()
        {
            Name = "Color",
            PropertyName = nameof(VortexTunnelConfig.Color),
            Type = EffectParameterType.Color,
            DefaultValue = "#8b5cf6",
            Description = "Primary ring color",
            Order = 2
        },
        new()
        {
            Name = "Scale Factor",
            PropertyName = nameof(VortexTunnelConfig.ScaleFactor),
            Type = EffectParameterType.Range,
            DefaultValue = 0.92,
            MinValue = 0.75,
            MaxValue = 0.98,
            Step = 0.01,
            Description = "Scale factor between successive rings",
            Order = 3
        },
        new()
        {
            Name = "Shape",
            PropertyName = nameof(VortexTunnelConfig.Shape),
            Type = EffectParameterType.Text,
            DefaultValue = "circle",
            Description = "Ring shape: circle, polygon, or square",
            Order = 4
        },
        new()
        {
            Name = "Polygon Sides",
            PropertyName = nameof(VortexTunnelConfig.PolygonSides),
            Type = EffectParameterType.Integer,
            DefaultValue = 6,
            MinValue = 3,
            MaxValue = 12,
            Step = 1,
            Description = "Number of sides when shape is polygon",
            Order = 5
        },
        new()
        {
            Name = "Line Width",
            PropertyName = nameof(VortexTunnelConfig.LineWidth),
            Type = EffectParameterType.Range,
            DefaultValue = 2.0,
            MinValue = 0.5,
            MaxValue = 6.0,
            Step = 0.5,
            Description = "Base stroke width for outermost ring",
            Order = 6
        },
        new()
        {
            Name = "Background Color",
            PropertyName = nameof(VortexTunnelConfig.BackgroundColor),
            Type = EffectParameterType.Color,
            DefaultValue = "#030712",
            Description = "Background color",
            Order = 7
        },
        new()
        {
            Name = "Opacity",
            PropertyName = nameof(VortexTunnelConfig.Opacity),
            Type = EffectParameterType.Range,
            DefaultValue = 1.0,
            MinValue = 0.1,
            MaxValue = 1.0,
            Step = 0.05,
            Description = "Overall effect opacity",
            Order = 8
        }
    ];

    public IReadOnlyList<EffectPreset<VortexTunnelConfig>> GetPresets() =>
    [
        new()
        {
            Name = "Purple Vortex",
            Description = "Hypnotic purple spiraling tunnel",
            Config = VortexTunnelPresets.Default,
            PreviewGradient = "linear-gradient(90deg, #030712, #8b5cf6, #3b0764)"
        },
        new()
        {
            Name = "Hypno Wheel",
            Description = "Multi-colored psychedelic rings",
            Config = VortexTunnelPresets.HypnoWheel,
            PreviewGradient = "linear-gradient(90deg, #ef4444, #eab308, #22c55e, #3b82f6, #8b5cf6)"
        },
        new()
        {
            Name = "Deep Space",
            Description = "Dark blue tunnel with subtle rotation",
            Config = VortexTunnelPresets.DeepSpace,
            PreviewGradient = "linear-gradient(90deg, #000510, #1e40af, #000510)"
        },
        new()
        {
            Name = "Cyber Grid",
            Description = "Digital square tunnel matrix",
            Config = VortexTunnelPresets.CyberGrid,
            PreviewGradient = "linear-gradient(90deg, #0a0a0a, #00ff41, #003a00)"
        },
        new()
        {
            Name = "Warm Portal",
            Description = "Orange-red spinning vortex",
            Config = VortexTunnelPresets.WarmPortal,
            PreviewGradient = "linear-gradient(90deg, #1a0500, #f59e0b, #dc2626)"
        },
        new()
        {
            Name = "Ice Crystal",
            Description = "Light blue hexagonal vortex",
            Config = VortexTunnelPresets.IceCrystal,
            PreviewGradient = "linear-gradient(90deg, #020617, #67e8f9, #083344)"
        }
    ];

    public VortexTunnelConfig ApplyParameter(VortexTunnelConfig config, string propertyName, object? value)
    {
        return propertyName switch
        {
            nameof(VortexTunnelConfig.RingCount) => config with { RingCount = Convert.ToInt32(value) },
            nameof(VortexTunnelConfig.RotationSpeed) => config with { RotationSpeed = Convert.ToDouble(value) },
            nameof(VortexTunnelConfig.Color) => config with { Color = value?.ToString() ?? "#8b5cf6" },
            nameof(VortexTunnelConfig.ScaleFactor) => config with { ScaleFactor = Convert.ToDouble(value) },
            nameof(VortexTunnelConfig.Shape) => config with { Shape = value?.ToString() ?? "circle" },
            nameof(VortexTunnelConfig.PolygonSides) => config with { PolygonSides = Convert.ToInt32(value) },
            nameof(VortexTunnelConfig.LineWidth) => config with { LineWidth = Convert.ToDouble(value) },
            nameof(VortexTunnelConfig.BackgroundColor) => config with { BackgroundColor = value?.ToString() ?? "#030712" },
            nameof(VortexTunnelConfig.Opacity) => config with { Opacity = Convert.ToDouble(value) },
            _ => config
        };
    }
}
