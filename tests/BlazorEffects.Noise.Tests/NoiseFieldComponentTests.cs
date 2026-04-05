using BlazorEffects.Noise;
using Bunit;
using AwesomeAssertions;

namespace BlazorEffects.Noise.Tests;

public class NoiseFieldComponentTests : BunitContext
{
    private readonly BunitJSModuleInterop _moduleJS;

    public NoiseFieldComponentTests()
    {
        // Setup the JS module mock for the Noise Field effect
        _moduleJS = JSInterop.SetupModule("./_content/BlazorEffects.Noise/noise-field.js");
        _moduleJS.Setup<string>("init", _ => true).SetResult("mock-instance-0");
        _moduleJS.SetupVoid("dispose", _ => true);
        _moduleJS.SetupVoid("update", _ => true);
    }

    [Fact]
    public void NoiseField_ShouldRenderCanvasElement()
    {
        // Act
        var cut = Render<NoiseField>();

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void NoiseField_ShouldRenderWithDefaultHeight()
    {
        // Act
        var cut = Render<NoiseField>();

        // Assert
        var container = cut.Find(".noise-field-container");
        container.GetAttribute("style").Should().Contain("100vh");
    }

    [Fact]
    public void NoiseField_ShouldApplyCustomHeight()
    {
        // Act
        var cut = Render<NoiseField>(parameters => parameters
            .Add(p => p.Height, "50vh"));

        // Assert
        var container = cut.Find(".noise-field-container");
        container.GetAttribute("style").Should().Contain("50vh");
    }

    [Fact]
    public void NoiseField_ShouldRenderChildContent()
    {
        // Act
        var cut = Render<NoiseField>(parameters => parameters
            .Add(p => p.ChildContent, "<h1>Hello Noise</h1>"));

        // Assert
        cut.Markup.Should().Contain("Hello Noise");
        cut.Find(".noise-field-overlay").Should().NotBeNull();
    }

    [Fact]
    public void NoiseField_ShouldNotRenderOverlay_WhenNoChildContent()
    {
        // Act
        var cut = Render<NoiseField>();

        // Assert
        cut.FindAll(".noise-field-overlay").Should().BeEmpty();
    }

    [Fact]
    public void NoiseField_ShouldApplyCssClass()
    {
        // Act
        var cut = Render<NoiseField>(parameters => parameters
            .Add(p => p.CssClass, "my-overlay")
            .Add(p => p.ChildContent, "<p>Content</p>"));

        // Assert
        cut.Find(".noise-field-overlay").ClassList.Should().Contain("my-overlay");
    }

    [Fact]
    public void NoiseField_ShouldApplyCanvasOpacity()
    {
        // Act
        var cut = Render<NoiseField>(parameters => parameters
            .Add(p => p.Opacity, 0.5));

        // Assert
        var canvas = cut.Find("canvas");
        canvas.GetAttribute("style").Should().Contain("0.50");
    }

    [Fact]
    public void NoiseField_ShouldRenderWithCustomColorStops()
    {
        // Act
        var cut = Render<NoiseField>(parameters => parameters
            .Add(p => p.ColorStops, new[] { "#000000", "#ffffff", "#000000" }));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void NoiseField_ShouldRenderWithAuroraPreset()
    {
        // Act
        var cut = Render<NoiseField>(parameters => parameters
            .Add(p => p.ColorStops, NoiseFieldPresets.Aurora));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void NoiseField_ShouldRenderWithSunsetPreset()
    {
        // Act
        var cut = Render<NoiseField>(parameters => parameters
            .Add(p => p.ColorStops, NoiseFieldPresets.Sunset));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void NoiseField_ShouldRenderWithOceanPreset()
    {
        // Act
        var cut = Render<NoiseField>(parameters => parameters
            .Add(p => p.ColorStops, NoiseFieldPresets.Ocean));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void NoiseField_ShouldRenderWithLavaPreset()
    {
        // Act
        var cut = Render<NoiseField>(parameters => parameters
            .Add(p => p.ColorStops, NoiseFieldPresets.Lava));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void NoiseField_ShouldRenderWithMonochromePreset()
    {
        // Act
        var cut = Render<NoiseField>(parameters => parameters
            .Add(p => p.ColorStops, NoiseFieldPresets.Monochrome));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void NoiseField_ShouldRenderWithNeonPreset()
    {
        // Act
        var cut = Render<NoiseField>(parameters => parameters
            .Add(p => p.ColorStops, NoiseFieldPresets.Neon));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void NoiseField_ShouldRenderWithAllCustomParameters()
    {
        // Act
        var cut = Render<NoiseField>(parameters => parameters
            .Add(p => p.ColorStops, NoiseFieldPresets.Neon)
            .Add(p => p.NoiseScale, 0.01)
            .Add(p => p.Speed, 0.005)
            .Add(p => p.Octaves, 5)
            .Add(p => p.Persistence, 0.6)
            .Add(p => p.Lacunarity, 2.5)
            .Add(p => p.Brightness, 1.5)
            .Add(p => p.Opacity, 0.5)
            .Add(p => p.TargetFps, 30));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
        var container = cut.Find(".noise-field-container");
        container.GetAttribute("style").Should().Contain("100vh");
    }

    [Fact]
    public void NoiseField_ShouldHaveBlackBackground()
    {
        // Act
        var cut = Render<NoiseField>();

        // Assert
        var container = cut.Find(".noise-field-container");
        container.GetAttribute("style").Should().Contain("background:#000");
    }

    [Fact]
    public void NoiseField_ShouldRenderWithCustomNoiseScale()
    {
        // Act
        var cut = Render<NoiseField>(parameters => parameters
            .Add(p => p.NoiseScale, 0.001));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void NoiseField_ShouldRenderWithHighOctaves()
    {
        // Act
        var cut = Render<NoiseField>(parameters => parameters
            .Add(p => p.Octaves, 6));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void NoiseField_ShouldRenderWithLowTargetFps()
    {
        // Act
        var cut = Render<NoiseField>(parameters => parameters
            .Add(p => p.TargetFps, 15));

        // Assert
        cut.Find("canvas").Should().NotBeNull();
    }
}
