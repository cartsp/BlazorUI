namespace BlazorEffects.FireEmbers;

/// <summary>
/// Common presets for the Fire/Embers effect.
/// </summary>
public static class FireEmbersPresets
{
    /// <summary>
    /// Default campfire — warm orange flames with golden embers.
    /// </summary>
    public static FireEmbersConfig Default => new();

    /// <summary>
    /// Bonfire — large, intense, many particles.
    /// </summary>
    public static FireEmbersConfig Bonfire => new()
    {
        ParticleCount = 400,
        FlameColor = "#ff4400",
        EmberColor = "#ffaa00",
        Speed = 2.0,
        ParticleSize = 5.0,
        Turbulence = 1.5,
        EmberRatio = 0.35
    };

    /// <summary>
    /// Candlelight — small, gentle, warm glow.
    /// </summary>
    public static FireEmbersConfig Candlelight => new()
    {
        ParticleCount = 60,
        FlameColor = "#ffaa33",
        EmberColor = "#ffdd88",
        Speed = 0.5,
        ParticleSize = 3.0,
        Turbulence = 0.4,
        EmberRatio = 0.15
    };

    /// <summary>
    /// Inferno — aggressive, fast, lots of sparks.
    /// </summary>
    public static FireEmbersConfig Inferno => new()
    {
        ParticleCount = 500,
        FlameColor = "#ff2200",
        EmberColor = "#ffff00",
        Speed = 3.0,
        ParticleSize = 4.5,
        Turbulence = 2.0,
        EmberRatio = 0.5,
        BackgroundColor = "#1a0000"
    };

    /// <summary>
    /// Blue flame — magical or gas burner aesthetic.
    /// </summary>
    public static FireEmbersConfig BlueFlame => new()
    {
        ParticleCount = 180,
        FlameColor = "#3b82f6",
        EmberColor = "#93c5fd",
        Speed = 1.2,
        ParticleSize = 3.5,
        Turbulence = 0.8,
        EmberRatio = 0.2,
        BackgroundColor = "#050510"
    };

    /// <summary>
    /// Embers only — sparse sparks rising from coals.
    /// </summary>
    public static FireEmbersConfig EmbersOnly => new()
    {
        ParticleCount = 100,
        FlameColor = "#cc3300",
        EmberColor = "#ff8800",
        Speed = 0.8,
        ParticleSize = 2.5,
        Turbulence = 0.6,
        EmberRatio = 1.0
    };
}
