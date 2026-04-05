/**
 * BlazorEffects.Core — Reduced Motion helper
 *
 * Provides a shared utility for respecting the user's prefers-reduced-motion
 * OS/browser setting. Effects call checkReducedMotion() in their init() to
 * determine whether to pause, run minimally, or ignore the preference.
 *
 * Exports:
 *   checkReducedMotion() — returns 'pause' | 'minimal' | 'ignore'
 *   getReducedMotionMediaQuery() — returns the MediaQueryList (for addListener)
 *   adjustSpeedForReducedMotion(speed, behavior) — returns adjusted speed
 */

const REDUCED_MOTION_MEDIA = '(prefers-reduced-motion: reduce)';

/**
 * Check if the user prefers reduced motion.
 * @param {string} behavior - 'Pause', 'Minimal', or 'Ignore' (from C# ReducedMotionBehavior enum)
 * @returns {string} 'pause' | 'minimal' | 'ignore'
 */
export function checkReducedMotion(behavior) {
    if (!behavior || behavior === 'Ignore') return 'ignore';

    const prefersReduced = window.matchMedia(REDUCED_MOTION_MEDIA).matches;
    if (!prefersReduced) return 'ignore';

    if (behavior === 'Pause') return 'pause';
    return 'minimal'; // Default: Minimal
}

/**
 * Get the MediaQueryList for prefers-reduced-motion.
 * Use this to add change listeners for dynamic updates.
 * @returns {MediaQueryList}
 */
export function getReducedMotionMediaQuery() {
    return window.matchMedia(REDUCED_MOTION_MEDIA);
}

/**
 * Adjust animation speed based on reduced-motion behavior.
 * @param {number} speed - Original speed value
 * @param {string} behavior - 'Pause', 'Minimal', or 'Ignore'
 * @returns {number} Adjusted speed (0 for pause, 0.1 for minimal, original for ignore)
 */
export function adjustSpeedForReducedMotion(speed, behavior) {
    const result = checkReducedMotion(behavior);
    if (result === 'pause') return 0;
    if (result === 'minimal') return Math.max(0.1, speed * 0.1);
    return speed;
}
