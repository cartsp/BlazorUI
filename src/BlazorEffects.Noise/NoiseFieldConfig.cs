using BlazorEffects.Core.Animation;

namespace BlazorEffects.Noise;

/// <summary>
/// Configuration object passed to the Noise Field JS module.
/// </summary>
public sealed record NoiseFieldConfig : IEffectConfig
{
    public string[] ColorStops { get; init; } = ["#0f172a", "#6366f1", "#a855f7", "#ec4899", "#0f172a"];
    public double NoiseScale { get; init; } = 0.005;
    public double Speed { get; init; } = 0.003;
    public int Octaves { get; init; } = 3;
    public double Persistence { get; init; } = 0.5;
    public double Lacunarity { get; init; } = 2.0;
    public double Brightness { get; init; } = 1.0;
    public double Opacity { get; init; } = 0.8;
    public int TargetFps { get; init; } = 60;
}
