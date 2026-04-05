namespace AppHost.Shared;

/// <summary>
/// A preset option displayed as a pill button in the demo control panel.
/// </summary>
public record PresetOption(string Key, string Label, string? Icon = null);

/// <summary>
/// A titled group of parameter definitions for the control panel.
/// </summary>
public record ParameterGroup(string Title, List<ParameterDef> Parameters);

/// <summary>
/// Base record for all parameter definitions.
/// </summary>
public abstract record ParameterDef(string Name, string Label);

/// <summary>
/// A color picker parameter.
/// </summary>
public record ColorParameter(string Name, string Label, string Value)
    : ParameterDef(Name, Label);

/// <summary>
/// A range slider parameter.
/// </summary>
public record RangeParameter(
    string Name,
    string Label,
    double Value,
    double Min,
    double Max,
    double Step,
    string Format = "F1")
    : ParameterDef(Name, Label);

/// <summary>
/// A dropdown select parameter.
/// </summary>
public record SelectParameter(
    string Name,
    string Label,
    string Value,
    List<SelectOption> Options)
    : ParameterDef(Name, Label);

/// <summary>
/// An option within a <see cref="SelectParameter"/>.
/// </summary>
public record SelectOption(string Value, string Label);
