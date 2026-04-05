namespace BlazorEffects.Core.Animation;

/// <summary>
/// Provides parameter definitions and presets for an effect component.
/// Implement this on each component's config type to enable the playground UI.
/// </summary>
/// <typeparam name="TConfig">The effect config type.</typeparam>
public interface IEffectDescriptor<TConfig> where TConfig : IEffectConfig
{
    /// <summary>
    /// All parameter definitions for this effect (drives the playground controls).
    /// </summary>
    IReadOnlyList<EffectParameterDefinition> GetParameterDefinitions();

    /// <summary>
    /// All available presets for this effect.
    /// </summary>
    IReadOnlyList<EffectPreset<TConfig>> GetPresets();

    /// <summary>
    /// The Blazor component type that renders this effect (for live preview).
    /// </summary>
    Type ComponentType { get; }

    /// <summary>
    /// Display name of the effect (for the playground UI).
    /// </summary>
    string EffectName { get; }

    /// <summary>
    /// Apply a single parameter value to a config, returning a new config.
    /// </summary>
    TConfig ApplyParameter(TConfig config, string propertyName, object? value);
}
