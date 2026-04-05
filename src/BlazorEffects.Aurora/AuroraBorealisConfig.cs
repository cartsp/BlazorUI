using BlazorEffects.Core.Animation;

namespace BlazorEffects.Aurora;

/// <summary>
/// Configuration object passed to the Aurora Borealis JS module.
/// </summary>
public sealed record AuroraBorealisConfig : IEffectConfig
{
    public string[] Colors { get; init; } = ["#00ff87", "#7b2ff7", "#00b4d8"];
    public int RibbonCount { get; init; } = 4;
    public double Amplitude { get; init; } = 120;
    public double Speed { get; init; } = 0.008;
    public double Opacity { get; init; } = 0.5;
    public string BlendMode { get; init; } = "screen";
    /// <summary>
    /// How to respond to prefers-reduced-motion. Default: Minimal.
    /// </summary>
    public string ReducedMotionBehavior { get; init; } = "Minimal";

    /// <summary>
    /// Target frames per second. Clamped 1-120.
    /// </summary>
    public int TargetFps { get; init; } = 60;
}
