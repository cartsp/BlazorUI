namespace BlazorEffects.Noise;

/// <summary>
/// Common color palette presets for the Noise Field effect.
/// </summary>
public static class NoiseFieldPresets
{
    /// <summary>
    /// Default palette: dark slate to indigo to purple to pink, cycling back.
    /// </summary>
    public static string[] Default => ["#0f172a", "#6366f1", "#a855f7", "#ec4899", "#0f172a"];

    /// <summary>
    /// Aurora: deep blues to greens to purples.
    /// </summary>
    public static string[] Aurora => ["#0c1445", "#1e3a5f", "#10b981", "#14b8a6", "#8b5cf6", "#0c1445"];

    /// <summary>
    /// Sunset: warm oranges to pinks to deep purples.
    /// </summary>
    public static string[] Sunset => ["#1a0a2e", "#f97316", "#f43f5e", "#a855f7", "#1a0a2e"];

    /// <summary>
    /// Ocean: dark blues to teals to light cyans.
    /// </summary>
    public static string[] Ocean => ["#0a1628", "#1e40af", "#0d9488", "#06b6d4", "#67e8f9", "#0a1628"];

    /// <summary>
    /// Lava: deep reds to oranges to yellows.
    /// </summary>
    public static string[] Lava => ["#1c0a00", "#991b1b", "#dc2626", "#f97316", "#facc15", "#1c0a00"];

    /// <summary>
    /// Monochrome: black to white grayscale.
    /// </summary>
    public static string[] Monochrome => ["#000000", "#1e1e1e", "#6b7280", "#d1d5db", "#ffffff", "#d1d5db", "#1e1e1e", "#000000"];

    /// <summary>
    /// Neon: dark to electric pink to cyan.
    /// </summary>
    public static string[] Neon => ["#0a0a1a", "#ec4899", "#06b6d4", "#a855f7", "#0a0a1a"];
}
