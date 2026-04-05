using BlazorEffects.Core.Animation;

namespace BlazorEffects.Blobs;

/// <summary>
/// Configuration object passed to the Morphing Blobs JS module.
/// </summary>
public sealed record MorphingBlobsConfig : IEffectConfig
{
    public int BlobCount { get; init; } = 4;
    public string[] Colors { get; init; } = ["#6366f1", "#ec4899", "#f97316", "#06b6d4"];
    public double BlobSize { get; init; } = 300;
    public double Speed { get; init; } = 0.005;
    public double MorphIntensity { get; init; } = 80;
    public string BlendMode { get; init; } = "screen";
    public double Opacity { get; init; } = 0.7;
    /// <summary>
    /// How to respond to prefers-reduced-motion. Default: Minimal.
    /// </summary>
    public string ReducedMotionBehavior { get; init; } = "Minimal";

    /// <summary>
    /// Target frames per second. Clamped 1-120.
    /// </summary>
    public int TargetFps { get; init; } = 60;
}
