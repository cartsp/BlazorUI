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
            DefaultValue = 0.005,
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
            DefaultValue = 0.003,
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
            DefaultValue = 3,
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
        new() { Name = "Default", Description = "Dark slate to indigo to purple to pink", Config = new() { ColorStops = NoiseFieldPresets.Default }, PreviewGradient = "linear-gradient(90deg, #0f172a, #6366f1, #a855f7, #ec4899, #0f172a)" },
        new() { Name = "Aurora", Description = "Deep blues to greens to purples", Config = new() { ColorStops = NoiseFieldPresets.Aurora }, PreviewGradient = "linear-gradient(90deg, #0c1445, #1e3a5f, #10b981, #14b8a6, #8b5cf6, #0c1445)" },
        new() { Name = "Sunset", Description = "Warm oranges to pinks to deep purples", Config = new() { ColorStops = NoiseFieldPresets.Sunset }, PreviewGradient = "linear-gradient(90deg, #1a0a2e, #f97316, #f43f5e, #a855f7, #1a0a2e)" },
        new() { Name = "Ocean", Description = "Dark blues to teals to light cyans", Config = new() { ColorStops = NoiseFieldPresets.Ocean }, PreviewGradient = "linear-gradient(90deg, #0a1628, #1e40af, #0d9488, #06b6d4, #67e8f9, #0a1628)" },
        new() { Name = "Lava", Description = "Deep reds to oranges to yellows", Config = new() { ColorStops = NoiseFieldPresets.Lava }, PreviewGradient = "linear-gradient(90deg, #1c0a00, #991b1b, #dc2626, #f97316, #facc15, #1c0a00)" },
        new() { Name = "Monochrome", Description = "Black to white grayscale", Config = new() { ColorStops = NoiseFieldPresets.Monochrome }, PreviewGradient = "linear-gradient(90deg, #000000, #6b7280, #ffffff, #6b7280, #000000)" },
        new() { Name = "Neon", Description = "Dark to electric pink to cyan", Config = new() { ColorStops = NoiseFieldPresets.Neon }, PreviewGradient = "linear-gradient(90deg, #0a0a1a, #ec4899, #06b6d4, #a855f7, #0a0a1a)" }
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
