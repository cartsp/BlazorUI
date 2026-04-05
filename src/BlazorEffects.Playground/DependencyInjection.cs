using Microsoft.Extensions.DependencyInjection;

namespace BlazorEffects.Playground;

/// <summary>
/// Builder for registering effect descriptors with the playground.
/// </summary>
public sealed class PlaygroundBuilder
{
    internal List<Func<IServiceProvider, IEffectDescriptorAdapter>> AdapterFactories { get; } = [];

    /// <summary>
    /// Register a typed effect descriptor with a Blazor markup generator.
    /// </summary>
    public PlaygroundBuilder AddEffect<TConfig>(
        IEffectDescriptor<TConfig> descriptor,
        Func<TConfig, string> markupGenerator)
        where TConfig : IEffectConfig
    {
        AdapterFactories.Add(_ => new EffectDescriptorAdapter<TConfig>(descriptor, markupGenerator));
        return this;
    }

    /// <summary>
    /// Register a typed effect descriptor (resolved from DI) with a Blazor markup generator.
    /// </summary>
    public PlaygroundBuilder AddEffect<TConfig, TDescriptor>(
        Func<TConfig, string> markupGenerator)
        where TConfig : IEffectConfig
        where TDescriptor : IEffectDescriptor<TConfig>, new()
    {
        AdapterFactories.Add(_ => new EffectDescriptorAdapter<TConfig>(new TDescriptor(), markupGenerator));
        return this;
    }
}

/// <summary>
/// DI extension methods for the playground.
/// </summary>
public static class PlaygroundServiceExtensions
{
    /// <summary>
    /// Add the BlazorEffects Playground services. Use the builder to register effects.
    /// </summary>
    public static IServiceCollection AddBlazorEffectsPlayground(
        this IServiceCollection services,
        Action<PlaygroundBuilder> configure)
    {
        var builder = new PlaygroundBuilder();
        configure(builder);

        services.AddSingleton<IEffectRegistry>(sp =>
        {
            var registry = new EffectRegistry();
            foreach (var factory in builder.AdapterFactories)
            {
                registry.Register(factory(sp));
            }
            return registry;
        });

        return services;
    }
}
