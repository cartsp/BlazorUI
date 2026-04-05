using BlazorEffects.GradientWaves;
using Bunit;
using AwesomeAssertions;

namespace BlazorEffects.GradientWaves.Tests;

public class GradientWavesComponentTests : BunitContext
{
    private readonly BunitJSModuleInterop _moduleJS;

    public GradientWavesComponentTests()
    {
        // Setup the JS module mock for the Gradient Waves effect
        // EffectComponentBase does: JS.InvokeAsync<IJSObjectReference>("import", ModulePath)
        // then: module.InvokeAsync<string>("init", canvasElement, config)
        _moduleJS = JSInterop.SetupModule("./_content/BlazorEffects.GradientWaves/gradient-waves.js");
        _moduleJS.Setup<string>("init", _ => true).SetResult("mock-instance-0");
        _moduleJS.SetupVoid("dispose", _ => true);
        _moduleJS.SetupVoid("update", _ => true);
    }

    [Fact]
    public void GradientWaves_ShouldRenderCanvasElement()
    {
        // Act
        var cut = Render<GradientWaves>();

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void GradientWaves_ShouldRenderWithDefaultHeight()
    {
        // Act
        var cut = Render<GradientWaves>();

        // Assert
        var container = cut.Find(".gradient-waves-container");
        container.GetAttribute("style").Should().Contain("100vh");
    }

    [Fact]
    public void GradientWaves_ShouldApplyCustomHeight()
    {
        // Act
        var cut = Render<GradientWaves>(parameters => parameters
            .Add(p => p.Height, "50vh"));

        // Assert
        var container = cut.Find(".gradient-waves-container");
        container.GetAttribute("style").Should().Contain("50vh");
    }

    [Fact]
    public void GradientWaves_ShouldRenderChildContent()
    {
        // Act
        var cut = Render<GradientWaves>(parameters => parameters
            .Add(p => p.ChildContent, "<h1>Hello Waves</h1>"));

        // Assert
        cut.Markup.Should().Contain("Hello Waves");
        cut.Find(".gradient-waves-overlay").Should().NotBeNull();
    }

    [Fact]
    public void GradientWaves_ShouldNotRenderOverlay_WhenNoChildContent()
    {
        // Act
        var cut = Render<GradientWaves>();

        // Assert
        cut.FindAll(".gradient-waves-overlay").Should().BeEmpty();
    }

    [Fact]
    public void GradientWaves_ShouldApplyCssClass()
    {
        // Act
        var cut = Render<GradientWaves>(parameters => parameters
            .Add(p => p.CssClass, "my-overlay")
            .Add(p => p.ChildContent, "<p>Content</p>"));

        // Assert
        cut.Find(".gradient-waves-overlay").ClassList.Should().Contain("my-overlay");
    }

    [Fact]
    public void GradientWaves_ShouldApplyCanvasOpacity()
    {
        // Act
        var cut = Render<GradientWaves>(parameters => parameters
            .Add(p => p.Opacity, 0.5));

        // Assert
        var canvas = cut.Find("canvas");
        canvas.GetAttribute("style").Should().Contain("0.50");
    }

    [Fact]
    public void GradientWaves_ShouldRenderWithCustomPointCount()
    {
        // Act
        var cut = Render<GradientWaves>(parameters => parameters
            .Add(p => p.PointCount, 8));

        // Assert - component renders without errors
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void GradientWaves_ShouldRenderWithCustomColors()
    {
        // Act
        var cut = Render<GradientWaves>(parameters => parameters
            .Add(p => p.Colors, new[] { "#ff0000", "#00ff00", "#0000ff" }));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void GradientWaves_ShouldRenderWithStripePreset()
    {
        // Act
        var cut = Render<GradientWaves>(parameters => parameters
            .Add(p => p.Colors, GradientWavesPresets.Stripe));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void GradientWaves_ShouldRenderWithOceanDeepPreset()
    {
        // Act
        var cut = Render<GradientWaves>(parameters => parameters
            .Add(p => p.Colors, GradientWavesPresets.OceanDeep));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void GradientWaves_ShouldRenderWithNorthernLightsPreset()
    {
        // Act
        var cut = Render<GradientWaves>(parameters => parameters
            .Add(p => p.Colors, GradientWavesPresets.NorthernLights));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void GradientWaves_ShouldRenderWithCyberpunkPreset()
    {
        // Act
        var cut = Render<GradientWaves>(parameters => parameters
            .Add(p => p.Colors, GradientWavesPresets.Cyberpunk));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void GradientWaves_ShouldRenderWithRoseGardenPreset()
    {
        // Act
        var cut = Render<GradientWaves>(parameters => parameters
            .Add(p => p.Colors, GradientWavesPresets.RoseGarden));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void GradientWaves_ShouldRenderWithForestPreset()
    {
        // Act
        var cut = Render<GradientWaves>(parameters => parameters
            .Add(p => p.Colors, GradientWavesPresets.Forest));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void GradientWaves_ShouldRenderWithCustomBlendMode()
    {
        // Act
        var cut = Render<GradientWaves>(parameters => parameters
            .Add(p => p.BlendMode, "screen"));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void GradientWaves_ShouldRenderWithAllCustomParameters()
    {
        // Act
        var cut = Render<GradientWaves>(parameters => parameters
            .Add(p => p.Colors, GradientWavesPresets.Cyberpunk)
            .Add(p => p.PointCount, 4)
            .Add(p => p.BlobSize, 0.3)
            .Add(p => p.Speed, 0.01)
            .Add(p => p.BlurAmount, 120)
            .Add(p => p.BlendMode, "screen")
            .Add(p => p.Opacity, 0.5)
            .Add(p => p.TargetFps, 30));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
        var container = cut.Find(".gradient-waves-container");
        container.GetAttribute("style").Should().Contain("100vh");
    }

    [Fact]
    public void GradientWaves_ShouldHaveDefaultBackground()
    {
        // Act
        var cut = Render<GradientWaves>();

        // Assert
        var container = cut.Find(".gradient-waves-container");
        container.GetAttribute("style").Should().Contain("#0f172a");
    }
}
