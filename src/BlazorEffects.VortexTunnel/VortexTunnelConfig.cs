using BlazorEffects.Core.Animation;

namespace BlazorEffects.VortexTunnel;

/// <summary>
/// Configuration object passed to the VortexTunnel JS module.
/// Rotating concentric shapes scale toward the viewer, creating a spiraling tunnel.
/// </summary>
public sealed record VortexTunnelConfig : IEffectConfig
{
    /// <summary>
    /// Number of concentric rings in the tunnel. Capped for performance.
    /// </summary>
    public int RingCount { get; init; } = 20;

    /// <summary>
    /// Rotation speed (radians per frame).
    /// </summary>
    public double RotationSpeed { get; init; } = 0.02;

    /// <summary>
    /// Primary ring color (hex). Used when Colors is empty.
    /// </summary>
    public string Color { get; init; } = "#8b5cf6";

    /// <summary>
    /// Array of colors for multi-colored rings. Overrides Color when non-empty.
    /// </summary>
    public string[] Colors { get; init; } = [];

    /// <summary>
    /// Scale factor between successive rings (how much each ring shrinks).
    /// </summary>
    public double ScaleFactor { get; init; } = 0.92;

    /// <summary>
    /// Shape of each ring: "circle", "polygon", or "square".
    /// </summary>
    public string Shape { get; init; } = "circle";

    /// <summary>
    /// Number of sides when Shape is "polygon".
    /// </summary>
    public int PolygonSides { get; init; } = 6;

    /// <summary>
    /// Base stroke width for the outermost ring. Inner rings get thinner.
    /// </summary>
    public double LineWidth { get; init; } = 2.0;

    /// <summary>
    /// Background color (hex or CSS value).
    /// </summary>
    public string BackgroundColor { get; init; } = "#030712";

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
