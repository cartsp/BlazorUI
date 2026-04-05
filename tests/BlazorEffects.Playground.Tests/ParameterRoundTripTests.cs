using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.Playground.Tests;

/// <summary>
/// Tests that simulate the full parameter round-trip:
/// Config → BuildComponentParameters → ParameterEditor value reading → ApplyParameter → rebuild
/// 
/// This mirrors the exact data flow in EffectPlayground.razor.
/// </summary>
public class ParameterRoundTripTests
{
    // Helper: simulates ParameterEditor.GetIntValue
    private static int GetIntValue(IDictionary<string, object?> configValues, EffectParameterDefinition def)
        => configValues.TryGetValue(def.PropertyName, out var val) && val is int i ? i
           : def.DefaultValue is int iv ? iv : 0;

    // Helper: simulates ParameterEditor.GetDoubleValue
    private static double GetDoubleValue(IDictionary<string, object?> configValues, EffectParameterDefinition def)
        => configValues.TryGetValue(def.PropertyName, out var val) && val is double d ? d
           : def.DefaultValue is double dv ? dv : 0;

    // Helper: simulates ParameterEditor.GetBoolValue
    private static bool GetBoolValue(IDictionary<string, object?> configValues, EffectParameterDefinition def)
        => configValues.TryGetValue(def.PropertyName, out var val) && val is bool b ? b
           : def.DefaultValue is bool bv && bv;

    // Helper: simulates ParameterEditor.GetStringValue
    private static string GetStringValue(IDictionary<string, object?> configValues, EffectParameterDefinition def)
        => configValues.TryGetValue(def.PropertyName, out var val) && val is string s ? s
           : def.DefaultValue?.ToString() ?? "";

    // Helper: simulates ParameterEditor.GetStringArrayValue
    private static string[] GetStringArrayValue(IDictionary<string, object?> configValues, EffectParameterDefinition def)
    {
        if (configValues.TryGetValue(def.PropertyName, out var val) && val is string[] arr)
            return arr;
        return def.DefaultValue is string[] darr ? darr : [];
    }

    /// <summary>
    /// Full round-trip test for every parameter of the test config.
    /// Simulates: default config → build params → read values → apply change → rebuild → read again
    /// </summary>
    [Fact]
    public void FullRoundTrip_AllParameters_ShouldUpdateCorrectly()
    {
        var adapter = CreateTestAdapter();
        var definitions = adapter.GetParameterDefinitions();

        // Step 1: Get default config and build parameters
        var config = adapter.GetDefaultConfig();
        var parameters = adapter.BuildComponentParameters(config);

        // Step 2: Verify all parameter values can be read from the initial parameters
        foreach (var def in definitions)
        {
            var value = ReadParameterValue(parameters, def);
            value.Should().NotBeNull($"parameter '{def.Name}' should have a non-null value in BuildComponentParameters");
        }

        // Step 3: For each parameter, apply a new value and verify the round-trip
        foreach (var def in definitions)
        {
            var newValue = CreateTestValue(def);
            var newConfig = adapter.ApplyParameter(config, def.PropertyName, newValue);
            var newParameters = adapter.BuildComponentParameters(newConfig);
            var readValue = ReadParameterValue(newParameters, def);

            readValue.Should().Be(newValue,
                $"after changing '{def.Name}' to {newValue}, the value should round-trip correctly. " +
                $"Expected type: {newValue?.GetType().Name ?? "null"}, " +
                $"Actual type: {readValue?.GetType().Name ?? "null"}, " +
                $"Actual value: {readValue}");
        }
    }

    /// <summary>
    /// Verify double parameters produce boxed doubles in BuildComponentParameters
    /// </summary>
    [Fact]
    public void RangeParameter_ShouldProduceBoxedDouble()
    {
        var adapter = CreateTestAdapter();
        var config = adapter.GetDefaultConfig();
        var parameters = adapter.BuildComponentParameters(config);
        var def = adapter.GetParameterDefinitions().First(d => d.PropertyName == "Speed");

        parameters.Should().ContainKey("Speed");
        var val = parameters["Speed"];
        val.Should().NotBeNull();
        val.Should().BeOfType<double>($"Speed should be double, not {val!.GetType().Name}");
    }

    /// <summary>
    /// Verify boolean parameters produce boxed bools
    /// </summary>
    [Fact]
    public void ToggleParameter_ShouldProduceBoxedBool()
    {
        var adapter = CreateTestAdapter();
        var config = adapter.GetDefaultConfig();
        var parameters = adapter.BuildComponentParameters(config);
        var def = adapter.GetParameterDefinitions().First(d => d.PropertyName == "Enabled");

        parameters.Should().ContainKey("Enabled");
        var val = parameters["Enabled"];
        val.Should().NotBeNull();
        val.Should().BeOfType<bool>($"Enabled should be bool, not {val!.GetType().Name}");
    }

    /// <summary>
    /// Verify string array parameters produce string arrays
    /// </summary>
    [Fact]
    public void ColorArrayParameter_ShouldProduceStringArray()
    {
        var adapter = CreateTestAdapter();
        var config = adapter.GetDefaultConfig();
        var parameters = adapter.BuildComponentParameters(config);

        parameters.Should().ContainKey("Tags");
        var val = parameters["Tags"];
        val.Should().NotBeNull();
        val.Should().BeOfType<string[]>();
    }

    /// <summary>
    /// Verify that GetDoubleValue can read the value produced by BuildComponentParameters
    /// </summary>
    [Fact]
    public void GetDoubleValue_ShouldReadBoxedDoubleFromConfigValues()
    {
        var adapter = CreateTestAdapter();
        var config = adapter.GetDefaultConfig();
        var parameters = adapter.BuildComponentParameters(config);
        var def = adapter.GetParameterDefinitions().First(d => d.PropertyName == "Speed");

        // This simulates exactly what ParameterEditor.GetDoubleValue does
        var value = GetDoubleValue(parameters, def);
        value.Should().Be(1.0);

        // Apply change and verify round-trip
        var newConfig = adapter.ApplyParameter(config, "Speed", 5.5);
        var newParams = adapter.BuildComponentParameters(newConfig);
        var newValue = GetDoubleValue(newParams, def);
        newValue.Should().Be(5.5);
    }

    /// <summary>
    /// Verify that GetBoolValue can read the value produced by BuildComponentParameters
    /// </summary>
    [Fact]
    public void GetBoolValue_ShouldReadBoxedBoolFromConfigValues()
    {
        var adapter = CreateTestAdapter();
        var config = adapter.GetDefaultConfig();
        var parameters = adapter.BuildComponentParameters(config);
        var def = adapter.GetParameterDefinitions().First(d => d.PropertyName == "Enabled");

        var value = GetBoolValue(parameters, def);
        value.Should().Be(true);

        var newConfig = adapter.ApplyParameter(config, "Enabled", false);
        var newParams = adapter.BuildComponentParameters(newConfig);
        var newValue = GetBoolValue(newParams, def);
        newValue.Should().Be(false);
    }

    /// <summary>
    /// Verify that GetStringValue can read the value produced by BuildComponentParameters
    /// </summary>
    [Fact]
    public void GetStringValue_ShouldReadStringFromConfigValues()
    {
        var adapter = CreateTestAdapter();
        var config = adapter.GetDefaultConfig();
        var parameters = adapter.BuildComponentParameters(config);
        var def = adapter.GetParameterDefinitions().First(d => d.PropertyName == "Name");

        var value = GetStringValue(parameters, def);
        value.Should().Be("default");

        var newConfig = adapter.ApplyParameter(config, "Name", "changed");
        var newParams = adapter.BuildComponentParameters(newConfig);
        var newValue = GetStringValue(newParams, def);
        newValue.Should().Be("changed");
    }

    /// <summary>
    /// Verify that GetStringArrayValue can read the value produced by BuildComponentParameters
    /// </summary>
    [Fact]
    public void GetStringArrayValue_ShouldReadStringArrayFromConfigValues()
    {
        var adapter = CreateTestAdapter();
        var config = adapter.GetDefaultConfig();
        var parameters = adapter.BuildComponentParameters(config);
        var def = adapter.GetParameterDefinitions().First(d => d.PropertyName == "Tags");

        var value = GetStringArrayValue(parameters, def);
        value.Should().Equal("tag1");

        var newTags = new[] { "a", "b", "c" };
        var newConfig = adapter.ApplyParameter(config, "Tags", newTags);
        var newParams = adapter.BuildComponentParameters(newConfig);
        var newValue = GetStringArrayValue(newParams, def);
        newValue.Should().Equal("a", "b", "c");
    }

    // --- Helpers ---

    private static IEffectDescriptorAdapter CreateTestAdapter()
    {
        return new EffectDescriptorAdapter<TestConfig>(
            new TestDescriptor(),
            c => BlazorMarkupGenerator.Generate(c, "TestEffect"));
    }

    private static object? ReadParameterValue(IDictionary<string, object?> parameters, EffectParameterDefinition def)
    {
        return def.Type switch
        {
            EffectParameterType.Range => GetDoubleValue(parameters, def),
            EffectParameterType.Integer => GetIntValue(parameters, def),
            EffectParameterType.Toggle => GetBoolValue(parameters, def),
            EffectParameterType.Color => GetStringValue(parameters, def),
            EffectParameterType.ColorArray => GetStringArrayValue(parameters, def),
            EffectParameterType.Text => GetStringValue(parameters, def),
            EffectParameterType.Select => GetStringValue(parameters, def),
            _ => null
        };
    }

    private static object CreateTestValue(EffectParameterDefinition def)
    {
        return def.Type switch
        {
            EffectParameterType.Range => def.MaxValue ?? 1.0,
            EffectParameterType.Integer => (int)(def.MaxValue ?? 10),
            EffectParameterType.Toggle => !(def.DefaultValue is bool b && b),
            EffectParameterType.Color => "#ff0000",
            EffectParameterType.ColorArray => new[] { "#ff0000", "#00ff00" },
            EffectParameterType.Text => "test-value",
            EffectParameterType.Select => "test",
            _ => throw new InvalidOperationException($"Unknown parameter type: {def.Type}")
        };
    }
}
