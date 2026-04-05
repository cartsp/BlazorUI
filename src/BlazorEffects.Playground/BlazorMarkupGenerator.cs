using System.Reflection;
using System.Text;

namespace BlazorEffects.Playground;

/// <summary>
/// Generates Blazor component markup from a config object using reflection.
/// Produces clean, formatted component tags with all non-default property values.
/// </summary>
public static class BlazorMarkupGenerator
{
    /// <summary>
    /// Generate a Blazor component tag from a config object.
    /// Only includes properties whose values differ from the default instance.
    /// </summary>
    public static string Generate<TConfig>(TConfig config, string componentTagName) where TConfig : IEffectConfig
    {
        var defaults = Activator.CreateInstance<TConfig>()!;
        var configType = typeof(TConfig);
        var sb = new StringBuilder();

        var props = configType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.Name != "TargetFps")
            .ToList();

        // Check if all values are defaults
        var hasCustomValues = props.Any(p => !ValuesEqual(p.GetValue(config), p.GetValue(defaults), p.PropertyType));

        if (!hasCustomValues)
        {
            sb.Append($"<{componentTagName} />");
            return sb.ToString();
        }

        sb.AppendLine($"<{componentTagName}");

        foreach (var prop in props.OrderBy(p => p.Name))
        {
            var configVal = prop.GetValue(config);
            var defaultVal = prop.GetValue(defaults);

            if (configVal is null && defaultVal is null) continue;
            if (ValuesEqual(configVal, defaultVal, prop.PropertyType)) continue;

            var formattedValue = FormatValue(configVal, prop.PropertyType);
            sb.AppendLine($"    {prop.Name}=\"{formattedValue}\"");
        }

        sb.Append(" />");
        return sb.ToString();
    }

    private static bool ValuesEqual(object? a, object? b, Type type)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;

        // Use sequence comparison for arrays (reference equality is wrong for string[])
        if (type == typeof(string[]))
            return ((string[])a).SequenceEqual((string[])b);

        return a.Equals(b);
    }

    private static string FormatValue(object? value, Type type)
    {
        if (value is null) return "null";

        if (type == typeof(string[]))
        {
            var arr = (string[])value;
            var items = string.Join(", ", arr.Select(s => $"\"{s}\""));
            return $"@(new[] {{ {items} }})";
        }

        if (type == typeof(double))
        {
            var d = (double)value;
            if (d == Math.Floor(d) && Math.Abs(d) < 1e15)
                return ((int)d).ToString();
            return d.ToString("G");
        }

        if (type == typeof(bool))
            return value.ToString()!.ToLowerInvariant();

        return value.ToString()!;
    }
}
