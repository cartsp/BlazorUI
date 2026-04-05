namespace BlazorEffects.Core.Animation;

/// <summary>
/// Represents a named preset for an effect — a curated set of parameter values.
/// </summary>
/// <typeparam name="TConfig">The effect config type this preset applies to.</typeparam>
public sealed record EffectPreset<TConfig> where TConfig : IEffectConfig
{
    /// <summary>
    /// Display name of the preset (e.g., "Aurora", "Sunset").
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Short human-readable description of the preset's visual character.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// The effect config with all preset values applied.
    /// </summary>
    public required TConfig Config { get; init; }

    /// <summary>
    /// Optional preview thumbnail URL or CSS gradient for the preset gallery.
    /// </summary>
    public string? PreviewGradient { get; init; }
}
