using BlazorEffects.Blobs;
using Bunit;
using AwesomeAssertions;

namespace BlazorEffects.Blobs.Tests;

public class MorphingBlobsComponentTests : BunitContext
{
    private readonly BunitJSModuleInterop _moduleJS;

    public MorphingBlobsComponentTests()
    {
        // Setup the JS module mock for the Morphing Blobs effect
        // EffectComponentBase does: JS.InvokeAsync<IJSObjectReference>("import", ModulePath)
        // then: module.InvokeAsync<string>("init", canvasElement, config)
        _moduleJS = JSInterop.SetupModule("./_content/BlazorEffects.Blobs/morphing-blobs.js");
        _moduleJS.Setup<string>("init", _ => true).SetResult("mock-instance-0");
        _moduleJS.SetupVoid("dispose", _ => true);
        _moduleJS.SetupVoid("update", _ => true);
    }

    [Fact]
    public void MorphingBlobs_ShouldRenderCanvasElement()
    {
        // Act
        var cut = Render<MorphingBlobs>();

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void MorphingBlobs_ShouldRenderWithDefaultHeight()
    {
        // Act
        var cut = Render<MorphingBlobs>();

        // Assert
        var container = cut.Find(".morphing-blobs-container");
        container.GetAttribute("style").Should().Contain("100vh");
    }

    [Fact]
    public void MorphingBlobs_ShouldApplyCustomHeight()
    {
        // Act
        var cut = Render<MorphingBlobs>(parameters => parameters
            .Add(p => p.Height, "50vh"));

        // Assert
        var container = cut.Find(".morphing-blobs-container");
        container.GetAttribute("style").Should().Contain("50vh");
    }

    [Fact]
    public void MorphingBlobs_ShouldRenderChildContent()
    {
        // Act
        var cut = Render<MorphingBlobs>(parameters => parameters
            .Add(p => p.ChildContent, "<h1>Hello Blobs</h1>"));

        // Assert
        cut.Markup.Should().Contain("Hello Blobs");
        cut.Find(".morphing-blobs-overlay").Should().NotBeNull();
    }

    [Fact]
    public void MorphingBlobs_ShouldNotRenderOverlay_WhenNoChildContent()
    {
        // Act
        var cut = Render<MorphingBlobs>();

        // Assert
        cut.FindAll(".morphing-blobs-overlay").Should().BeEmpty();
    }

    [Fact]
    public void MorphingBlobs_ShouldApplyCssClass()
    {
        // Act
        var cut = Render<MorphingBlobs>(parameters => parameters
            .Add(p => p.CssClass, "my-overlay")
            .Add(p => p.ChildContent, "<p>Content</p>"));

        // Assert
        cut.Find(".morphing-blobs-overlay").ClassList.Should().Contain("my-overlay");
    }

    [Fact]
    public void MorphingBlobs_ShouldApplyCanvasOpacity()
    {
        // Act
        var cut = Render<MorphingBlobs>(parameters => parameters
            .Add(p => p.Opacity, 0.5));

        // Assert
        var canvas = cut.Find("canvas");
        canvas.GetAttribute("style").Should().Contain("0.50");
    }

    [Fact]
    public void MorphingBlobs_ShouldRenderWithCustomBlobCount()
    {
        // Act
        var cut = Render<MorphingBlobs>(parameters => parameters
            .Add(p => p.BlobCount, 6));

        // Assert - component renders without errors
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void MorphingBlobs_ShouldRenderWithCustomColors()
    {
        // Act
        var cut = Render<MorphingBlobs>(parameters => parameters
            .Add(p => p.Colors, new[] { "#ff0000", "#00ff00", "#0000ff" }));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void MorphingBlobs_ShouldRenderWithSunsetPreset()
    {
        // Act
        var cut = Render<MorphingBlobs>(parameters => parameters
            .Add(p => p.Colors, MorphingBlobsPresets.Sunset));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void MorphingBlobs_ShouldRenderWithOceanPreset()
    {
        // Act
        var cut = Render<MorphingBlobs>(parameters => parameters
            .Add(p => p.Colors, MorphingBlobsPresets.Ocean));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void MorphingBlobs_ShouldRenderWithAuroraPreset()
    {
        // Act
        var cut = Render<MorphingBlobs>(parameters => parameters
            .Add(p => p.Colors, MorphingBlobsPresets.Aurora));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void MorphingBlobs_ShouldRenderWithNeonPreset()
    {
        // Act
        var cut = Render<MorphingBlobs>(parameters => parameters
            .Add(p => p.Colors, MorphingBlobsPresets.Neon));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void MorphingBlobs_ShouldRenderWithPastelPreset()
    {
        // Act
        var cut = Render<MorphingBlobs>(parameters => parameters
            .Add(p => p.Colors, MorphingBlobsPresets.Pastel));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void MorphingBlobs_ShouldRenderWithCustomBlendMode()
    {
        // Act
        var cut = Render<MorphingBlobs>(parameters => parameters
            .Add(p => p.BlendMode, "lighter"));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void MorphingBlobs_ShouldRenderWithAllCustomParameters()
    {
        // Act
        var cut = Render<MorphingBlobs>(parameters => parameters
            .Add(p => p.BlobCount, 6)
            .Add(p => p.Colors, MorphingBlobsPresets.Neon)
            .Add(p => p.BlobSize, 200)
            .Add(p => p.Speed, 0.01)
            .Add(p => p.MorphIntensity, 100)
            .Add(p => p.BlendMode, "lighter")
            .Add(p => p.Opacity, 0.5)
            .Add(p => p.TargetFps, 30));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
        var container = cut.Find(".morphing-blobs-container");
        container.GetAttribute("style").Should().Contain("100vh");
    }

    [Fact]
    public void MorphingBlobs_ShouldHaveBlackBackground()
    {
        // Act
        var cut = Render<MorphingBlobs>();

        // Assert
        var container = cut.Find(".morphing-blobs-container");
        container.GetAttribute("style").Should().Contain("background:#000");
    }
}
