namespace Presentation.Theming;

public interface IThemeService
{
    Theme CurrentTheme { get; }
    event EventHandler<Theme>? ThemeChanged;
    void SetTheme(Theme theme);
    void SetMode(ThemeMode mode);
    void SetTokenOverride(string tokenName, string value);
    void ClearTokenOverrides();
}
