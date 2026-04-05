using AwesomeAssertions;
using BlazorEffects.Core.Animation;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace BlazorEffects.Playground.Tests;

public class PlaygroundBuilderTests
{
    // --- AddEffect with descriptor instance ---

    [Fact]
    public void AddEffect_WithDescriptorInstance_ShouldRegisterAdapterFactory()
    {
        // Arrange
        var builder = new PlaygroundBuilder();
        var descriptor = new TestDescriptor();

        // Act
        builder.AddEffect(descriptor, config => $"<Test>{config.Name}</Test>");

        // Assert
        builder.AdapterFactories.Should().HaveCount(1);
    }

    [Fact]
    public void AddEffect_WithDescriptorInstance_ShouldReturnBuilderForChaining()
    {
        // Arrange
        var builder = new PlaygroundBuilder();
        var descriptor = new TestDescriptor();

        // Act
        var result = builder.AddEffect(descriptor, config => $"<Test>{config.Name}</Test>");

        // Assert
        result.Should().BeSameAs(builder);
    }

    // --- AddEffect with generic descriptor ---

    [Fact]
    public void AddEffect_WithGenericDescriptor_ShouldRegisterAdapterFactory()
    {
        // Arrange
        var builder = new PlaygroundBuilder();

        // Act
        builder.AddEffect<TestConfig, TestDescriptor>(config => $"<Test>{config.Name}</Test>");

        // Assert
        builder.AdapterFactories.Should().HaveCount(1);
    }

    // --- Multiple effects ---

    [Fact]
    public void AddEffect_WithMultipleEffects_ShouldRegisterAll()
    {
        // Arrange
        var builder = new PlaygroundBuilder();
        var descriptor = new TestDescriptor();

        // Act
        builder.AddEffect(descriptor, config => $"<A>{config.Name}</A>");
        builder.AddEffect<TestConfig, TestDescriptor>(config => $"<B>{config.Name}</B>");

        // Assert
        builder.AdapterFactories.Should().HaveCount(2);
    }

    [Fact]
    public void AddEffect_MultipleEffectsShouldNotConflict()
    {
        // Arrange
        var builder = new PlaygroundBuilder();
        var descriptor = new TestDescriptor();

        builder.AddEffect(descriptor, config => $"<A>{config.Name}</A>");
        builder.AddEffect<TestConfig, TestDescriptor>(config => $"<B>{config.Name}</B>");

        // Act
        using var sp = BuildServiceProvider(builder);
        var registry = sp.GetRequiredService<IEffectRegistry>();
        var all = registry.GetAll();

        // Assert — both factories create adapters with EffectName "Test Effect"
        all.Should().HaveCount(2);
    }

    // --- Factory produces correct adapter ---

    [Fact]
    public void AdapterFactory_ShouldProduceEffectDescriptorAdapter()
    {
        // Arrange
        var builder = new PlaygroundBuilder();
        var descriptor = new TestDescriptor();
        builder.AddEffect(descriptor, config => BlazorMarkupGenerator.Generate(config, "TestTag"));

        // Act
        using var sp = BuildServiceProvider(builder);
        var registry = sp.GetRequiredService<IEffectRegistry>();
        var adapter = registry.GetByName("Test Effect");

        // Assert
        adapter.Should().NotBeNull();
        adapter.EffectName.Should().Be("Test Effect");
    }

    [Fact]
    public void AdapterFactory_ShouldProduceWorkingMarkupGenerator()
    {
        // Arrange
        var builder = new PlaygroundBuilder();
        var descriptor = new TestDescriptor();
        builder.AddEffect(descriptor, config => BlazorMarkupGenerator.Generate(config, "TestTag"));

        // Act
        using var sp = BuildServiceProvider(builder);
        var registry = sp.GetRequiredService<IEffectRegistry>();
        var adapter = registry.GetByName("Test Effect");
        var markup = adapter.GenerateBlazorMarkup(new TestConfig { Name = "hello" });

        // Assert
        markup.Should().Contain("Name=\"hello\"");
    }

    // --- DI integration via PlaygroundServiceExtensions ---

    [Fact]
    public void AddBlazorEffectsPlayground_ShouldRegisterIEffectRegistry()
    {
        // Arrange
        var services = new ServiceCollection();
        var descriptor = new TestDescriptor();

        // Act
        services.AddBlazorEffectsPlayground(builder =>
        {
            builder.AddEffect(descriptor, config => BlazorMarkupGenerator.Generate(config, "TestTag"));
        });

        // Assert
        using var sp = services.BuildServiceProvider();
        var registry = sp.GetService<IEffectRegistry>();
        registry.Should().NotBeNull();
    }

    [Fact]
    public void AddBlazorEffectsPlayground_ShouldRegisterEffectsInRegistry()
    {
        // Arrange
        var services = new ServiceCollection();
        var descriptor = new TestDescriptor();

        // Act
        services.AddBlazorEffectsPlayground(builder =>
        {
            builder.AddEffect(descriptor, config => BlazorMarkupGenerator.Generate(config, "TestTag"));
        });

        // Assert
        using var sp = services.BuildServiceProvider();
        var registry = sp.GetRequiredService<IEffectRegistry>();
        var all = registry.GetAll();
        all.Should().HaveCount(1);
        all[0].EffectName.Should().Be("Test Effect");
    }

    [Fact]
    public void AddBlazorEffectsPlayground_WithNoEffects_ShouldRegisterEmptyRegistry()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddBlazorEffectsPlayground(_ => { });

        // Assert
        using var sp = services.BuildServiceProvider();
        var registry = sp.GetRequiredService<IEffectRegistry>();
        registry.GetAll().Should().BeEmpty();
    }

    [Fact]
    public void AddBlazorEffectsPlayground_ShouldReturnServiceCollectionForChaining()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddBlazorEffectsPlayground(_ => { });

        // Assert
        result.Should().BeSameAs(services);
    }

    [Fact]
    public void AddBlazorEffectsPlayground_RegistryShouldBeSingleton()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddBlazorEffectsPlayground(b =>
            b.AddEffect<TestConfig, TestDescriptor>(c => $"<T>{c.Name}</T>"));

        // Act
        using var sp = services.BuildServiceProvider();
        var registry1 = sp.GetRequiredService<IEffectRegistry>();
        var registry2 = sp.GetRequiredService<IEffectRegistry>();

        // Assert
        registry1.Should().BeSameAs(registry2);
    }

    // --- Helper ---

    private static ServiceProvider BuildServiceProvider(PlaygroundBuilder builder)
    {
        var services = new ServiceCollection();
        services.AddSingleton<IEffectRegistry>(sp =>
        {
            var registry = new EffectRegistry();
            foreach (var factory in builder.AdapterFactories)
            {
                registry.Register(factory(sp));
            }
            return registry;
        });
        return services.BuildServiceProvider();
    }
}
