using BlazorEffects.Ripple;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.Ripple.Tests;

public class RippleConfigTests
{
    [Fact]
    public void DefaultConfig_ShouldHaveExpectedValues()
    {
        var config = new RippleConfig();

        config.MaxRipples.Should().Be(20);
        config.MaxRadius.Should().Be(300);
        config.Speed.Should().Be(3.0);
        config.Color.Should().Be("#60a5fa");
        config.LineWidth.Should().Be(2.0);
        config.Decay.Should().Be(0.02);
        config.Trigger.Should().Be("auto");
        config.AutoRippleCount.Should().Be(5);
        config.AutoInterval.Should().Be(800);
        config.BackgroundColor.Should().Be("#0f172a");
        config.Opacity.Should().Be(1.0);
        config.TargetFps.Should().Be(60);
    }

    [Fact]
    public void Config_ShouldImplementIEffectConfig()
    {
        var config = new RippleConfig();
        config.Should().BeAssignableTo<IEffectConfig>();
    }

    [Fact]
    public void Config_WithCustomValues_ShouldStoreCorrectly()
    {
        var config = new RippleConfig
        {
            MaxRipples = 30,
            MaxRadius = 500,
            Speed = 5.0,
            Color = "#22d3ee",
            LineWidth = 3.0,
            Decay = 0.01,
            Trigger = "click",
            AutoRippleCount = 0,
            AutoInterval = 400,
            BackgroundColor = "#020617",
            Opacity = 0.8,
            TargetFps = 30
        };

        config.MaxRipples.Should().Be(30);
        config.MaxRadius.Should().Be(500);
        config.Speed.Should().Be(5.0);
        config.Color.Should().Be("#22d3ee");
        config.LineWidth.Should().Be(3.0);
        config.Decay.Should().Be(0.01);
        config.Trigger.Should().Be("click");
        config.AutoRippleCount.Should().Be(0);
        config.AutoInterval.Should().Be(400);
        config.BackgroundColor.Should().Be("#020617");
        config.Opacity.Should().Be(0.8);
        config.TargetFps.Should().Be(30);
    }

    [Fact]
    public void ConfigRecords_WithSameValues_ShouldBeEqual()
    {
        var config1 = new RippleConfig { Color = "#60a5fa", Speed = 3.0 };
        var config2 = new RippleConfig { Color = "#60a5fa", Speed = 3.0 };

        config1.Should().Be(config2);
        config1.GetHashCode().Should().Be(config2.GetHashCode());
    }

    [Fact]
    public void ConfigRecords_WithDifferentValues_ShouldNotBeEqual()
    {
        var config1 = new RippleConfig { Color = "#60a5fa" };
        var config2 = new RippleConfig { Color = "#22d3ee" };

        config1.Should().NotBe(config2);
    }
}
