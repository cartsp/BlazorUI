using Microsoft.AspNetCore.Components;

namespace Presentation.Components;

public enum ButtonVariant
{
    Primary,
    Secondary,
    Danger
}

public enum ButtonSize
{
    Small,
    Medium,
    Large
}

public class ButtonBase : ComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public ButtonVariant Variant { get; set; } = ButtonVariant.Primary;
    [Parameter] public ButtonSize Size { get; set; } = ButtonSize.Medium;
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public string? AdditionalClass { get; set; }

    protected string GetCssClass()
    {
        var baseClasses = "inline-flex items-center justify-center font-medium rounded-lg transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed";

        var variantClasses = Variant switch
        {
            ButtonVariant.Primary => "bg-blue-600 text-white hover:bg-blue-700 focus:ring-blue-500",
            ButtonVariant.Secondary => "bg-gray-200 text-gray-900 hover:bg-gray-300 focus:ring-gray-500",
            ButtonVariant.Danger => "bg-red-600 text-white hover:bg-red-700 focus:ring-red-500",
            _ => "bg-blue-600 text-white hover:bg-blue-700 focus:ring-blue-500"
        };

        var sizeClasses = Size switch
        {
            ButtonSize.Small => "px-3 py-1.5 text-sm",
            ButtonSize.Medium => "px-4 py-2 text-base",
            ButtonSize.Large => "px-6 py-3 text-lg",
            _ => "px-4 py-2 text-base"
        };

        return $"{baseClasses} {variantClasses} {sizeClasses} {AdditionalClass}".Trim();
    }
}
