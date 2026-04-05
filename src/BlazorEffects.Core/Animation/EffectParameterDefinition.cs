namespace BlazorEffects.Core.Animation;

/// <summary>
/// Describes a single effect parameter for the playground UI.
/// </summary>
public sealed record EffectParameterDefinition
{
    /// <summary>
    /// Display name of the parameter.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Property name on the config object (for code generation).
    /// </summary>
    public required string PropertyName { get; init; }

    /// <summary>
    /// Parameter type — determines the UI control to render.
    /// </summary>
    public required EffectParameterType Type { get; init; }

    /// <summary>
    /// Default value (boxed). Used to reset and for code generation.
    /// </summary>
    public required object DefaultValue { get; init; }

    /// <summary>
    /// Minimum value (for numeric range parameters).
    /// </summary>
    public double? MinValue { get; init; }

    /// <summary>
    /// Maximum value (for numeric range parameters).
    /// </summary>
    public double? MaxValue { get; init; }

    /// <summary>
    /// Step increment (for numeric range parameters).
    /// </summary>
    public double? Step { get; init; }

    /// <summary>
    /// Human-readable description shown as a tooltip.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Display order (lower = higher in the UI).
    /// </summary>
    public int Order { get; init; }
}

/// <summary>
/// The type of an effect parameter, determining the UI control to render.
/// </summary>
public enum EffectParameterType
{
    /// <summary>Numeric range with slider control.</summary>
    Range,

    /// <summary>Integer value with number input.</summary>
    Integer,

    /// <summary>Boolean toggle switch.</summary>
    Toggle,

    /// <summary>Single color value with color picker.</summary>
    Color,

    /// <summary>Array of color values (gradient palette).</summary>
    ColorArray,

    /// <summary>Text string with text input.</summary>
    Text,

    /// <summary>Dropdown selection from a predefined list.</summary>
    Select
}
