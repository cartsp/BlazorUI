namespace Presentation.Tokens;

public sealed record ColorToken(
    string Name,
    string Value,
    string? Description = null);

public sealed record SpacingToken(
    string Name,
    string Value,
    string? Description = null);

public sealed record TypographyToken(
    string Name,
    string Value,
    string? Description = null);

public sealed record ShadowToken(
    string Name,
    string Value,
    string? Description = null);

public sealed record BorderRadiusToken(
    string Name,
    string Value,
    string? Description = null);

public sealed record TransitionToken(
    string Name,
    string Value,
    string? Description = null);
