namespace BlazorEffects.Noise;

/// <summary>
/// Common color palette presets for the Noise Field effect.
/// </summary>
public static class NoiseFieldPresets
{
    /// <summary>
    /// Default palette: deep indigo to violet to purple to pink to rose, cycling back.
    /// Rich multi-stop gradient for flowing, organic textures.
    /// </summary>
    public static string[] Default => ["#0a0a2e", "#1e1b4b", "#6366f1", "#8b5cf6", "#c084fc", "#ec4899", "#f43f5e", "#1e1b4b", "#0a0a2e"];

    /// <summary>
    /// Aurora: deep space to emerald to teal to violet.
    /// </summary>
    public static string[] Aurora => ["#020617", "#0c1445", "#0f4c3a", "#10b981", "#14b8a6", "#22d3ee", "#8b5cf6", "#0c1445", "#020617"];

    /// <summary>
    /// Sunset: deep plum to coral to amber to violet.
    /// </summary>
    public static string[] Sunset => ["#1a0a2e", "#4c1d95", "#f97316", "#fb923c", "#f43f5e", "#e879f9", "#a855f7", "#4c1d95", "#1a0a2e"];

    /// <summary>
    /// Ocean: deep abyss to navy to teal to cyan to light blue.
    /// </summary>
    public static string[] Ocean => ["#020617", "#0a1628", "#1e3a5f", "#1e40af", "#0d9488", "#06b6d4", "#22d3ee", "#67e8f9", "#0a1628", "#020617"];

    /// <summary>
    /// Lava: deep black to crimson to orange to gold.
    /// </summary>
    public static string[] Lava => ["#0a0000", "#1c0a00", "#7f1d1d", "#991b1b", "#dc2626", "#f97316", "#facc15", "#991b1b", "#1c0a00", "#0a0000"];

    /// <summary>
    /// Monochrome: black to white grayscale.
    /// </summary>
    public static string[] Monochrome => ["#000000", "#1e1e1e", "#6b7280", "#d1d5db", "#ffffff", "#d1d5db", "#1e1e1e", "#000000"];

    /// <summary>
    /// Neon: dark to electric pink to cyan to violet.
    /// </summary>
    public static string[] Neon => ["#05051a", "#0a0a2e", "#ec4899", "#f472b6", "#06b6d4", "#22d3ee", "#a855f7", "#c084fc", "#0a0a2e", "#05051a"];
}
