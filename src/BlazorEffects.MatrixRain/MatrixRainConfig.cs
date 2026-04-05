using BlazorEffects.Core.Animation;

namespace BlazorEffects.MatrixRain;

/// <summary>
/// Configuration object passed to the Matrix Rain JS module.
/// </summary>
public sealed record MatrixRainConfig : IEffectConfig
{
    public string Characters { get; init; } = MatrixRainPresets.Classic;
    public double FontSize { get; init; } = 16;
    public string FontFamily { get; init; } = "monospace";
    public string Color { get; init; } = "#00ff41";
    public string FadeColor { get; init; } = "#003b00";
    public double Speed { get; init; } = 1.0;
    public double Density { get; init; } = 1.0;
    public double Opacity { get; init; } = 0.8;
    public int TargetFps { get; init; } = 30;
}
