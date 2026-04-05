using Microsoft.Extensions.DependencyInjection;
using Presentation.Theming;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddScoped<IThemeService, ThemeService>();
        return services;
    }
}
