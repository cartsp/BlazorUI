using BlazorEffects.Core.Animation;

namespace BlazorEffects.FireEmbers;

/// <summary>
/// Configuration object passed to the Fire/Embers JS module.
/// Rising flame particles with glowing embers — campfire aesthetic.
/// </summary>
public sealed record FireEmbersConfig : IEffectConfig
{
    /// <summary>
    /// Number of flame particles.
    /// </summary>
    public int ParticleCount { get; init; } = 200;

    /// <summary>
    /// Base flame color (hex) — blends to transparent at top.
    /// </summary>
    public string FlameColor { get; init; } = "#ff6600";

    /// <summary>
    /// Ember spark color (hex).
    /// </summary>
    public string EmberColor { get; init; } = "#ffcc00";

    /// <summary>
    /// Particle rise speed multiplier.
    /// </summary>
    public double Speed { get; init; } = 1.5;

    /// <summary>
    /// Maximum particle size in px.
    /// </summary>
    public double ParticleSize { get; init; } = 4.0;

    /// <summary>
    /// Turbulence intensity (horizontal wobble).
    /// </summary>
    public double Turbulence { get; init; } = 1.0;

    /// <summary>
    /// Percentage of particles that are embers (sparks) vs flames (0.0–1.0).
    /// </summary>
    public double EmberRatio { get; init; } = 0.3;

    /// <summary>
    /// Background color (hex or CSS value).
    /// </summary>
    public string BackgroundColor { get; init; } = "#0a0a0a";

    /// <summary>
    /// Overall effect opacity (0.0–1.0).
    /// </summary>
    public double Opacity { get; init; } = 0.9;

    /// <summary>
    /// Target frames per second. Clamped 1-120.
    /// </summary>
    /// <summary>
    /// How to respond to prefers-reduced-motion. Default: Minimal.
    /// </summary>
    public string ReducedMotionBehavior { get; init; } = "Minimal";

    /// <summary>
    /// Target frames per second. Clamped 1-120.
    /// </summary>
    public int TargetFps { get; init; } = 60;
}
