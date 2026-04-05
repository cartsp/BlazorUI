using BlazorEffects.Core.Animation;

namespace BlazorEffects.Noise;

/// <summary>
/// Configuration object passed to the Noise Field JS module.
/// </summary>
public sealed record NoiseFieldConfig : IEffectConfig
{
    public string[] ColorStops { get; init; } = ["#0a0a2e", "#1e1b4b", "#6366f1", "#8b5cf6", "#c084fc", "#ec4899", "#f43f5e", "#1e1b4b", "#0a0a2e"];
    public double NoiseScale { get; init; } = 0.003;
    public double Speed { get; init; } = 0.005;
    public int Octaves { get; init; } = 4;
    public double Persistence { get; init; } = 0.5;
    public double Lacunarity { get; init; } = 2.0;
    public double Brightness { get; init; } = 1.0;
    public double Opacity { get; init; } = 0.85;
    /// <summary>
    /// How to respond to prefers-reduced-motion. Default: Minimal.
    /// </summary>
    public string ReducedMotionBehavior { get; init; } = "Minimal";

    /// <summary>
    /// Target frames per second. Clamped 1-120.
    /// </summary>
    public int TargetFps { get; init; } = 60;
}
