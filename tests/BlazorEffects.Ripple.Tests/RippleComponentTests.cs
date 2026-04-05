using BlazorEffects.Ripple;
using Bunit;
using AwesomeAssertions;

namespace BlazorEffects.Ripple.Tests;

public class RippleComponentTests : BunitContext
{
    private readonly BunitJSModuleInterop _moduleJS;

    public RippleComponentTests()
    {
        _moduleJS = JSInterop.SetupModule("./_content/BlazorEffects.Ripple/ripple.js");
        _moduleJS.Setup<string>("init", _ => true).SetResult("mock-instance-0");
        _moduleJS.SetupVoid("dispose", _ => true);
        _moduleJS.SetupVoid("update", _ => true);
    }

    [Fact]
    public void Ripple_ShouldRenderCanvasElement()
    {
        var cut = Render<Ripple>();

        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void Ripple_ShouldRenderWithDefaultHeight()
    {
        var cut = Render<Ripple>();

        var container = cut.Find(".ripple-container");
        container.GetAttribute("style").Should().Contain("100vh");
    }

    [Fact]
    public void Ripple_ShouldApplyCustomHeight()
    {
        var cut = Render<Ripple>(parameters => parameters
            .Add(p => p.Height, "50vh"));

        var container = cut.Find(".ripple-container");
        container.GetAttribute("style").Should().Contain("50vh");
    }

    [Fact]
    public void Ripple_ShouldRenderChildContent()
    {
        var cut = Render<Ripple>(parameters => parameters
            .Add(p => p.ChildContent, "<h1>Hello Ripple</h1>"));

        cut.Markup.Should().Contain("Hello Ripple");
        cut.Find(".ripple-overlay").Should().NotBeNull();
    }

    [Fact]
    public void Ripple_ShouldNotRenderOverlay_WhenNoChildContent()
    {
        var cut = Render<Ripple>();

        cut.FindAll(".ripple-overlay").Should().BeEmpty();
    }

    [Fact]
    public void Ripple_ShouldApplyCssClass()
    {
        var cut = Render<Ripple>(parameters => parameters
            .Add(p => p.CssClass, "my-overlay")
            .Add(p => p.ChildContent, "<p>Content</p>"));

        cut.Find(".ripple-overlay").ClassList.Should().Contain("my-overlay");
    }

    [Fact]
    public void Ripple_ShouldApplyCanvasOpacity()
    {
        var cut = Render<Ripple>(parameters => parameters
            .Add(p => p.Opacity, 0.5));

        var canvas = cut.Find("canvas");
        canvas.GetAttribute("style").Should().Contain("0.50");
    }

    [Fact]
    public void Ripple_ShouldRenderWithCustomMaxRipples()
    {
        var cut = Render<Ripple>(parameters => parameters
            .Add(p => p.MaxRipples, 30));

        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void Ripple_ShouldRenderWithCustomColors()
    {
        var cut = Render<Ripple>(parameters => parameters
            .Add(p => p.Color, "#22d3ee")
            .Add(p => p.BackgroundColor, "#020617"));

        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void Ripple_ShouldRenderWithNeonElectricPreset()
    {
        var preset = RipplePresets.NeonElectric;
        var cut = Render<Ripple>(parameters => parameters
            .Add(p => p.MaxRipples, preset.MaxRipples)
            .Add(p => p.Color, preset.Color)
            .Add(p => p.Speed, preset.Speed)
            .Add(p => p.LineWidth, preset.LineWidth));

        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void Ripple_ShouldRenderWithClickTrigger()
    {
        var cut = Render<Ripple>(parameters => parameters
            .Add(p => p.Trigger, "click"));

        cut.Find("canvas").Should().NotBeNull();
    }
}
