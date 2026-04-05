using BlazorEffects.Starfield;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.Starfield.Tests;

public class StarfieldConfigTests
{
    [Fact]
    public void DefaultConfig_ShouldHaveExpectedValues()
    {
        var config = new StarfieldConfig();

        config.StarCount.Should().Be(800);
        config.StarColor.Should().Be("#ffffff");
        config.StarSize.Should().Be(2.0);
        config.Speed.Should().Be(2.0);
        config.TrailLength.Should().Be(0.6);
        config.Depth.Should().Be(1000);
        config.BackgroundColor.Should().Be("#000000");
        config.Opacity.Should().Be(1.0);
        config.TargetFps.Should().Be(60);
    }

    [Fact]
    public void Config_ShouldImplementIEffectConfig()
    {
        var config = new StarfieldConfig();
        config.Should().BeAssignableTo<IEffectConfig>();
    }

    [Fact]
    public void Config_WithCustomValues_ShouldStoreCorrectly()
    {
        var config = new StarfieldConfig
        {
            StarCount = 1500,
            StarColor = "#60a5fa",
            StarSize = 3.0,
            Speed = 5.0,
            TrailLength = 0.8,
            Depth = 2000,
            BackgroundColor = "#0c1445",
            Opacity = 0.7,
            TargetFps = 30
        };

        config.StarCount.Should().Be(1500);
        config.StarColor.Should().Be("#60a5fa");
        config.StarSize.Should().Be(3.0);
        config.Speed.Should().Be(5.0);
        config.TrailLength.Should().Be(0.8);
        config.Depth.Should().Be(2000);
        config.BackgroundColor.Should().Be("#0c1445");
        config.Opacity.Should().Be(0.7);
        config.TargetFps.Should().Be(30);
    }

    [Fact]
    public void ConfigRecords_WithSameValues_ShouldBeEqual()
    {
        var config1 = new StarfieldConfig { StarColor = "#ffffff", Speed = 3.0 };
        var config2 = new StarfieldConfig { StarColor = "#ffffff", Speed = 3.0 };

        config1.Should().Be(config2);
        config1.GetHashCode().Should().Be(config2.GetHashCode());
    }

    [Fact]
    public void ConfigRecords_WithDifferentValues_ShouldNotBeEqual()
    {
        var config1 = new StarfieldConfig { StarColor = "#ffffff" };
        var config2 = new StarfieldConfig { StarColor = "#60a5fa" };

        config1.Should().NotBe(config2);
    }
}
