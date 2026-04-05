using Microsoft.JSInterop;

namespace Presentation.Theming;

public sealed class ThemeService : IThemeService, IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private Theme _currentTheme = Theme.Light;

    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public Theme CurrentTheme => _currentTheme;

    public event EventHandler<Theme>? ThemeChanged;

    public void SetTheme(Theme theme)
    {
        _currentTheme = theme;
        _ = ApplyThemeAsync(theme);
        ThemeChanged?.Invoke(this, theme);
    }

    public void SetMode(ThemeMode mode)
    {
        var newTheme = _currentTheme with { Mode = mode, Name = mode.ToString() };
        SetTheme(newTheme);
    }

    public void SetTokenOverride(string tokenName, string value)
    {
        _currentTheme = _currentTheme.WithTokenOverride(tokenName, value);
        _ = ApplyTokenOverridesAsync(_currentTheme);
        ThemeChanged?.Invoke(this, _currentTheme);
    }

    public void ClearTokenOverrides()
    {
        _currentTheme = _currentTheme.ClearTokenOverrides();
        _ = ApplyThemeAsync(_currentTheme);
        ThemeChanged?.Invoke(this, _currentTheme);
    }

    private async Task ApplyThemeAsync(Theme theme)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("eval",
                $"document.documentElement.setAttribute('data-theme', '{theme.Mode.ToString().ToLowerInvariant()}')");
            await ApplyTokenOverridesAsync(theme);
        }
        catch (JSDisconnectedException)
        {
            // JS interop not available during prerendering
        }
    }

    private async Task ApplyTokenOverridesAsync(Theme theme)
    {
        if (theme.TokenOverrides.Count == 0) return;

        try
        {
            foreach (var (tokenName, value) in theme.TokenOverrides)
            {
                await _jsRuntime.InvokeVoidAsync("eval",
                    $"document.documentElement.style.setProperty('--{tokenName}', '{value}')");
            }
        }
        catch (JSDisconnectedException)
        {
            // JS interop not available during prerendering
        }
    }

    public ValueTask DisposeAsync()
    {
        ThemeChanged = null;
        return ValueTask.CompletedTask;
    }
}
