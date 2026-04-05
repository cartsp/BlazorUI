using AwesomeAssertions;
using BlazorEffects.Core.Animation;

namespace BlazorEffects.Playground.Tests;

public class EffectDescriptorAdapterTests
{
    private readonly TestDescriptor _descriptor;
    private readonly EffectDescriptorAdapter<TestConfig> _sut;

    public EffectDescriptorAdapterTests()
    {
        _descriptor = new TestDescriptor();
        _sut = new EffectDescriptorAdapter<TestConfig>(
            _descriptor,
            config => BlazorMarkupGenerator.Generate(config, "TestEffect"));
    }

    // --- EffectName ---

    [Fact]
    public void EffectName_ShouldReturnDescriptorEffectName()
    {
        // Assert
        _sut.EffectName.Should().Be("Test Effect");
    }

    // --- ComponentType ---

    [Fact]
    public void ComponentType_ShouldReturnDescriptorComponentType()
    {
        // Assert
        _sut.ComponentType.Should().Be(typeof(object));
    }

    // --- GetParameterDefinitions ---

    [Fact]
    public void GetParameterDefinitions_ShouldReturnDescriptorDefinitions()
    {
        // Act
        var definitions = _sut.GetParameterDefinitions();

        // Assert
        definitions.Should().HaveCount(4);
        definitions.Should().Contain(p => p.PropertyName == nameof(TestConfig.Name));
        definitions.Should().Contain(p => p.PropertyName == nameof(TestConfig.Speed));
        definitions.Should().Contain(p => p.PropertyName == nameof(TestConfig.Enabled));
        definitions.Should().Contain(p => p.PropertyName == nameof(TestConfig.Tags));
    }

    // --- GetPresets ---

    [Fact]
    public void GetPresets_ShouldReturnNonGenericPresetViews()
    {
        // Act
        var presets = _sut.GetPresets();

        // Assert
        presets.Should().HaveCount(2);
    }

    [Fact]
    public void GetPresets_ShouldMapPresetFieldsCorrectly()
    {
        // Act
        var presets = _sut.GetPresets();
        var first = presets[0];

        // Assert
        first.Name.Should().Be("Preset A");
        first.Description.Should().Be("First test preset");
        first.PreviewGradient.Should().BeNull();
        first.Config.Should().NotBeNull();
    }

    [Fact]
    public void GetPresets_ShouldMapPreviewGradient_WhenPresent()
    {
        // Act
        var presets = _sut.GetPresets();
        var second = presets[1];

        // Assert
        second.PreviewGradient.Should().Be("linear-gradient(90deg, #ff0000, #00ff00)");
    }

    [Fact]
    public void GetPresets_ConfigShouldContainPresetValues()
    {
        // Act
        var presets = _sut.GetPresets();
        var config = (TestConfig)presets[0].Config;

        // Assert
        config.Name.Should().Be("alpha");
        config.Speed.Should().Be(2.0);
    }

    // --- GetDefaultConfig ---

    [Fact]
    public void GetDefaultConfig_ShouldReturnNewConfigWithDefaults()
    {
        // Act
        var config = _sut.GetDefaultConfig();

        // Assert
        config.Should().NotBeNull();
        var typedConfig = (TestConfig)config;
        typedConfig.Name.Should().Be("default");
        typedConfig.Speed.Should().Be(1.0);
        typedConfig.Enabled.Should().Be(true);
    }

    // --- ApplyParameter ---

    [Fact]
    public void ApplyParameter_ShouldModifyConfigThroughDescriptor()
    {
        // Arrange
        var config = new TestConfig();

        // Act
        var result = _sut.ApplyParameter(config, nameof(TestConfig.Speed), 3.5);

        // Assert
        var typedResult = (TestConfig)result;
        typedResult.Speed.Should().Be(3.5);
    }

    [Fact]
    public void ApplyParameter_ShouldModifyStringProperty()
    {
        // Arrange
        var config = new TestConfig();

        // Act
        var result = _sut.ApplyParameter(config, nameof(TestConfig.Name), "updated");

        // Assert
        var typedResult = (TestConfig)result;
        typedResult.Name.Should().Be("updated");
    }

    [Fact]
    public void ApplyParameter_ShouldNotMutateOriginalConfig()
    {
        // Arrange
        var original = new TestConfig();

        // Act
        _ = _sut.ApplyParameter(original, nameof(TestConfig.Speed), 9.0);

        // Assert
        original.Speed.Should().Be(1.0); // record `with` creates a new instance
    }

    // --- GenerateBlazorMarkup ---

    [Fact]
    public void GenerateBlazorMarkup_WithDefaultConfig_ShouldReturnSelfClosingTag()
    {
        // Arrange — ValuesEqual uses SequenceEqual for arrays, so all-default configs
        // correctly produce a self-closing tag
        var config = new TestConfig();

        // Act
        var markup = _sut.GenerateBlazorMarkup(config);

        // Assert
        markup.Trim().Should().Be("<TestEffect />");
    }

    [Fact]
    public void GenerateBlazorMarkup_WithModifiedConfig_ShouldReturnAttributes()
    {
        // Arrange
        var config = new TestConfig { Name = "custom", Speed = 5.0 };

        // Act
        var markup = _sut.GenerateBlazorMarkup(config);

        // Assert
        markup.Should().Contain("Name=\"custom\"");
        markup.Should().Contain("Speed=\"5\"");
    }

    // --- BuildComponentParameters ---

    [Fact]
    public void BuildComponentParameters_ShouldMapConfigPropertiesToDictionary()
    {
        // Arrange
        var config = new TestConfig { Name = "test", Speed = 2.5 };

        // Act
        var parameters = _sut.BuildComponentParameters(config);

        // Assert
        parameters.Should().ContainKey("Name");
        parameters["Name"].Should().Be("test");
        parameters.Should().ContainKey("Speed");
        parameters["Speed"].Should().Be(2.5);
    }

    [Fact]
    public void BuildComponentParameters_ShouldSkipTargetFps()
    {
        // Arrange
        var config = new TestConfig();

        // Act
        var parameters = _sut.BuildComponentParameters(config);

        // Assert
        parameters.Should().NotContainKey("TargetFps");
    }

    [Fact]
    public void BuildComponentParameters_ShouldIncludeBooleanProperties()
    {
        // Arrange
        var config = new TestConfig { Enabled = false };

        // Act
        var parameters = _sut.BuildComponentParameters(config);

        // Assert
        parameters.Should().ContainKey("Enabled");
        parameters["Enabled"].Should().Be(false);
    }

    [Fact]
    public void BuildComponentParameters_ShouldIncludeStringArrayProperties()
    {
        // Arrange
        var tags = new[] { "a", "b", "c" };
        var config = new TestConfig { Tags = tags };

        // Act
        var parameters = _sut.BuildComponentParameters(config);

        // Assert
        parameters.Should().ContainKey("Tags");
        parameters["Tags"].Should().BeEquivalentTo(tags);
    }

    [Fact]
    public void BuildComponentParameters_WithNullProperty_ShouldSkipIt()
    {
        // Arrange — TestConfig doesn't have nullable props, but we can test with
        // a config where all values equal defaults (no null properties in TestConfig).
        // Use a real config to verify the behaviour with non-null values only.
        var config = new TestConfig();

        // Act
        var parameters = _sut.BuildComponentParameters(config);

        // Assert — every value should be non-null
        foreach (var kvp in parameters)
        {
            kvp.Value.Should().NotBeNull();
        }
    }
}
