namespace BlazorEffects.Playground;

/// <summary>
/// Non-generic adapter for <see cref="IEffectDescriptor{TConfig}"/> so the playground
/// UI can work with any effect without knowing the concrete config type.
/// </summary>
public interface IEffectDescriptorAdapter
{
    /// <summary>Display name of the effect.</summary>
    string EffectName { get; }

    /// <summary>The Blazor component type that renders this effect.</summary>
    Type ComponentType { get; }

    /// <summary>Parameter definitions for this effect.</summary>
    IReadOnlyList<EffectParameterDefinition> GetParameterDefinitions();

    /// <summary>Presets for this effect (non-generic view).</summary>
    IReadOnlyList<EffectPresetView> GetPresets();

    /// <summary>Get the default config as a boxed object.</summary>
    object GetDefaultConfig();

    /// <summary>Apply a single parameter value to a config, returning a new config.</summary>
    object ApplyParameter(object config, string propertyName, object? value);

    /// <summary>Generate Blazor markup for the given config state.</summary>
    string GenerateBlazorMarkup(object config);

    /// <summary>Build the parameters dictionary for DynamicComponent from a config.</summary>
    IDictionary<string, object?> BuildComponentParameters(object config);
}

/// <summary>
/// Non-generic view of an <see cref="EffectPreset{TConfig}"/>.
/// </summary>
public sealed record EffectPresetView
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public string? PreviewGradient { get; init; }
    public required object Config { get; init; }
}
