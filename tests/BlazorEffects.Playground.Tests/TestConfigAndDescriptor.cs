using BlazorEffects.Core.Animation;

namespace BlazorEffects.Playground.Tests;

/// <summary>
/// Simple test config used across test files. Mirrors real effect configs
/// with a mix of types: string, double, bool, string array, and TargetFps.
/// </summary>
public sealed record TestConfig : IEffectConfig
{
    public string Name { get; init; } = "default";
    public double Speed { get; init; } = 1.0;
    public bool Enabled { get; init; } = true;
    public string[] Tags { get; init; } = ["tag1"];
    public int TargetFps { get; init; } = 60;
}

/// <summary>
/// Test descriptor for <see cref="TestConfig"/>. Provides parameter definitions,
/// presets, and ApplyParameter logic so we can test the adapter layer in isolation.
/// </summary>
public sealed class TestDescriptor : IEffectDescriptor<TestConfig>
{
    public Type ComponentType => typeof(object); // Not a real component — just for testing

    public string EffectName => "Test Effect";

    public IReadOnlyList<EffectParameterDefinition> GetParameterDefinitions() =>
    [
        new()
        {
            Name = "Name",
            PropertyName = nameof(TestConfig.Name),
            Type = EffectParameterType.Text,
            DefaultValue = "default",
            Order = 0
        },
        new()
        {
            Name = "Speed",
            PropertyName = nameof(TestConfig.Speed),
            Type = EffectParameterType.Range,
            DefaultValue = 1.0,
            MinValue = 0.0,
            MaxValue = 10.0,
            Step = 0.1,
            Order = 1
        },
        new()
        {
            Name = "Enabled",
            PropertyName = nameof(TestConfig.Enabled),
            Type = EffectParameterType.Toggle,
            DefaultValue = true,
            Order = 2
        },
        new()
        {
            Name = "Tags",
            PropertyName = nameof(TestConfig.Tags),
            Type = EffectParameterType.ColorArray,
            DefaultValue = new[] { "tag1" },
            Order = 3
        }
    ];

    public IReadOnlyList<EffectPreset<TestConfig>> GetPresets() =>
    [
        new()
        {
            Name = "Preset A",
            Description = "First test preset",
            Config = new() { Name = "alpha", Speed = 2.0 }
        },
        new()
        {
            Name = "Preset B",
            Description = "Second test preset",
            PreviewGradient = "linear-gradient(90deg, #ff0000, #00ff00)",
            Config = new() { Name = "bravo", Speed = 5.0, Enabled = false }
        }
    ];

    public TestConfig ApplyParameter(TestConfig config, string propertyName, object? value)
    {
        return propertyName switch
        {
            nameof(TestConfig.Name) => config with { Name = value?.ToString() ?? "default" },
            nameof(TestConfig.Speed) => config with { Speed = Convert.ToDouble(value) },
            nameof(TestConfig.Enabled) => config with { Enabled = Convert.ToBoolean(value) },
            nameof(TestConfig.Tags) => config with { Tags = value as string[] ?? ["tag1"] },
            _ => config
        };
    }
}
