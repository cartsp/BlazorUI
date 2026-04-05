using BlazorEffects.Blobs;
using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.Blobs.Tests;

public class MorphingBlobsConfigTests
{
    [Fact]
    public void DefaultConfig_ShouldHaveExpectedValues()
    {
        var config = new MorphingBlobsConfig();

        config.BlobCount.Should().Be(4);
        config.Colors.Should().BeEquivalentTo(["#6366f1", "#ec4899", "#f97316", "#06b6d4"]);
        config.BlobSize.Should().Be(300);
        config.Speed.Should().Be(0.005);
        config.MorphIntensity.Should().Be(80);
        config.BlendMode.Should().Be("screen");
        config.Opacity.Should().Be(0.7);
        config.TargetFps.Should().Be(60);
    }

    [Fact]
    public void Config_ShouldImplementIEffectConfig()
    {
        var config = new MorphingBlobsConfig();
        config.Should().BeAssignableTo<IEffectConfig>();
    }

    [Fact]
    public void Config_WithCustomValues_ShouldStoreCorrectly()
    {
        var config = new MorphingBlobsConfig
        {
            BlobCount = 6,
            Colors = ["#ff0000", "#00ff00", "#0000ff"],
            BlobSize = 200,
            Speed = 0.01,
            MorphIntensity = 100,
            BlendMode = "lighter",
            Opacity = 0.5,
            TargetFps = 30
        };

        config.BlobCount.Should().Be(6);
        config.Colors.Should().BeEquivalentTo(["#ff0000", "#00ff00", "#0000ff"]);
        config.BlobSize.Should().Be(200);
        config.Speed.Should().Be(0.01);
        config.MorphIntensity.Should().Be(100);
        config.BlendMode.Should().Be("lighter");
        config.Opacity.Should().Be(0.5);
        config.TargetFps.Should().Be(30);
    }

    [Fact]
    public void ConfigRecords_WithSameValues_ShouldHaveSameBlobCount()
    {
        var config1 = new MorphingBlobsConfig { BlobCount = 5, Speed = 0.008 };
        var config2 = new MorphingBlobsConfig { BlobCount = 5, Speed = 0.008 };

        config1.BlobCount.Should().Be(config2.BlobCount);
        config1.Speed.Should().Be(config2.Speed);
        config1.BlendMode.Should().Be(config2.BlendMode);
        config1.Opacity.Should().Be(config2.Opacity);
    }

    [Fact]
    public void ConfigRecords_WithDifferentBlobCount_ShouldHaveDifferentValues()
    {
        var config1 = new MorphingBlobsConfig { BlobCount = 4 };
        var config2 = new MorphingBlobsConfig { BlobCount = 6 };

        config1.BlobCount.Should().NotBe(config2.BlobCount);
    }

    [Fact]
    public void Config_WithSameColorArrays_ShouldMatchElementWise()
    {
        var colors1 = new[] { "#ff0000", "#00ff00" };
        var colors2 = new[] { "#ff0000", "#00ff00" };
        var config1 = new MorphingBlobsConfig { Colors = colors1 };
        var config2 = new MorphingBlobsConfig { Colors = colors2 };

        config1.Colors.Should().BeEquivalentTo(config2.Colors);
    }
}
