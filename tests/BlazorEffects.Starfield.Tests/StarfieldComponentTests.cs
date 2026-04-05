using BlazorEffects.Starfield;
using Bunit;
using AwesomeAssertions;

namespace BlazorEffects.Starfield.Tests;

public class StarfieldComponentTests : BunitContext
{
    private readonly BunitJSModuleInterop _moduleJS;

    public StarfieldComponentTests()
    {
        _moduleJS = JSInterop.SetupModule("./_content/BlazorEffects.Starfield/starfield.js");
        _moduleJS.Setup<string>("init", _ => true).SetResult("mock-instance-0");
        _moduleJS.SetupVoid("dispose", _ => true);
        _moduleJS.SetupVoid("update", _ => true);
    }

    [Fact]
    public void Starfield_ShouldRenderCanvasElement()
    {
        var cut = Render<Starfield>();

        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void Starfield_ShouldRenderWithDefaultHeight()
    {
        var cut = Render<Starfield>();

        var container = cut.Find(".starfield-container");
        container.GetAttribute("style").Should().Contain("100vh");
    }

    [Fact]
    public void Starfield_ShouldApplyCustomHeight()
    {
        var cut = Render<Starfield>(parameters => parameters
            .Add(p => p.Height, "50vh"));

        var container = cut.Find(".starfield-container");
        container.GetAttribute("style").Should().Contain("50vh");
    }

    [Fact]
    public void Starfield_ShouldRenderChildContent()
    {
        var cut = Render<Starfield>(parameters => parameters
            .Add(p => p.ChildContent, "<h1>Hello Starfield</h1>"));

        cut.Markup.Should().Contain("Hello Starfield");
        cut.Find(".starfield-overlay").Should().NotBeNull();
    }

    [Fact]
    public void Starfield_ShouldNotRenderOverlay_WhenNoChildContent()
    {
        var cut = Render<Starfield>();

        cut.FindAll(".starfield-overlay").Should().BeEmpty();
    }

    [Fact]
    public void Starfield_ShouldApplyCssClass()
    {
        var cut = Render<Starfield>(parameters => parameters
            .Add(p => p.CssClass, "my-overlay")
            .Add(p => p.ChildContent, "<p>Content</p>"));

        cut.Find(".starfield-overlay").ClassList.Should().Contain("my-overlay");
    }

    [Fact]
    public void Starfield_ShouldApplyCanvasOpacity()
    {
        var cut = Render<Starfield>(parameters => parameters
            .Add(p => p.Opacity, 0.5));

        var canvas = cut.Find("canvas");
        canvas.GetAttribute("style").Should().Contain("0.50");
    }

    [Fact]
    public void Starfield_ShouldRenderWithCustomStarCount()
    {
        var cut = Render<Starfield>(parameters => parameters
            .Add(p => p.StarCount, 2000));

        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void Starfield_ShouldRenderWithCustomColors()
    {
        var cut = Render<Starfield>(parameters => parameters
            .Add(p => p.StarColor, "#60a5fa")
            .Add(p => p.BackgroundColor, "#0c1445"));

        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void Starfield_ShouldRenderWithWarpDrivePreset()
    {
        var preset = StarfieldPresets.WarpDrive;
        var cut = Render<Starfield>(parameters => parameters
            .Add(p => p.StarCount, preset.StarCount)
            .Add(p => p.StarColor, preset.StarColor)
            .Add(p => p.Speed, preset.Speed)
            .Add(p => p.TrailLength, preset.TrailLength)
            .Add(p => p.Depth, preset.Depth));

        cut.Find("canvas").Should().NotBeNull();
    }

    [Fact]
    public void Starfield_ShouldRenderWithTrailDisabled()
    {
        var cut = Render<Starfield>(parameters => parameters
            .Add(p => p.TrailLength, 0.0));

        cut.Find("canvas").Should().NotBeNull();
    }
}
