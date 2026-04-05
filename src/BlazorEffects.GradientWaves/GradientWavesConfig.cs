namespace BlazorEffects.GradientWaves;

/// <summary>
/// Configuration object passed to the Gradient Waves JS module.
/// </summary>
public sealed record GradientWavesConfig : IEffectConfig
{
    public string[] Colors { get; init; } = GradientWavesPresets.Stripe;
    public int PointCount { get; init; } = 6;
    public double BlobSize { get; init; } = 0.5;
    public double Speed { get; init; } = 0.004;
    public double BlurAmount { get; init; } = 80;
    public string BlendMode { get; init; } = "normal";
    public double Opacity { get; init; } = 1.0;
    /// <summary>
    /// How to respond to prefers-reduced-motion. Default: Minimal.
    /// </summary>
    public string ReducedMotionBehavior { get; init; } = "Minimal";

    /// <summary>
    /// Target frames per second. Clamped 1-120.
    /// </summary>
    public int TargetFps { get; init; } = 60;
}
