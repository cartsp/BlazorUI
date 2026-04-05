using BlazorEffects.Aurora;
using Bunit;
using AwesomeAssertions;

namespace BlazorEffects.Aurora.Tests;

public class AuroraBorealisComponentTests : BunitContext
{
    private readonly BunitJSModuleInterop _moduleJS;

    public AuroraBorealisComponentTests()
    {
        _moduleJS = JSInterop.SetupModule("./_content/BlazorEffects.Aurora/aurora-borealis.js");
        _moduleJS.Setup<string>("init", _ => true).SetResult("mock-instance-0");
        _moduleJS.SetupVoid("dispose", _ => true);
        _moduleJS.SetupVoid("update", _ => true);
    }

    [Fact]
    public void AuroraBorealis_ShouldRenderCanvasElement()
    {
        // Act
        var cut = Render<BlazorEffects.Aurora.AuroraBorealis>();

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void AuroraBorealis_ShouldRenderWithDefaultHeight()
    {
        // Act
        var cut = Render<BlazorEffects.Aurora.AuroraBorealis>();

        // Assert
        var container = cut.Find(".aurora-borealis-container");
        container.GetAttribute("style").Should().Contain("100vh");
    }

    [Fact]
    public void AuroraBorealis_ShouldApplyCustomHeight()
    {
        // Act
        var cut = Render<BlazorEffects.Aurora.AuroraBorealis>(parameters => parameters
            .Add(p => p.Height, "50vh"));

        // Assert
        var container = cut.Find(".aurora-borealis-container");
        container.GetAttribute("style").Should().Contain("50vh");
    }

    [Fact]
    public void AuroraBorealis_ShouldRenderChildContent()
    {
        // Act
        var cut = Render<BlazorEffects.Aurora.AuroraBorealis>(parameters => parameters
            .Add(p => p.ChildContent, "<h1>Hello Aurora</h1>"));

        // Assert
        cut.Markup.Should().Contain("Hello Aurora");
        cut.Find(".aurora-borealis-overlay").Should().NotBeNull();
    }

    [Fact]
    public void AuroraBorealis_ShouldNotRenderOverlay_WhenNoChildContent()
    {
        // Act
        var cut = Render<BlazorEffects.Aurora.AuroraBorealis>();

        // Assert
        cut.FindAll(".aurora-borealis-overlay").Should().BeEmpty();
    }

    [Fact]
    public void AuroraBorealis_ShouldApplyCssClass()
    {
        // Act
        var cut = Render<BlazorEffects.Aurora.AuroraBorealis>(parameters => parameters
            .Add(p => p.CssClass, "my-overlay")
            .Add(p => p.ChildContent, "<p>Content</p>"));

        // Assert
        cut.Find(".aurora-borealis-overlay").ClassList.Should().Contain("my-overlay");
    }

    [Fact]
    public void AuroraBorealis_ShouldApplyCanvasOpacity()
    {
        // Act
        var cut = Render<BlazorEffects.Aurora.AuroraBorealis>(parameters => parameters
            .Add(p => p.Opacity, 0.5));

        // Assert
        var canvas = cut.Find("canvas");
        canvas.GetAttribute("style").Should().Contain("0.50");
    }

    [Fact]
    public void AuroraBorealis_ShouldHaveBlackBackground()
    {
        // Act
        var cut = Render<BlazorEffects.Aurora.AuroraBorealis>();

        // Assert
        var container = cut.Find(".aurora-borealis-container");
        container.GetAttribute("style").Should().Contain("background:#000");
    }

    [Fact]
    public void AuroraBorealis_ShouldRenderWithCustomRibbonCount()
    {
        // Act
        var cut = Render<BlazorEffects.Aurora.AuroraBorealis>(parameters => parameters
            .Add(p => p.RibbonCount, 6));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void AuroraBorealis_ShouldRenderWithCustomColors()
    {
        // Act
        var cut = Render<BlazorEffects.Aurora.AuroraBorealis>(parameters => parameters
            .Add(p => p.Colors, new[] { "#ff0000", "#00ff00", "#0000ff" }));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void AuroraBorealis_ShouldRenderWithArcticPreset()
    {
        // Act
        var cut = Render<BlazorEffects.Aurora.AuroraBorealis>(parameters => parameters
            .Add(p => p.Colors, AuroraBorealisPresets.Arctic));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void AuroraBorealis_ShouldRenderWithSunsetPreset()
    {
        // Act
        var cut = Render<BlazorEffects.Aurora.AuroraBorealis>(parameters => parameters
            .Add(p => p.Colors, AuroraBorealisPresets.Sunset));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void AuroraBorealis_ShouldRenderWithEmeraldPreset()
    {
        // Act
        var cut = Render<BlazorEffects.Aurora.AuroraBorealis>(parameters => parameters
            .Add(p => p.Colors, AuroraBorealisPresets.Emerald));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void AuroraBorealis_ShouldRenderWithCosmicPreset()
    {
        // Act
        var cut = Render<BlazorEffects.Aurora.AuroraBorealis>(parameters => parameters
            .Add(p => p.Colors, AuroraBorealisPresets.Cosmic));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void AuroraBorealis_ShouldRenderWithCustomBlendMode()
    {
        // Act
        var cut = Render<BlazorEffects.Aurora.AuroraBorealis>(parameters => parameters
            .Add(p => p.BlendMode, "lighter"));

        // Assert
        var canvas = cut.Find("canvas");
        canvas.GetAttribute("style").Should().Contain("lighter");
    }

    [Fact]
    public void AuroraBorealis_ShouldRenderWithAllCustomParameters()
    {
        // Act
        var cut = Render<BlazorEffects.Aurora.AuroraBorealis>(parameters => parameters
            .Add(p => p.Colors, AuroraBorealisPresets.Cosmic)
            .Add(p => p.RibbonCount, 6)
            .Add(p => p.Amplitude, 200)
            .Add(p => p.Speed, 0.02)
            .Add(p => p.Opacity, 0.8)
            .Add(p => p.BlendMode, "lighter")
            .Add(p => p.TargetFps, 30)
            .Add(p => p.Height, "80vh"));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
        var container = cut.Find(".aurora-borealis-container");
        container.GetAttribute("style").Should().Contain("80vh");
    }
}
