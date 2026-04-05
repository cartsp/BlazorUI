namespace BlazorEffects.VortexTunnel;

/// <summary>
/// Common presets for the VortexTunnel effect.
/// </summary>
public static class VortexTunnelPresets
{
    /// <summary>
    /// Default purple vortex — hypnotic spiraling tunnel.
    /// </summary>
    public static VortexTunnelConfig Default => new();

    /// <summary>
    /// Hypno wheel — multi-colored psychedelic rings.
    /// </summary>
    public static VortexTunnelConfig HypnoWheel => new()
    {
        Colors = ["#ef4444", "#f97316", "#eab308", "#22c55e", "#3b82f6", "#8b5cf6"],
        RingCount = 30,
        RotationSpeed = 0.04,
        ScaleFactor = 0.90,
        Shape = "polygon",
        PolygonSides = 6,
        LineWidth = 3.0,
        BackgroundColor = "#000000"
    };

    /// <summary>
    /// Deep space — dark blue tunnel with subtle rotation.
    /// </summary>
    public static VortexTunnelConfig DeepSpace => new()
    {
        Color = "#1e40af",
        RingCount = 25,
        RotationSpeed = 0.01,
        ScaleFactor = 0.93,
        Shape = "circle",
        LineWidth = 1.5,
        BackgroundColor = "#000510"
    };

    /// <summary>
    /// Cyber grid — square shapes creating a digital tunnel.
    /// </summary>
    public static VortexTunnelConfig CyberGrid => new()
    {
        Color = "#00ff41",
        RingCount = 20,
        RotationSpeed = 0.03,
        ScaleFactor = 0.88,
        Shape = "square",
        LineWidth = 1.5,
        BackgroundColor = "#0a0a0a"
    };

    /// <summary>
    /// Warm portal — orange/red spinning vortex.
    /// </summary>
    public static VortexTunnelConfig WarmPortal => new()
    {
        Colors = ["#dc2626", "#ea580c", "#f59e0b"],
        RingCount = 18,
        RotationSpeed = 0.025,
        ScaleFactor = 0.91,
        Shape = "polygon",
        PolygonSides = 8,
        LineWidth = 2.5,
        BackgroundColor = "#1a0500"
    };

    /// <summary>
    /// Ice crystal — light blue hexagonal vortex.
    /// </summary>
    public static VortexTunnelConfig IceCrystal => new()
    {
        Color = "#67e8f9",
        RingCount = 22,
        RotationSpeed = 0.015,
        ScaleFactor = 0.94,
        Shape = "polygon",
        PolygonSides = 6,
        LineWidth = 1.0,
        BackgroundColor = "#020617"
    };
}
