using BlazorEffects.Core.Animation;

namespace BlazorEffects.Noise;

/// <summary>
/// Describes the Noise Field effect's parameters and presets for the playground UI.
/// </summary>
public sealed class NoiseFieldDescriptor : IEffectDescriptor<NoiseFieldConfig>
{
    public Type ComponentType => typeof(NoiseField);

    public string EffectName => "Noise Field";

    public IReadOnlyList<EffectParameterDefinition> GetParameterDefinitions() =>
    [
        new()
        {
            Name = "Color Palette",
            PropertyName = nameof(NoiseFieldConfig.ColorStops),
            Type = EffectParameterType.ColorArray,
            DefaultValue = NoiseFieldPresets.Default,
            Description = "Gradient color palette for mapping noise values",
            Order = 0
        },
        new()
        {
            Name = "Noise Scale",
            PropertyName = nameof(NoiseFieldConfig.NoiseScale),
            Type = EffectParameterType.Range,
            DefaultValue = 0.003,
            MinValue = 0.0001,
            MaxValue = 0.05,
            Step = 0.0005,
            Description = "Zoom level of noise (lower = smoother, larger features)",
            Order = 1
        },
        new()
        {
            Name = "Speed",
            PropertyName = nameof(NoiseFieldConfig.Speed),
            Type = EffectParameterType.Range,
            DefaultValue = 0.005,
            MinValue = 0.0,
            MaxValue = 0.02,
            Step = 0.001,
            Description = "Evolution speed of the noise animation",
            Order = 2
        },
        new()
        {
            Name = "Octaves",
            PropertyName = nameof(NoiseFieldConfig.Octaves),
            Type = EffectParameterType.Integer,
            DefaultValue = 4,
            MinValue = 1,
            MaxValue = 8,
            Step = 1,
            Description = "Number of noise layers (fractal Brownian motion)",
            Order = 3
        },
        new()
        {
            Name = "Persistence",
            PropertyName = nameof(NoiseFieldConfig.Persistence),
            Type = EffectParameterType.Range,
            DefaultValue = 0.5,
            MinValue = 0.0,
            MaxValue = 1.0,
            Step = 0.05,
            Description = "Amplitude multiplier per octave",
            Order = 4
        },
        new()
        {
            Name = "Lacunarity",
            PropertyName = nameof(NoiseFieldConfig.Lacunarity),
            Type = EffectParameterType.Range,
            DefaultValue = 2.0,
            MinValue = 1.0,
            MaxValue = 4.0,
            Step = 0.1,
            Description = "Frequency multiplier per octave",
            Order = 5
        },
        new()
        {
            Name = "Brightness",
            PropertyName = nameof(NoiseFieldConfig.Brightness),
            Type = EffectParameterType.Range,
            DefaultValue = 1.0,
            MinValue = 0.0,
            MaxValue = 3.0,
            Step = 0.1,
            Description = "Color brightness multiplier",
            Order = 6
        },
        new()
        {
            Name = "Opacity",
            PropertyName = nameof(NoiseFieldConfig.Opacity),
            Type = EffectParameterType.Range,
            DefaultValue = 0.8,
            MinValue = 0.0,
            MaxValue = 1.0,
            Step = 0.05,
            Description = "Overall effect opacity",
            Order = 7
        }
    ];

    public IReadOnlyList<EffectPreset<NoiseFieldConfig>> GetPresets() =>
    [
        new() { Name = "Default", Description = "Deep indigo to violet to purple to pink to rose", Config = new() { ColorStops = NoiseFieldPresets.Default }, PreviewGradient = "linear-gradient(90deg, #0a0a2e, #1e1b4b, #6366f1, #8b5cf6, #c084fc, #ec4899, #f43f5e, #1e1b4b, #0a0a2e)" },
        new() { Name = "Aurora", Description = "Deep space to emerald to teal to violet", Config = new() { ColorStops = NoiseFieldPresets.Aurora }, PreviewGradient = "linear-gradient(90deg, #020617, #0c1445, #0f4c3a, #10b981, #14b8a6, #22d3ee, #8b5cf6, #0c1445, #020617)" },
        new() { Name = "Sunset", Description = "Deep plum to coral to amber to violet", Config = new() { ColorStops = NoiseFieldPresets.Sunset }, PreviewGradient = "linear-gradient(90deg, #1a0a2e, #4c1d95, #f97316, #fb923c, #f43f5e, #e879f9, #a855f7, #4c1d95, #1a0a2e)" },
        new() { Name = "Ocean", Description = "Deep abyss to navy to teal to cyan to light blue", Config = new() { ColorStops = NoiseFieldPresets.Ocean }, PreviewGradient = "linear-gradient(90deg, #020617, #0a1628, #1e3a5f, #1e40af, #0d9488, #06b6d4, #22d3ee, #67e8f9, #0a1628, #020617)" },
        new() { Name = "Lava", Description = "Deep black to crimson to orange to gold", Config = new() { ColorStops = NoiseFieldPresets.Lava }, PreviewGradient = "linear-gradient(90deg, #0a0000, #1c0a00, #7f1d1d, #991b1b, #dc2626, #f97316, #facc15, #991b1b, #1c0a00, #0a0000)" },
        new() { Name = "Monochrome", Description = "Black to white grayscale", Config = new() { ColorStops = NoiseFieldPresets.Monochrome }, PreviewGradient = "linear-gradient(90deg, #000000, #6b7280, #ffffff, #6b7280, #000000)" },
        new() { Name = "Neon", Description = "Dark to electric pink to cyan to violet", Config = new() { ColorStops = NoiseFieldPresets.Neon }, PreviewGradient = "linear-gradient(90deg, #05051a, #0a0a2e, #ec4899, #f472b6, #06b6d4, #22d3ee, #a855f7, #c084fc, #0a0a2e, #05051a)" }
    ];

    public NoiseFieldConfig ApplyParameter(NoiseFieldConfig config, string propertyName, object? value)
    {
        return propertyName switch
        {
            nameof(NoiseFieldConfig.ColorStops) => config with { ColorStops = value as string[] ?? NoiseFieldPresets.Default },
            nameof(NoiseFieldConfig.NoiseScale) => config with { NoiseScale = Convert.ToDouble(value) },
            nameof(NoiseFieldConfig.Speed) => config with { Speed = Convert.ToDouble(value) },
            nameof(NoiseFieldConfig.Octaves) => config with { Octaves = Convert.ToInt32(value) },
            nameof(NoiseFieldConfig.Persistence) => config with { Persistence = Convert.ToDouble(value) },
            nameof(NoiseFieldConfig.Lacunarity) => config with { Lacunarity = Convert.ToDouble(value) },
            nameof(NoiseFieldConfig.Brightness) => config with { Brightness = Convert.ToDouble(value) },
            nameof(NoiseFieldConfig.Opacity) => config with { Opacity = Convert.ToDouble(value) },
            _ => config
        };
    }
}
