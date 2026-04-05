namespace Presentation.Theming;

public enum ThemeMode
{
    Light,
    Dark
}

public sealed record Theme(
    ThemeMode Mode,
    string Name,
    Dictionary<string, string> TokenOverrides)
{
    public static Theme Light => new(ThemeMode.Light, "Light", []);
    public static Theme Dark => new(ThemeMode.Dark, "Dark", []);

    public Theme WithTokenOverride(string tokenName, string value)
        => this with { TokenOverrides = new Dictionary<string, string>(TokenOverrides) { [tokenName] = value } };

    public Theme ClearTokenOverrides()
        => this with { TokenOverrides = [] };
}
