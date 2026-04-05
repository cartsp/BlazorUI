using BlazorEffects.Particles;
using Bunit;
using AwesomeAssertions;

namespace BlazorEffects.Particles.Tests;

public class ParticleConstellationComponentTests : BunitContext
{
    private readonly BunitJSModuleInterop _moduleJS;

    public ParticleConstellationComponentTests()
    {
        _moduleJS = JSInterop.SetupModule("./_content/BlazorEffects.Particles/particle-constellation.js");
        _moduleJS.Setup<string>("init", _ => true).SetResult("mock-instance-0");
        _moduleJS.SetupVoid("dispose", _ => true);
        _moduleJS.SetupVoid("update", _ => true);
        _moduleJS.SetupVoid("setMousePosition", _ => true);
        _moduleJS.SetupVoid("clearMousePosition", _ => true);
    }

    [Fact]
    public void ParticleConstellation_ShouldRenderCanvasElement()
    {
        // Act
        var cut = Render<ParticleConstellation>();

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void ParticleConstellation_ShouldRenderWithDefaultHeight()
    {
        // Act
        var cut = Render<ParticleConstellation>();

        // Assert
        var container = cut.Find(".particle-constellation-container");
        container.GetAttribute("style").Should().Contain("100vh");
    }

    [Fact]
    public void ParticleConstellation_ShouldApplyCustomHeight()
    {
        // Act
        var cut = Render<ParticleConstellation>(parameters => parameters
            .Add(p => p.Height, "50vh"));

        // Assert
        var container = cut.Find(".particle-constellation-container");
        container.GetAttribute("style").Should().Contain("50vh");
    }

    [Fact]
    public void ParticleConstellation_ShouldRenderChildContent()
    {
        // Act
        var cut = Render<ParticleConstellation>(parameters => parameters
            .Add(p => p.ChildContent, "<h1>Hello Particles</h1>"));

        // Assert
        cut.Markup.Should().Contain("Hello Particles");
        cut.Find(".particle-constellation-overlay").Should().NotBeNull();
    }

    [Fact]
    public void ParticleConstellation_ShouldNotRenderOverlay_WhenNoChildContent()
    {
        // Act
        var cut = Render<ParticleConstellation>();

        // Assert
        cut.FindAll(".particle-constellation-overlay").Should().BeEmpty();
    }

    [Fact]
    public void ParticleConstellation_ShouldApplyCssClass()
    {
        // Act
        var cut = Render<ParticleConstellation>(parameters => parameters
            .Add(p => p.CssClass, "my-overlay")
            .Add(p => p.ChildContent, "<p>Content</p>"));

        // Assert
        cut.Find(".particle-constellation-overlay").ClassList.Should().Contain("my-overlay");
    }

    [Fact]
    public void ParticleConstellation_ShouldApplyCanvasOpacity()
    {
        // Act
        var cut = Render<ParticleConstellation>(parameters => parameters
            .Add(p => p.Opacity, 0.5));

        // Assert
        var canvas = cut.Find("canvas");
        canvas.GetAttribute("style").Should().Contain("0.50");
    }

    [Fact]
    public void ParticleConstellation_ShouldRenderWithCustomParticleCount()
    {
        // Act
        var cut = Render<ParticleConstellation>(parameters => parameters
            .Add(p => p.ParticleCount, 300));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void ParticleConstellation_ShouldRenderWithCustomColors()
    {
        // Act
        var cut = Render<ParticleConstellation>(parameters => parameters
            .Add(p => p.ParticleColor, "#ff0000")
            .Add(p => p.ConnectionColor, "#00ff00"));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void ParticleConstellation_ShouldRenderWithRepelForce()
    {
        // Act
        var cut = Render<ParticleConstellation>(parameters => parameters
            .Add(p => p.MouseForce, "repel"));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void ParticleConstellation_ShouldRenderWithMouseDisabled()
    {
        // Act
        var cut = Render<ParticleConstellation>(parameters => parameters
            .Add(p => p.MouseInteraction, false));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void ParticleConstellation_ShouldRenderWithMinimalPreset()
    {
        // Act
        var preset = ParticleConstellationPresets.Minimal;
        var cut = Render<ParticleConstellation>(parameters => parameters
            .Add(p => p.ParticleCount, preset.ParticleCount)
            .Add(p => p.ParticleColor, preset.ParticleColor)
            .Add(p => p.ConnectionColor, preset.ConnectionColor)
            .Add(p => p.ParticleSize, preset.ParticleSize)
            .Add(p => p.ConnectionDistance, preset.ConnectionDistance)
            .Add(p => p.Speed, preset.Speed)
            .Add(p => p.Opacity, preset.Opacity));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }
}
