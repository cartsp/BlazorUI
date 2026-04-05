using BlazorEffects.VortexTunnel;
using Bunit;
using AwesomeAssertions;

namespace BlazorEffects.VortexTunnel.Tests;

public class VortexTunnelComponentTests : BunitContext
{
    private readonly BunitJSModuleInterop _moduleJS;

    public VortexTunnelComponentTests()
    {
        _moduleJS = JSInterop.SetupModule("./_content/BlazorEffects.VortexTunnel/vortextunnel.js");
        _moduleJS.Setup<string>("init", _ => true).SetResult("mock-instance-0");
        _moduleJS.SetupVoid("dispose", _ => true);
        _moduleJS.SetupVoid("update", _ => true);
    }

    [Fact]
    public void VortexTunnel_ShouldRenderCanvasElement()
    {
        var cut = Render<VortexTunnel>();

        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void VortexTunnel_ShouldRenderWithDefaultHeight()
    {
        var cut = Render<VortexTunnel>();

        var container = cut.Find(".vortextunnel-container");
        container.GetAttribute("style").Should().Contain("100vh");
    }

    [Fact]
    public void VortexTunnel_ShouldApplyCustomHeight()
    {
        var cut = Render<VortexTunnel>(parameters => parameters
            .Add(p => p.Height, "50vh"));

        var container = cut.Find(".vortextunnel-container");
        container.GetAttribute("style").Should().Contain("50vh");
    }

    [Fact]
    public void VortexTunnel_ShouldRenderChildContent()
    {
        var cut = Render<VortexTunnel>(parameters => parameters
            .Add(p => p.ChildContent, "<h1>Hello VortexTunnel</h1>"));

        cut.Markup.Should().Contain("Hello VortexTunnel");
        cut.Find(".vortextunnel-overlay").Should().NotBeNull();
    }

    [Fact]
    public void VortexTunnel_ShouldNotRenderOverlay_WhenNoChildContent()
    {
        var cut = Render<VortexTunnel>();

        cut.FindAll(".vortextunnel-overlay").Should().BeEmpty();
    }

    [Fact]
    public void VortexTunnel_ShouldApplyCssClass()
    {
        var cut = Render<VortexTunnel>(parameters => parameters
            .Add(p => p.CssClass, "my-overlay")
            .Add(p => p.ChildContent, "<p>Content</p>"));

        cut.Find(".vortextunnel-overlay").ClassList.Should().Contain("my-overlay");
    }

    [Fact]
    public void VortexTunnel_ShouldApplyCanvasOpacity()
    {
        var cut = Render<VortexTunnel>(parameters => parameters
            .Add(p => p.Opacity, 0.5));

        var canvas = cut.Find("canvas");
        canvas.GetAttribute("style").Should().Contain("0.50");
    }

    [Fact]
    public void VortexTunnel_ShouldRenderWithCustomRingCount()
    {
        var cut = Render<VortexTunnel>(parameters => parameters
            .Add(p => p.RingCount, 25));

        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void VortexTunnel_ShouldRenderWithCustomColors()
    {
        var cut = Render<VortexTunnel>(parameters => parameters
            .Add(p => p.Color, "#22d3ee")
            .Add(p => p.BackgroundColor, "#020617"));

        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void VortexTunnel_ShouldRenderWithPolygonShape()
    {
        var cut = Render<VortexTunnel>(parameters => parameters
            .Add(p => p.Shape, "polygon")
            .Add(p => p.PolygonSides, 8));

        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void VortexTunnel_ShouldRenderWithHypnoWheelPreset()
    {
        var preset = VortexTunnelPresets.HypnoWheel;
        var cut = Render<VortexTunnel>(parameters => parameters
            .Add(p => p.RingCount, preset.RingCount)
            .Add(p => p.RotationSpeed, preset.RotationSpeed)
            .Add(p => p.Shape, preset.Shape)
            .Add(p => p.ScaleFactor, preset.ScaleFactor));

        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void VortexTunnel_ShouldRenderWithColorsArray()
    {
        var cut = Render<VortexTunnel>(parameters => parameters
            .Add(p => p.Colors, ["#ef4444", "#3b82f6", "#22c55e"]));

        cut.Find("canvas").Should().NotBeNull();
    }
}
