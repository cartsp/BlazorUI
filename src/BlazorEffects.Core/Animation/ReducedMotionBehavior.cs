namespace BlazorEffects.Core.Animation;

/// <summary>
/// Defines how an effect should respond when the user has enabled
/// prefers-reduced-motion in their OS/browser accessibility settings.
/// </summary>
public enum ReducedMotionBehavior
{
    /// <summary>
    /// Run the effect at 10% of normal speed — very slow animation instead of full stop.
    /// This is the default to preserve visual intent while respecting the preference.
    /// </summary>
    Minimal = 0,

    /// <summary>
    /// Pause the effect entirely (speed = 0). Use for effects where even slow motion
    /// could be distracting (e.g., rapidly flashing particles).
    /// </summary>
    Pause = 1,

    /// <summary>
    /// Ignore the reduced-motion preference and run at full speed.
    /// Use sparingly — only when the effect is essential to functionality.
    /// </summary>
    Ignore = 2
}
