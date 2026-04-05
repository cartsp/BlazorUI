namespace BlazorEffects.Ripple;

/// <summary>
/// Common presets for the Ripple effect.
/// </summary>
public static class RipplePresets
{
    /// <summary>
    /// Default blue ripples on dark background — calming water surface.
    /// </summary>
    public static RippleConfig Default => new();

    /// <summary>
    /// Neon cyan ripples — electric sci-fi feel.
    /// </summary>
    public static RippleConfig NeonElectric => new()
    {
        Color = "#22d3ee",
        MaxRadius = 400,
        Speed = 4.0,
        LineWidth = 3.0,
        Decay = 0.015,
        BackgroundColor = "#020617",
        AutoRippleCount = 8,
        AutoInterval = 600
    };

    /// <summary>
    /// Warm sunset ripples — soft coral tones.
    /// </summary>
    public static RippleConfig Sunset => new()
    {
        Color = "#fb923c",
        MaxRadius = 250,
        Speed = 2.0,
        LineWidth = 2.5,
        Decay = 0.025,
        BackgroundColor = "#1c0a00",
        AutoRippleCount = 4,
        AutoInterval = 1000
    };

    /// <summary>
    /// Minimal white ripples — clean and subtle.
    /// </summary>
    public static RippleConfig Minimal => new()
    {
        Color = "#e2e8f0",
        MaxRadius = 200,
        Speed = 1.5,
        LineWidth = 1.0,
        Decay = 0.03,
        BackgroundColor = "#0f172a",
        AutoRippleCount = 3,
        AutoInterval = 1200
    };

    /// <summary>
    /// Rain drops — green tones simulating rain impact on water.
    /// </summary>
    public static RippleConfig RainDrops => new()
    {
        Color = "#4ade80",
        MaxRadius = 150,
        Speed = 2.5,
        LineWidth = 1.5,
        Decay = 0.035,
        BackgroundColor = "#052e16",
        AutoRippleCount = 12,
        AutoInterval = 300
    };

    /// <summary>
    /// Click-to-ripple mode — interactive purple ripples.
    /// </summary>
    public static RippleConfig Interactive => new()
    {
        Color = "#a78bfa",
        Trigger = "click",
        MaxRadius = 350,
        Speed = 3.5,
        LineWidth = 2.0,
        Decay = 0.018,
        BackgroundColor = "#0c0a1f",
        AutoRippleCount = 0
    };
}
