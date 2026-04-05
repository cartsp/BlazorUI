using BlazorEffects.MatrixRain;
using Bunit;
using AwesomeAssertions;

namespace BlazorEffects.MatrixRain.Tests;

public class MatrixRainComponentTests : BunitContext
{
    private readonly BunitJSModuleInterop _moduleJS;

    public MatrixRainComponentTests()
    {
        // Setup the JS module mock for the Matrix Rain effect
        // EffectComponentBase does: JS.InvokeAsync<IJSObjectReference>("import", ModulePath)
        // then: module.InvokeAsync<string>("init", canvasElement, config)
        _moduleJS = JSInterop.SetupModule("./_content/BlazorEffects.MatrixRain/matrix-rain.js");
        _moduleJS.Setup<string>("init", _ => true).SetResult("mock-instance-0");
        _moduleJS.SetupVoid("dispose", _ => true);
        _moduleJS.SetupVoid("update", _ => true);
    }

    [Fact]
    public void MatrixRain_ShouldRenderCanvasElement()
    {
        // Act
        var cut = Render<BlazorEffects.MatrixRain.MatrixRain>();

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void MatrixRain_ShouldRenderWithDefaultHeight()
    {
        // Act
        var cut = Render<BlazorEffects.MatrixRain.MatrixRain>();

        // Assert
        var container = cut.Find(".matrix-rain-container");
        container.GetAttribute("style").Should().Contain("100vh");
    }

    [Fact]
    public void MatrixRain_ShouldApplyCustomHeight()
    {
        // Act
        var cut = Render<BlazorEffects.MatrixRain.MatrixRain>(parameters => parameters
            .Add(p => p.Height, "50vh"));

        // Assert
        var container = cut.Find(".matrix-rain-container");
        container.GetAttribute("style").Should().Contain("50vh");
    }

    [Fact]
    public void MatrixRain_ShouldRenderChildContent()
    {
        // Act
        var cut = Render<BlazorEffects.MatrixRain.MatrixRain>(parameters => parameters
            .Add(p => p.ChildContent, "<h1>Hello Matrix</h1>"));

        // Assert
        cut.Markup.Should().Contain("Hello Matrix");
        cut.Find(".matrix-rain-overlay").Should().NotBeNull();
    }

    [Fact]
    public void MatrixRain_ShouldNotRenderOverlay_WhenNoChildContent()
    {
        // Act
        var cut = Render<BlazorEffects.MatrixRain.MatrixRain>();

        // Assert
        cut.FindAll(".matrix-rain-overlay").Should().BeEmpty();
    }

    [Fact]
    public void MatrixRain_ShouldApplyCssClass()
    {
        // Act
        var cut = Render<BlazorEffects.MatrixRain.MatrixRain>(parameters => parameters
            .Add(p => p.CssClass, "my-overlay")
            .Add(p => p.ChildContent, "<p>Content</p>"));

        // Assert
        cut.Find(".matrix-rain-overlay").ClassList.Should().Contain("my-overlay");
    }

    [Fact]
    public void MatrixRain_ShouldApplyCanvasOpacity()
    {
        // Act
        var cut = Render<BlazorEffects.MatrixRain.MatrixRain>(parameters => parameters
            .Add(p => p.Opacity, 0.5));

        // Assert
        var canvas = cut.Find("canvas");
        canvas.GetAttribute("style").Should().Contain("0.50");
    }

    [Fact]
    public void MatrixRain_ShouldRenderWithCustomColor()
    {
        // Act
        var cut = Render<BlazorEffects.MatrixRain.MatrixRain>(parameters => parameters
            .Add(p => p.Color, "#ff0000"));

        // Assert - component renders without errors
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void MatrixRain_ShouldRenderWithBinaryPreset()
    {
        // Act
        var cut = Render<BlazorEffects.MatrixRain.MatrixRain>(parameters => parameters
            .Add(p => p.Characters, MatrixRainPresets.Binary));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void MatrixRain_ShouldRenderWithKatakanaPreset()
    {
        // Act
        var cut = Render<BlazorEffects.MatrixRain.MatrixRain>(parameters => parameters
            .Add(p => p.Characters, MatrixRainPresets.Katakana));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }
}
