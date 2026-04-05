using System.Reflection;

namespace BlazorEffects.Playground;

/// <summary>
/// Generic adapter that wraps a typed <see cref="IEffectDescriptor{TConfig}"/>
/// into the non-generic <see cref="IEffectDescriptorAdapter"/> interface.
/// </summary>
public sealed class EffectDescriptorAdapter<TConfig> : IEffectDescriptorAdapter
    where TConfig : IEffectConfig
{
    private readonly IEffectDescriptor<TConfig> _descriptor;
    private readonly Func<TConfig, string> _markupGenerator;

    public EffectDescriptorAdapter(
        IEffectDescriptor<TConfig> descriptor,
        Func<TConfig, string> markupGenerator)
    {
        _descriptor = descriptor;
        _markupGenerator = markupGenerator;
    }

    public string EffectName => _descriptor.EffectName;
    public Type ComponentType => _descriptor.ComponentType;

    public IReadOnlyList<EffectParameterDefinition> GetParameterDefinitions()
        => _descriptor.GetParameterDefinitions();

    public IReadOnlyList<EffectPresetView> GetPresets()
        => _descriptor.GetPresets()
            .Select(p => new EffectPresetView
            {
                Name = p.Name,
                Description = p.Description,
                PreviewGradient = p.PreviewGradient,
                Config = p.Config
            })
            .ToList();

    public object GetDefaultConfig()
    {
        // Create a default config instance using the record's default constructor
        return Activator.CreateInstance<TConfig>()!;
    }

    public object ApplyParameter(object config, string propertyName, object? value)
        => _descriptor.ApplyParameter((TConfig)config, propertyName, value);

    public string GenerateBlazorMarkup(object config)
        => _markupGenerator((TConfig)config);

    public IDictionary<string, object?> BuildComponentParameters(object config)
    {
        var typedConfig = (TConfig)config;
        var parameters = new Dictionary<string, object?>();
        var configType = typeof(TConfig);

        foreach (var prop in configType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            // Skip TargetFps — that's infrastructure, not a UI parameter
            if (prop.Name == "TargetFps") continue;

            var value = prop.GetValue(typedConfig);
            if (value is not null)
            {
                parameters[prop.Name] = value;
            }
        }

        return parameters;
    }
}
