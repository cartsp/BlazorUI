# Accessibility Guide — BlazorEffects

> **QA Review Date:** 2026-04-05
> **Status:** Audit complete, critical gaps identified

## Overview

BlazorEffects uses HTML5 `<canvas>` elements for all visual effects. Canvas-based animations present unique accessibility challenges because they are inherently visual and not accessible to screen readers or assistive technology in their rendered output.

## Current State

### ❌ Critical Gap: `prefers-reduced-motion` Support

**Status:** Only 1 of 10 effects respects `prefers-reduced-motion`.

| Effect | Respects `prefers-reduced-motion` | Notes |
|--------|----------------------------------|-------|
| MatrixRain | ❌ No | Continuous animation with no pause |
| Morphing Blobs | ❌ No | Continuous animation with no pause |
| Particles | ❌ No | Continuous animation with no pause |
| Aurora Borealis | ❌ No | Continuous animation with no pause |
| Noise Field | ❌ No | Continuous animation with no pause |
| Gradient Waves | ✅ Yes | Freezes animation, renders single static frame |
| Starfield | ❌ No | Continuous animation with no pause |
| Fire/Embers | ❌ No | Continuous animation with no pause |
| Ripple | ❌ No | Event-triggered, but no reduced-motion handling |
| Vortex/Tunnel | ❌ No | Continuous animation with no pause |

**Recommendation:** Add a shared `prefers-reduced-motion` check to `blazor-effects-core.js` that all effects use:

```javascript
// blazor-effects-core.js — proposed addition
export function prefersReducedMotion() {
    return window.matchMedia?.('(prefers-reduced-motion: reduce)')?.matches ?? false;
}
```

Each effect should:
1. Check this on init — if true, render a single static frame and skip the animation loop
2. Listen for `change` events on the media query — toggle animation on/off dynamically
3. Expose a `ReducedMotionBehavior` enum in C#: `Pause`, `StaticFrame`, `ReduceIntensity`

### ❌ Critical Gap: Canvas ARIA Attributes

**Status:** No canvas element has `aria-hidden="true"` or `role="presentation"`.

Canvas elements are invisible to screen readers by default, but without explicit `aria-hidden="true"`:
- Screen readers may announce an unlabeled image/graphic
- The canvas may appear in the accessibility tree as an empty element

**Recommendation:** Add to `EffectComponentBase` rendering:

```razor
<canvas @ref="CanvasElement"
        aria-hidden="true"
        role="presentation"
        style="..."></canvas>
```

### ✅ Screen Reader Impact

Canvas elements are correctly excluded from the accessibility tree by nature (canvas has no internal DOM). The concerns are:
- **Announcement noise** — without `aria-hidden`, screen readers may announce "graphic" or "image"
- **Missing description** — no `aria-label` or `aria-describedby` on the canvas
- **Solution:** `aria-hidden="true"` eliminates both concerns

### ✅ Keyboard Navigation

Canvas animations do not interfere with keyboard navigation:
- No focusable elements within the canvas
- Tab order flows naturally through sibling/parent content
- Effect parameters in the Playground UI are standard Blazor inputs (keyboard accessible)

### ✅ Seizure Risk Assessment

All effects were reviewed for photosensitive seizure risk per WCAG 2.3.1 (three flashes or below threshold):
- **No effect exceeds 3 flashes per second**
- MatrixRain: Continuous scrolling text (no flashing)
- Blobs: Smooth morphing (no rapid changes)
- Particles: Gentle drift (no flashing)
- Aurora: Slow wave motion
- Noise: Smooth field evolution
- Gradient Waves: Slow undulation
- Starfield: Slow drift with twinkle
- Fire/Embers: Rising particle simulation
- Ripple: Expanding rings (event-triggered, not repetitive)
- Vortex/Tunnel: Smooth rotation

**All effects pass WCAG 2.3.1 — Three Flashes or Below Threshold.**

## Contrast & Text Overlay

When effects overlay text content (via `ChildContent`), contrast ratios depend on the effect's opacity and colors:

| Effect | Default Opacity | Text Overlay Risk | Notes |
|--------|----------------|-------------------|-------|
| MatrixRain | 0.8 | 🟡 Medium | Green text on dark bg can blend |
| Morphing Blobs | 0.7 | 🔴 High | Colorful blobs behind text reduce contrast |
| Particles | 0.6 | 🟡 Medium | Semi-transparent connections |
| Aurora | 0.5 | 🔴 High | Colorful ribbons with screen blend |
| Noise | N/A | 🟢 Low | Monochrome, less contrast interference |
| Gradient Waves | N/A | 🟡 Medium | Depends on gradient colors |
| Starfield | N/A | 🟢 Low | Dark background, small bright dots |
| Fire/Embers | N/A | 🟡 Medium | Orange/red particles on dark bg |
| Ripple | N/A | 🟢 Low | Subtle rings, low interference |
| Vortex/Tunnel | N/A | 🟡 Medium | Concentric shapes behind text |

**Recommendation for text overlay:**
- Use `z-index: 1` on the overlay div (already implemented)
- Add a semi-transparent background to the overlay div for guaranteed contrast
- Document recommended text colors per effect in the component docs

## Recommendations

### P0 — Must fix before v0.3.0

1. **Add `aria-hidden="true"` and `role="presentation"`** to all canvas elements in `EffectComponentBase`
2. **Add `prefers-reduced-motion` support** — shared utility in core JS + per-effect handling
3. **Add `prefers-reduced-motion` guidance** to component docs — explain the behavior

### P1 — Should fix soon

4. **Add overlay background recommendation** — suggest semi-transparent overlay for text readability
5. **Add `ReducedMotionBehavior` config option** — let developers choose how effects behave
6. **Document contrast considerations** per effect in component usage docs

### P2 — Nice to have

7. **Consider `aria-describedby`** for canvas effects with text overlay — describe the visual effect
8. **Add `prefers-color-scheme` support** — dark/light aware effect defaults
9. **Test with actual screen readers** (VoiceOver, NVDA, JAWS)

## Effect-Specific Notes

### MatrixRain
- High-motion effect — `prefers-reduced-motion: reduce` should show static column of characters
- Green-on-black is high contrast but the trailing fade can reduce readability

### Morphing Blobs
- `screen` blend mode can wash out text — consider recommending dark text only
- High-opacity blobs behind white text can fail WCAG AA

### Noise Field
- Performance-heavy — reduced motion should also reduce octaves (lower GPU/CPU)
- Monochrome output is generally less distracting

### Fire/Embers
- Orange/red particles on dark background — warm colors less likely to trigger motion sensitivity
- Rising direction is predictable, reducing vestibular discomfort

## WCAG Compliance Matrix

| Criterion | Level | Status |
|-----------|-------|--------|
| 1.1.1 Non-text Content | A | ❌ No `aria-hidden` on canvas |
| 1.4.3 Contrast (Minimum) | AA | ⚠️ Depends on usage context |
| 1.4.11 Non-text Contrast | AA | ✅ Decorative — exempt |
| 2.2.2 Pause, Stop, Hide | A | ❌ No pause/stop control |
| 2.3.1 Three Flashes | A | ✅ No effects exceed threshold |
| 2.3.2 Three Flashes | AAA | ✅ No effects exceed threshold |
| 4.1.2 Name, Role, Value | A | ❌ Canvas needs `aria-hidden` |

## Summary

BlazorEffects has **two critical accessibility gaps** that should be addressed before v0.3.0:
1. Missing `prefers-reduced-motion` support (9 of 10 effects don't respect it)
2. Missing ARIA attributes on canvas elements

Both are straightforward to fix in `EffectComponentBase` and the core JS module.
