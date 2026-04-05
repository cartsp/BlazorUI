using Presentation.Tokens;

namespace Presentation.Tokens;

/// <summary>
/// Central access point for all design tokens with pre-defined values.
/// </summary>
public static class DesignTokens
{
    // Colors
    public static ColorToken Primary { get; } = new("primary", "#3b82f6", "Primary brand color");
    public static ColorToken Secondary { get; } = new("secondary", "#64748b", "Secondary color");
    public static ColorToken Accent { get; } = new("accent", "#8b5cf6", "Accent color");
    public static ColorToken Success { get; } = new("success", "#22c55e", "Success state");
    public static ColorToken Warning { get; } = new("warning", "#f59e0b", "Warning state");
    public static ColorToken Error { get; } = new("error", "#ef4444", "Error state");

    // Spacing
    public static SpacingToken Space0 { get; } = new("0", "0px");
    public static SpacingToken Space1 { get; } = new("1", "0.25rem");
    public static SpacingToken Space2 { get; } = new("2", "0.5rem");
    public static SpacingToken Space3 { get; } = new("3", "0.75rem");
    public static SpacingToken Space4 { get; } = new("4", "1rem");
    public static SpacingToken Space5 { get; } = new("5", "1.25rem");
    public static SpacingToken Space6 { get; } = new("6", "1.5rem");
    public static SpacingToken Space8 { get; } = new("8", "2rem");
    public static SpacingToken Space10 { get; } = new("10", "2.5rem");
    public static SpacingToken Space12 { get; } = new("12", "3rem");
    public static SpacingToken Space16 { get; } = new("16", "4rem");
    public static SpacingToken Space20 { get; } = new("20", "5rem");
    public static SpacingToken Space24 { get; } = new("24", "6rem");

    public static IReadOnlyList<SpacingToken> AllSpacing =>
    [
        Space0, Space1, Space2, Space3, Space4, Space5, Space6,
        Space8, Space10, Space12, Space16, Space20, Space24
    ];

    // Typography — Font sizes
    public static TypographyToken TextXs { get; } = new("xs", "0.75rem");
    public static TypographyToken TextSm { get; } = new("sm", "0.875rem");
    public static TypographyToken TextBase { get; } = new("base", "1rem");
    public static TypographyToken TextLg { get; } = new("lg", "1.125rem");
    public static TypographyToken TextXl { get; } = new("xl", "1.25rem");
    public static TypographyToken Text2xl { get; } = new("2xl", "1.5rem");
    public static TypographyToken Text3xl { get; } = new("3xl", "1.875rem");
    public static TypographyToken Text4xl { get; } = new("4xl", "2.25rem");

    // Typography — Weights
    public static TypographyToken FontWeightNormal { get; } = new("normal", "400");
    public static TypographyToken FontWeightMedium { get; } = new("medium", "500");
    public static TypographyToken FontWeightSemibold { get; } = new("semibold", "600");
    public static TypographyToken FontWeightBold { get; } = new("bold", "700");

    public static IReadOnlyList<TypographyToken> AllTypography =>
    [
        TextXs, TextSm, TextBase, TextLg, TextXl, Text2xl, Text3xl, Text4xl,
        FontWeightNormal, FontWeightMedium, FontWeightSemibold, FontWeightBold
    ];

    // Shadows
    public static ShadowToken ShadowSm { get; } = new("sm", "0 1px 2px 0 rgb(0 0 0 / 0.05)");
    public static ShadowToken ShadowMd { get; } = new("md", "0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1)");
    public static ShadowToken ShadowLg { get; } = new("lg", "0 10px 15px -3px rgb(0 0 0 / 0.1), 0 4px 6px -4px rgb(0 0 0 / 0.1)");
    public static ShadowToken ShadowXl { get; } = new("xl", "0 20px 25px -5px rgb(0 0 0 / 0.1), 0 8px 10px -6px rgb(0 0 0 / 0.1)");

    public static IReadOnlyList<ShadowToken> AllShadows => [ShadowSm, ShadowMd, ShadowLg, ShadowXl];

    // Border Radius
    public static BorderRadiusToken RadiusNone { get; } = new("none", "0px");
    public static BorderRadiusToken RadiusSm { get; } = new("sm", "0.25rem");
    public static BorderRadiusToken RadiusMd { get; } = new("md", "0.375rem");
    public static BorderRadiusToken RadiusLg { get; } = new("lg", "0.5rem");
    public static BorderRadiusToken RadiusXl { get; } = new("xl", "0.75rem");
    public static BorderRadiusToken RadiusFull { get; } = new("full", "9999px");

    public static IReadOnlyList<BorderRadiusToken> AllBorderRadius =>
        [RadiusNone, RadiusSm, RadiusMd, RadiusLg, RadiusXl, RadiusFull];

    // Transitions
    public static TransitionToken TransitionFast { get; } = new("fast", "150ms ease");
    public static TransitionToken TransitionNormal { get; } = new("normal", "200ms ease");
    public static TransitionToken TransitionSlow { get; } = new("slow", "300ms ease");

    public static IReadOnlyList<TransitionToken> AllTransitions =>
        [TransitionFast, TransitionNormal, TransitionSlow];
}
