using BlazorEffects.Core.Animation;

namespace BlazorEffects.Particles;

/// <summary>
/// Configuration object passed to the Particle Constellation JS module.
/// </summary>
public sealed record ParticleConstellationConfig : IEffectConfig
{
    public int ParticleCount { get; init; } = 150;
    public string ParticleColor { get; init; } = "#6366f1";
    public double ParticleSize { get; init; } = 2;
    public double ConnectionDistance { get; init; } = 120;
    public string ConnectionColor { get; init; } = "#6366f1";
    public double Speed { get; init; } = 0.5;
    public bool MouseInteraction { get; init; } = true;
    public double MouseRadius { get; init; } = 150;
    public string MouseForce { get; init; } = "attract";
    public double Opacity { get; init; } = 0.6;
    /// <summary>
    /// How to respond to prefers-reduced-motion. Default: Minimal.
    /// </summary>
    public string ReducedMotionBehavior { get; init; } = "Minimal";

    /// <summary>
    /// Target frames per second. Clamped 1-120.
    /// </summary>
    public int TargetFps { get; init; } = 60;
}
