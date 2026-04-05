namespace BlazorEffects.Particles;

/// <summary>
/// Common presets for the Particle Constellation effect.
/// </summary>
public static class ParticleConstellationPresets
{
    /// <summary>
    /// Default indigo network — tech/SaaS aesthetic.
    /// </summary>
    public static ParticleConstellationConfig Default => new();

    /// <summary>
    /// Neon green on dark — cyberpunk feel.
    /// </summary>
    public static ParticleConstellationConfig Cyberpunk => new()
    {
        ParticleColor = "#00ff41",
        ConnectionColor = "#00ff41",
        ParticleSize = 1.5,
        ConnectionDistance = 100,
        Speed = 0.8,
        Opacity = 0.7
    };

    /// <summary>
    /// Cool blue constellation — deep space feel.
    /// </summary>
    public static ParticleConstellationConfig DeepSpace => new()
    {
        ParticleColor = "#38bdf8",
        ConnectionColor = "#1e40af",
        ParticleSize = 1.5,
        ConnectionDistance = 150,
        Speed = 0.3,
        ParticleCount = 200,
        Opacity = 0.5
    };

    /// <summary>
    /// Warm amber particles — organic warmth.
    /// </summary>
    public static ParticleConstellationConfig Amber => new()
    {
        ParticleColor = "#f59e0b",
        ConnectionColor = "#ea580c",
        ParticleSize = 2.5,
        ConnectionDistance = 130,
        Speed = 0.4,
        Opacity = 0.65
    };

    /// <summary>
    /// Sparse minimal — subtle, elegant background.
    /// </summary>
    public static ParticleConstellationConfig Minimal => new()
    {
        ParticleCount = 50,
        ParticleColor = "#94a3b8",
        ConnectionColor = "#475569",
        ParticleSize = 1,
        ConnectionDistance = 200,
        Speed = 0.2,
        Opacity = 0.3
    };

    /// <summary>
    /// Dense mesh — intense connectivity visual.
    /// </summary>
    public static ParticleConstellationConfig Dense => new()
    {
        ParticleCount = 300,
        ParticleColor = "#a855f7",
        ConnectionColor = "#7c3aed",
        ParticleSize = 1.5,
        ConnectionDistance = 80,
        Speed = 0.6,
        Opacity = 0.5
    };
}
