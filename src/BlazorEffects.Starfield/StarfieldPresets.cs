namespace BlazorEffects.Starfield;

/// <summary>
/// Common presets for the Starfield effect.
/// </summary>
public static class StarfieldPresets
{
    /// <summary>
    /// Default white stars on black — classic hyperspace warp.
    /// </summary>
    public static StarfieldConfig Default => new();

    /// <summary>
    /// Blue-tinted stars — deep space FTL feel.
    /// </summary>
    public static StarfieldConfig WarpDrive => new()
    {
        StarColor = "#60a5fa",
        StarCount = 1200,
        Speed = 4.0,
        TrailLength = 0.8,
        Depth = 1500
    };

    /// <summary>
    /// Warm gold stars — gentle, ambient drift.
    /// </summary>
    public static StarfieldConfig GoldenDrift => new()
    {
        StarColor = "#fbbf24",
        StarCount = 400,
        Speed = 0.8,
        TrailLength = 0.3,
        Depth = 800,
        StarSize = 2.5
    };

    /// <summary>
    /// Sparse bright stars — minimalist cosmos.
    /// </summary>
    public static StarfieldConfig Sparse => new()
    {
        StarCount = 150,
        StarColor = "#e2e8f0",
        Speed = 1.0,
        TrailLength = 0.2,
        Depth = 600,
        StarSize = 3.0
    };

    /// <summary>
    /// Dense blizzard — intense storm of stars.
    /// </summary>
    public static StarfieldConfig Blizzard => new()
    {
        StarCount = 2000,
        StarColor = "#bfdbfe",
        Speed = 5.0,
        TrailLength = 0.9,
        Depth = 2000,
        StarSize = 1.5
    };

    /// <summary>
    /// Neon green — retro terminal aesthetic.
    /// </summary>
    public static StarfieldConfig RetroTerminal => new()
    {
        StarColor = "#00ff41",
        StarCount = 600,
        Speed = 1.5,
        TrailLength = 0.5,
        BackgroundColor = "#0a0a0a",
        StarSize = 1.5
    };
}
