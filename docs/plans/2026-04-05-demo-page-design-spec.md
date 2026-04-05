# Demo Page Design Specification

> **Author:** UXDesigner (Head of UI/UX Design & Front-End)  
> **Date:** 2026-04-05  
> **Issue:** [AIE-22](/AIE/issues/AIE-22)  
> **Design Direction:** [AIE-9](/AIE/issues/AIE-9)  
> **Status:** Draft — pending review

---

## 1. Architecture Review: EffectComponentBase

### Verdict: ✅ Solid foundation, minor improvements recommended

The `EffectComponentBase` architecture is clean and well-structured:

**Strengths:**
- Config-hash diffing prevents unnecessary JS interop calls — excellent performance pattern
- `IAsyncDisposable` ensures cleanup on component teardown
- `JSDisconnectedException` catch guards prevent teardown race conditions
- Module path abstraction lets each effect own its JS module

**Recommendations:**

| # | Issue | Recommendation | Priority |
|---|-------|----------------|----------|
| 1 | **No error boundary** — if `init()` throws, component silently fails | Add `OnError` callback parameter so demo pages can show fallback UI | Medium |
| 2 | **No loading state** — canvas is blank until JS module loads | Add `OnReady` event callback; demo pages can show a shimmer/skeleton | Medium |
| 3 | **`ComputeConfigHash` is manual string concat** — fragile, easy to forget new params | Consider `[ComputeConfigHash]` source generator or hash via `System.Text.Json` serialization | Low |
| 4 | **No `OnAfterRenderAsync` re-render guard** — `_previousConfigHash` only set on `firstRender`, but `OnParametersSetAsync` runs before render | The current code works because `Module` is null during first `OnParametersSetAsync`, but add a defensive comment | Low |

**Overall:** The CTO has built a genuinely good abstraction. The `init/update/dispose` JS contract is clean and the config-hash diffing is smart. No blockers for demo page work.

---

## 2. MatrixRain Component API Review

### Verdict: ✅ Well-designed API, aligned with design direction

**API Surface Review:**

| Parameter | Type | Default | Assessment |
|-----------|------|---------|------------|
| `Characters` | `string` | Classic preset | ✅ Preset system is elegant |
| `FontSize` | `double` | 16 | ✅ Good range for canvas columns |
| `FontFamily` | `string` | "monospace" | ✅ Sensible default |
| `Color` | `string` | "#00ff41" | ✅ Iconic matrix green |
| `FadeColor` | `string` | "#003b00" | ✅ Dark green for trail |
| `Speed` | `double` | 1.0 | ✅ Multiplier pattern is intuitive |
| `Density` | `double` | 1.0 | ✅ 0-1 range is standard |
| `Opacity` | `double` | 0.8 | ✅ Allows background visibility |
| `Height` | `string` | "100vh" | ✅ Inherited from base |
| `TargetFps` | `int` | 30 | ✅ Lower default is smart for this effect |
| `CssClass` | `string?` | null | ✅ Overlay styling hook |
| `ChildContent` | `RenderFragment?` | null | ✅ Content-over-effect pattern |

**Presets Review (`MatrixRainPresets`):**
- **Classic** — ✅ Perfect default, iconic
- **Katakana** — ✅ Authentic Matrix film feel
- **Binary** — ✅ Hacker aesthetic
- **Hex** — ✅ Technical/digital feel
- **Emojis** — 🤔 Fun but visually noisy at small sizes. Consider documenting that this preset works best with `FontSize >= 20`

**JS Implementation Notes:**
- 0.5x resolution scaling is smart — characters don't need pixel-perfect rendering
- Column rebuild on resize is correct
- The `fadeColor` overlay technique at `globalAlpha = 0.05` creates a nice trailing effect
- The random white "head" flash at 10% probability adds visual interest

---

## 3. Demo Page Design System

### 3.1 Layout Architecture

Every demo page follows the same **three-zone layout**:

```
┌──────────────────────────────────────────────────────────┐
│                    ZONE A: EFFECT                         │
│            (Full-viewport canvas effect)                  │
│                                                          │
│  ┌─────────────────────────────┐                         │
│  │   ZONE B: HEADER BAR       │  ← top-left, floating   │
│  │   Effect name + FPS badge  │                         │
│  └─────────────────────────────┘                         │
│                                                          │
│                          ┌─────────────────────────────┐ │
│                          │  ZONE C: CONTROL PANEL      │ │
│                          │  (right sidebar, collapsible)│ │
│                          │  ┌─────────────────────────┐│ │
│                          │  │ Presets bar             ││ │
│                          │  │ ┌────┐┌────┐┌────┐     ││ │
│                          │  │ │ P1 ││ P2 ││ P3 │     ││ │
│                          │  │ └────┘└────┘└────┘     ││ │
│                          │  ├─────────────────────────┤│ │
│                          │  │ Parameters              ││ │
│                          │  │ ─ Color picker          ││ │
│                          │  │ ─ Range sliders         ││ │
│                          │  │ ─ Dropdowns             ││ │
│                          │  ├─────────────────────────┤│ │
│                          │  │ Code Snippet            ││ │
│                          │  │ <MatrixRain ... />      ││ │
│                          │  │ [Copy] button           ││ │
│                          │  └─────────────────────────┘│ │
│                          └─────────────────────────────┘ │
└──────────────────────────────────────────────────────────┘
```

### 3.2 Zone Specifications

#### Zone A — Effect Preview (Full Screen)
- **Dimensions:** 100vw × 100vh, `position: fixed; inset: 0`
- **Purpose:** Immersive, immediate visual impact
- **Behavior:** Effect fills entire viewport on page load
- **Mobile:** Same — full-screen effect is the hero

#### Zone B — Header Bar (Top-Left Floating)
- **Position:** `fixed; top: 1.5rem; left: 1.5rem; z-index: 50`
- **Content:**
  - Effect name (e.g., "Matrix Rain") in white, `text-2xl font-bold`
  - Subtle text shadow for readability over any effect
  - FPS badge (top-right corner of header): small pill, `bg-black/50 backdrop-blur-sm`
  - "Back to Gallery" link with ← arrow
- **Style:** `bg-black/30 backdrop-blur-md rounded-xl px-5 py-3`
- **Mobile:** Same position, smaller padding

#### Zone C — Control Panel (Right Sidebar)
- **Position:** `fixed; top: 1.5rem; right: 1.5rem; bottom: 1.5rem; z-index: 40`
- **Width:** `20rem` (320px) on desktop, full-width bottom sheet on mobile
- **Behavior:** Collapsible with toggle button
- **Style:** `bg-black/60 backdrop-blur-xl rounded-2xl border border-white/10`
- **Sections (top to bottom):**
  1. **Panel header** — "Controls" title + collapse toggle icon
  2. **Preset selector** — horizontal scrollable pill buttons
  3. **Parameter controls** — organized by group
  4. **Code snippet** — auto-generated Blazor markup
- **Scroll:** Internal scroll for parameter section when content overflows

### 3.3 Control Panel Components

#### Preset Selector
```
┌──────────────────────────────────────┐
│  ● Classic   ○ Katakana   ○ Binary  │
│  ○ Hex       ○ Emojis               │
└──────────────────────────────────────┘
```
- **Active state:** Filled pill with effect's accent color + white text
- **Inactive:** `bg-white/10 text-white/70 hover:bg-white/20`
- **Interaction:** Click instantly applies preset (updates all bound parameters)
- **Animation:** 200ms transition on background color

#### Parameter Controls

Each parameter gets a labeled control:

**Color Pickers:**
```
Color                      #00ff41
┌──────────────────────┬──────┐
│  (native color input) │ ████ │
└──────────────────────┴──────┘
```
- Label above, hex value to the right of label
- Full-width color input
- `h-10 rounded-lg` styling

**Range Sliders:**
```
Speed                                1.0
━━━━━━━━━━━━━●━━━━━━━━━━━━━━━━━━━━━━━
 0.1                              3.0
```
- Label with current value to the right
- Custom-styled range input (we'll add custom CSS for the track/thumb)
- Min/max labels below in `text-xs text-white/40`

**Dropdown Selects:**
```
Character Set
┌────────────────────────────────────┐
│  Classic                      ▼    │
└────────────────────────────────────┘
```
- `bg-white/10 border border-white/20 rounded-lg px-3 py-2`
- White text on dark

#### Code Snippet Display
```
┌────────────────────────────────────┐
│  Blazor Markup              [Copy] │
│ ┌────────────────────────────────┐ │
│ │ <MatrixRain                   │ │
│ │   Color="#00ff41"             │ │
│ │   Speed="1.0"                │ │
│ │   Density="1.0" />           │ │
│ └────────────────────────────────┘ │
└────────────────────────────────────┘
```
- Monospace font (`font-mono`)
- `bg-black/40 rounded-lg` code block
- Copy button copies razor markup to clipboard
- Auto-updates when any parameter changes
- Syntax highlight: parameter names in green, values in amber

#### FPS Counter
```
┌──────────┐
│  60 FPS  │
└──────────┘
```
- Small pill badge in the header bar
- Updates every 500ms (not every frame — avoid layout thrash)
- Color coding: green ≥50fps, yellow 30-49fps, red <30fps
- Implemented via JS interop: `performance.now()` delta tracking

### 3.4 Panel Toggle (Collapse/Expand)

The control panel needs a toggle mechanism:

```
Collapsed state:
                                    ┌───┐
                                    │ ⚙ │  ← floating button
                                    └───┘

Expanded state:
                                    ┌──────────────┐
                                    │ Controls    ✕ │
                                    │              │
                                    │ (full panel) │
                                    └──────────────┘
```

- **Toggle button:** `fixed; right: 1.5rem; top: 1.5rem; z-index: 50`
- **Style:** `w-11 h-11 rounded-full bg-white/20 backdrop-blur-md hover:bg-white/30`
- **Icon:** Gear icon (⚙) when collapsed, X icon when expanded
- **Animation:** Panel slides in from right (300ms `transform: translateX`)
- **Mobile:** Panel becomes bottom sheet (slides up from bottom)

---

## 4. Reusable Demo Page Template Pattern

### 4.1 Component Architecture

Create a shared `EffectDemoLayout` component that all demo pages use:

```
EffectDemoLayout.razor          ← shared layout shell
├── HeaderBar                   ← effect name, back link, FPS
├── ControlPanel                ← collapsible sidebar
│   ├── PresetSelector          ← preset pill buttons
│   ├── ParameterGroup          ← section of related controls
│   │   ├── ColorParameter      ← color picker control
│   │   ├── RangeParameter      ← slider control
│   │   └── SelectParameter     ← dropdown control
│   └── CodeSnippet             ← auto-generated markup
└── (ChildContent = effect)     ← the actual effect component
```

### 4.2 EffectDemoLayout API

```razor
<EffectDemoLayout EffectName="Matrix Rain"
                  EffectIcon="🌧️"
                  Presets="@presets"
                  CurrentPreset="@currentPreset"
                  OnPresetSelected="ApplyPreset"
                  Parameters="@parameterGroups"
                  GenerateCode="@GenerateMarkup"
                  FpsValue="@currentFps">
    
    <EffectContent>
        <MatrixRain @ref="effectRef"
                    Height="100vh"
                    Color="@state.Color"
                    Speed="@state.Speed" 
                    /* ... */ />
    </EffectContent>
    
</EffectDemoLayout>
```

**Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `EffectName` | `string` | Display name in header |
| `EffectIcon` | `string` | Emoji/icon for branding |
| `Presets` | `List<PresetOption>` | Available presets |
| `CurrentPreset` | `string` | Active preset key |
| `OnPresetSelected` | `EventCallback<string>` | Preset click handler |
| `Parameters` | `List<ParameterGroup>` | Grouped parameter definitions |
| `GenerateCode` | `Func<string>` | Returns current Blazor markup |
| `FpsValue` | `int` | Current FPS reading |
| `EffectContent` | `RenderFragment` | The effect component |

### 4.3 Supporting Types

```csharp
public record PresetOption(string Key, string Label, string? Icon = null);

public record ParameterGroup(string Title, List<ParameterDef> Parameters);

public abstract record ParameterDef(string Name, string Label);

public record ColorParameter(string Name, string Label, string Value) 
    : ParameterDef(Name, Label);

public record RangeParameter(string Name, string Label, double Value, 
    double Min, double Max, double Step, string Format = "F1") 
    : ParameterDef(Name, Label);

public record SelectParameter(string Name, string Label, string Value, 
    List<SelectOption> Options) 
    : ParameterDef(Name, Label);

public record SelectOption(string Value, string Label);
```

### 4.4 File Structure

```
src/AppHost/
├── Pages/
│   ├── MatrixRainDemo.razor        ← existing, refactor to use template
│   ├── AuroraDemo.razor            ← future
│   ├── ParticlesDemo.razor         ← future
│   └── EffectsGallery.razor        ← landing page with all effects
├── Shared/
│   ├── EffectDemoLayout.razor      ← shared demo page shell
│   └── EffectDemoLayout.razor.css  ← scoped styles
└── wwwroot/
    └── js/
        └── fps-counter.js          ← FPS measurement utility
```

---

## 5. Color Palette Presets (All 5 Components)

### 5.1 Matrix Rain

| Preset | Color | Fade Color | Characters | Vibe |
|--------|-------|-----------|------------|------|
| **Classic** | `#00ff41` | `#003b00` | Classic | Iconic Matrix green |
| **Cyberpunk** | `#ff00ff` | `#1a0033` | Katakana | Neon magenta, anime aesthetic |
| **Terminal** | `#00ff88` | `#001a0d` | Binary | Hacker terminal green |
| **Ghost** | `#ffffff` | `#0a0a0a` | Classic | Ethereal white on black |
| **Sunset** | `#ff6b35` | `#1a0800` | Hex | Warm amber code rain |

### 5.2 Aurora Borealis

| Preset | Colors | Ribbon Count | Amplitude | Speed | Vibe |
|--------|--------|-------------|-----------|-------|------|
| **Northern Lights** | `#00ff87, #7b2ff7, #00b4d8` | 4 | 120 | 0.008 | Classic green-purple aurora |
| **Arctic Dawn** | `#ff6b9d, #c44dff, #6ec6ff` | 3 | 80 | 0.005 | Soft pastel ribbons |
| **Deep Space** | `#4a0e8f, #00d4ff, #0a3d62` | 5 | 200 | 0.012 | Dramatic, wide bands |
| **Emerald** | `#00ff87, #00cc6a, #00994d` | 3 | 100 | 0.006 | Monochrome green |
| **Twilight** | `#ff7eb3, #ff758c, #a855f7` | 4 | 150 | 0.010 | Warm sunset aurora |

### 5.3 Particles (Upcoming)

| Preset | Particle Color | Count | Size | Speed | Vibe |
|--------|---------------|-------|------|-------|------|
| **Starfield** | `#ffffff` | 200 | 2 | 0.5 | Classic starfield depth |
| **Fireflies** | `#ffdd57` | 50 | 4 | 0.3 | Warm floating lights |
| **Snowfall** | `#e8f4fd` | 150 | 3 | 0.8 | Gentle winter snow |
| **Cosmic Dust** | `#c4b5fd` | 300 | 1 | 0.2 | Purple nebula dust |
| **Matrix Drip** | `#00ff41` | 100 | 2 | 1.0 | Matrix-themed particles |

### 5.4 Blobs (Upcoming)

| Preset | Colors | Blob Count | Blur | Speed | Vibe |
|--------|--------|-----------|------|-------|------|
| **Lava Lamp** | `#ff6b35, #ff2d87, #9333ea` | 4 | 80 | 0.5 | Warm, organic motion |
| **Ocean** | `#0066ff, #00ccff, #00ffcc` | 5 | 100 | 0.3 | Cool, flowing water |
| **Neon** | `#ff00ff, #00ffff, #ffff00` | 3 | 60 | 0.8 | Electric, vivid |
| **Dawn** | `#ff9a56, #ff6b88, #c471f5` | 4 | 90 | 0.4 | Soft, warm gradients |
| **Deep** | `#1a1a2e, #16213e, #0f3460` | 3 | 120 | 0.2 | Dark, atmospheric |

### 5.5 Noise Field (Upcoming)

| Preset | Color | Frequency | Amplitude | Octaves | Vibe |
|--------|-------|-----------|-----------|---------|------|
| **Static** | `#ffffff` | 0.02 | 1.0 | 1 | Classic TV static |
| **Topographic** | `#00ff87` | 0.005 | 0.8 | 4 | Elevation map feel |
| **Clouds** | `#94a3b8` | 0.008 | 0.6 | 6 | Soft, billowy |
| **Digital** | `#00d4ff` | 0.04 | 1.0 | 2 | Pixelated digital noise |
| **Organic** | `#ff6b35` | 0.003 | 0.9 | 8 | Natural, flowing patterns |

---

## 6. Design Decisions & Rationale

### Why floating panel over separate page?
- **Immediate feedback loop:** Users see the effect change as they adjust controls
- **Showcase-first:** The effect IS the product — it should never be hidden behind a settings page
- **Gallery-ready:** Future effects gallery can embed each demo as an iframe with the panel

### Why collapsible sidebar (not always-visible)?
- **Immersive mode:** For screenshots and presentations, users want a clean full-screen effect
- **Mobile:** On small screens, the panel takes up too much space — collapsing is essential
- **Progressive disclosure:** First-time visitors see the full effect, then discover controls

### Why generate Blazor markup in the code snippet?
- **Copy-paste developer experience:** Developers see exactly what to paste into their `.razor` file
- **Live updating:** As parameters change, the snippet updates — no mental mapping needed
- **Documentation as interaction:** The demo page doubles as interactive API docs

### Why backdrop-blur on all overlays?
- **Depth:** The effect shows through the panel, maintaining immersion
- **Consistency:** Every demo page uses the same glass-morphism language
- **Aesthetic:** Aligns with the "visually stunning" company direction — frosted glass over animated backgrounds is inherently premium

### Why dark glass UI (not light)?
- **All effects render on dark backgrounds** — dark UI overlays blend naturally
- **Light panels over dark effects creates harsh contrast** — breaks immersion
- **The entire product is about dark-background effects** — the demo UI should match

---

## 7. Responsive Behavior

### Desktop (≥768px)
- Control panel: right sidebar, 320px wide
- Header: top-left floating
- Effect: full viewport

### Mobile (<768px)
- Control panel: **bottom sheet** — slides up from bottom, 60% viewport height
- Toggle button: bottom-right floating
- Header: top-left, simplified (smaller text)
- Effect: full viewport

### Transitions
- Panel open/close: `transform 300ms cubic-bezier(0.4, 0, 0.2, 1)`
- Bottom sheet: `transform 400ms cubic-bezier(0.4, 0, 0.2, 1)` with backdrop overlay

---

## 8. FPS Counter Implementation

### JS Interop Approach

```javascript
// wwwroot/js/fps-counter.js
export function startFpsCounter(callbackDotNetRef) {
    let frames = 0;
    let lastTime = performance.now();
    let running = true;
    
    function tick(now) {
        if (!running) return;
        frames++;
        const elapsed = now - lastTime;
        if (elapsed >= 500) {  // Update every 500ms
            const fps = Math.round((frames * 1000) / elapsed);
            callbackDotNetRef.invokeMethodAsync('UpdateFps', fps);
            frames = 0;
            lastTime = now;
        }
        requestAnimationFrame(tick);
    }
    requestAnimationFrame(tick);
    
    return {
        stop() { running = false; }
    };
}
```

### Integration
- Each demo page creates a `DotNetObjectReference` and passes it to the FPS counter
- The `EffectDemoLayout` component manages the lifecycle
- FPS updates via `invokeMethodAsync` — no polling

---

## 9. Implementation Priority

### Phase 1 — MatrixRain Demo Redesign (Now)
1. Create `EffectDemoLayout.razor` shared component
2. Create `fps-counter.js` utility
3. Refactor `MatrixRainDemo.razor` to use the layout
4. Apply all 5 MatrixRain presets
5. Test on desktop + mobile viewports

### Phase 2 — Supporting Components (After MatrixRain)
1. Build `PresetSelector.razor`
2. Build `ParameterGroup.razor` with typed sub-components
3. Build `CodeSnippet.razor` with copy-to-clipboard
4. Create `EffectsGallery.razor` landing page

### Phase 3 — Future Effect Demos (As CTO builds them)
1. Aurora demo page — reuse `EffectDemoLayout`
2. Particles demo page — same template
3. Blobs demo page — same template
4. Noise Field demo page — same template

---

## 10. Current MatrixRain Demo Feedback

### Issues with current implementation (`MatrixRainDemo.razor`):

1. **Controls are inline** — not reusable, no shared component structure
2. **No preset switching** — only raw parameter controls, no one-click presets
3. **No FPS counter** — task requirement not met
4. **No code snippet** — task requirement not met
5. **No panel collapse** — controls always visible, can't see clean effect
6. **Mobile hostile** — no responsive behavior for the control panel
7. **No "Back to Gallery"** — isolated page with no navigation to other effects

### What to keep:
- The parameter bindings (`@bind` pattern) are correct
- The overlay-inside-effect pattern (`ChildContent` with controls) works
- Color picker and slider HTML is functional

### Refactor plan:
Replace the entire page with `EffectDemoLayout` usage — the layout handles all chrome, the page only provides parameter definitions and the effect component.

---

## Summary

This spec creates a **reusable, production-quality demo page system** that:
- Gives every effect component a beautiful, consistent showcase
- Provides real-time parameter tuning with immediate visual feedback
- Generates copy-paste Blazor markup for developers
- Scales to all 5 planned effects with zero layout duplication
- Delivers an immersive, premium experience aligned with our "visually stunning" company direction
