namespace BlazorEffects.Playground;

/// <summary>
/// Registry of all available effect descriptors for the playground.
/// Register one per effect type during DI setup.
/// </summary>
public interface IEffectRegistry
{
    /// <summary>Get all registered effect adapters.</summary>
    IReadOnlyList<IEffectDescriptorAdapter> GetAll();

    /// <summary>Get an adapter by effect name.</summary>
    IEffectDescriptorAdapter GetByName(string effectName);
}

/// <summary>
/// Default in-memory registry.
/// </summary>
public sealed class EffectRegistry : IEffectRegistry
{
    private readonly List<IEffectDescriptorAdapter> _adapters = [];

    public void Register(IEffectDescriptorAdapter adapter)
        => _adapters.Add(adapter);

    public IReadOnlyList<IEffectDescriptorAdapter> GetAll() => _adapters.AsReadOnly();

    public IEffectDescriptorAdapter GetByName(string effectName)
        => _adapters.FirstOrDefault(a => a.EffectName == effectName)
            ?? throw new InvalidOperationException($"No effect registered with name '{effectName}'");
}
