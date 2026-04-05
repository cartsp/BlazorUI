using BlazorEffects.Core.Animation;

namespace BlazorEffects.Starfield;

/// <summary>
/// Configuration object passed to the Starfield JS module.
/// Stars fly toward the camera with warp-speed trails.
/// </summary>
public sealed record StarfieldConfig : IEffectConfig
{
    /// <summary>
    /// Number of stars in the field.
    /// </summary>
    public int StarCount { get; init; } = 800;

    /// <summary>
    /// Star fill color (hex).
    /// </summary>
    public string StarColor { get; init; } = "#ffffff";

    /// <summary>
    /// Maximum star radius in px.
    /// </summary>
    public double StarSize { get; init; } = 2.0;

    /// <summary>
    /// Warp speed multiplier (how fast stars approach).
    /// </summary>
    public double Speed { get; init; } = 2.0;

    /// <summary>
    /// Length of speed trail behind each star (0 = no trail, 1 = full trail).
    /// </summary>
    public double TrailLength { get; init; } = 0.6;

    /// <summary>
    /// Depth of the star field (how far away stars spawn).
    /// </summary>
    public double Depth { get; init; } = 1000;

    /// <summary>
    /// Background color (hex or CSS value).
    /// </summary>
    public string BackgroundColor { get; init; } = "#000000";

    /// <summary>
    /// Overall effect opacity (0.0–1.0).
    /// </summary>
    public double Opacity { get; init; } = 1.0;

    /// <summary>
    /// Target frames per second. Clamped 1-120.
    /// </summary>
    public int TargetFps { get; init; } = 60;
}
