using BlazorEffects.Core.Animation;

namespace BlazorEffects.Ripple;

/// <summary>
/// Configuration object passed to the Ripple JS module.
/// Concentric rings expand from click/center points with configurable decay.
/// </summary>
public sealed record RippleConfig : IEffectConfig
{
    /// <summary>
    /// Maximum number of active ripples. Capped for performance.
    /// </summary>
    public int MaxRipples { get; init; } = 20;

    /// <summary>
    /// Maximum radius each ripple can reach before being removed (px).
    /// </summary>
    public double MaxRadius { get; init; } = 300;

    /// <summary>
    /// Speed at which ripples expand (px per frame).
    /// </summary>
    public double Speed { get; init; } = 3.0;

    /// <summary>
    /// Ripple stroke color (hex).
    /// </summary>
    public string Color { get; init; } = "#60a5fa";

    /// <summary>
    /// Stroke width of each ripple ring (px).
    /// </summary>
    public double LineWidth { get; init; } = 2.0;

    /// <summary>
    /// Opacity decay rate per frame (0.0–1.0). Higher = faster fade.
    /// </summary>
    public double Decay { get; init; } = 0.02;

    /// <summary>
    /// Trigger mode: "auto" generates ripples automatically, "click" responds to clicks/touches.
    /// </summary>
    public string Trigger { get; init; } = "auto";

    /// <summary>
    /// Number of auto-generated ripples when in auto mode.
    /// </summary>
    public int AutoRippleCount { get; init; } = 5;

    /// <summary>
    /// Interval in ms between auto-generated ripples.
    /// </summary>
    public int AutoInterval { get; init; } = 800;

    /// <summary>
    /// Background color (hex or CSS value).
    /// </summary>
    public string BackgroundColor { get; init; } = "#0f172a";

    /// <summary>
    /// Overall effect opacity (0.0–1.0).
    /// </summary>
    public double Opacity { get; init; } = 1.0;

    /// <summary>
    /// How to respond to prefers-reduced-motion. Options: Pause, Minimal, Ignore.
    /// Default: Minimal.
    /// </summary>
    public string ReducedMotionBehavior { get; init; } = "Minimal";

    /// <summary>
    /// Target frames per second. Clamped 1-120.
    /// </summary>
    public int TargetFps { get; init; } = 60;
}
