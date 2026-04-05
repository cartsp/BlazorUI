using BlazorEffects.FireEmbers;
using Bunit;
using AwesomeAssertions;

namespace BlazorEffects.FireEmbers.Tests;

public class FireEmbersComponentTests : BunitContext
{
    private readonly BunitJSModuleInterop _moduleJS;

    public FireEmbersComponentTests()
    {
        _moduleJS = JSInterop.SetupModule("./_content/BlazorEffects.FireEmbers/fire-embers.js");
        _moduleJS.Setup<string>("init", _ => true).SetResult("mock-instance-0");
        _moduleJS.SetupVoid("dispose", _ => true);
        _moduleJS.SetupVoid("update", _ => true);
    }

    [Fact]
    public void FireEmbers_ShouldRenderCanvasElement()
    {
        var cut = Render<FireEmbers>();

        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void FireEmbers_ShouldRenderWithDefaultHeight()
    {
        var cut = Render<FireEmbers>();

        var container = cut.Find(".fire-embers-container");
        container.GetAttribute("style").Should().Contain("100vh");
    }

    [Fact]
    public void FireEmbers_ShouldApplyCustomHeight()
    {
        var cut = Render<FireEmbers>(parameters => parameters
            .Add(p => p.Height, "50vh"));

        var container = cut.Find(".fire-embers-container");
        container.GetAttribute("style").Should().Contain("50vh");
    }

    [Fact]
    public void FireEmbers_ShouldRenderChildContent()
    {
        var cut = Render<FireEmbers>(parameters => parameters
            .Add(p => p.ChildContent, "<h1>Hello Fire</h1>"));

        cut.Markup.Should().Contain("Hello Fire");
        cut.Find(".fire-embers-overlay").Should().NotBeNull();
    }

    [Fact]
    public void FireEmbers_ShouldNotRenderOverlay_WhenNoChildContent()
    {
        var cut = Render<FireEmbers>();

        cut.FindAll(".fire-embers-overlay").Should().BeEmpty();
    }

    [Fact]
    public void FireEmbers_ShouldApplyCssClass()
    {
        var cut = Render<FireEmbers>(parameters => parameters
            .Add(p => p.CssClass, "my-overlay")
            .Add(p => p.ChildContent, "<p>Content</p>"));

        cut.Find(".fire-embers-overlay").ClassList.Should().Contain("my-overlay");
    }

    [Fact]
    public void FireEmbers_ShouldApplyCanvasOpacity()
    {
        var cut = Render<FireEmbers>(parameters => parameters
            .Add(p => p.Opacity, 0.5));

        var canvas = cut.Find("canvas");
        canvas.GetAttribute("style").Should().Contain("0.50");
    }

    [Fact]
    public void FireEmbers_ShouldRenderWithCustomParticleCount()
    {
        var cut = Render<FireEmbers>(parameters => parameters
            .Add(p => p.ParticleCount, 500));

        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void FireEmbers_ShouldRenderWithCustomColors()
    {
        var cut = Render<FireEmbers>(parameters => parameters
            .Add(p => p.FlameColor, "#3b82f6")
            .Add(p => p.EmberColor, "#93c5fd"));

        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void FireEmbers_ShouldRenderWithBonfirePreset()
    {
        var preset = FireEmbersPresets.Bonfire;
        var cut = Render<FireEmbers>(parameters => parameters
            .Add(p => p.ParticleCount, preset.ParticleCount)
            .Add(p => p.FlameColor, preset.FlameColor)
            .Add(p => p.EmberColor, preset.EmberColor)
            .Add(p => p.Speed, preset.Speed)
            .Add(p => p.Turbulence, preset.Turbulence));

        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void FireEmbers_ShouldRenderWithHighTurbulence()
    {
        var cut = Render<FireEmbers>(parameters => parameters
            .Add(p => p.Turbulence, 3.0));

        cut.Find("canvas").Should().NotBeNull();
    }
}
