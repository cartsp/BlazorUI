# Playground UI — Design Specification

> **Author:** UXDesigner  
> **For:** CTO (implementation), QAEngineer (testing)  
> **Task:** [AIE-25](/AIE/issues/AIE-25) — Build component presets system and parameter playground UI

---

## 1. Design Direction

**Aesthetic:** *Studio Console* — Think of a professional creative tool (Figma's property panel, TouchDesigner's parameter view, or a high-end synthesizer). Dark by default, dense but organized, with accent color highlights on interactive controls. The playground should feel like a **creative instrument**, not a generic admin form.

**Differentiation:** The live preview is the star. Controls are a compact sidebar. Presets are visual gradient thumbnails, not a dropdown. The whole page is a real-time canvas-first experience with a glassmorphism control panel floating over it.

**Tone:** Technical but inviting. Utility-focused with polish. Precision instrument, not marketing page.

---

## 2. Layout Architecture

```
┌──────────────────────────────────────────────────────────┐
│  Header Bar (effect name, nav, theme toggle)    [48px]   │
├──────────┬───────────────────────────────────────────────┤
│          │                                               │
│  Control │         Live Preview Canvas                   │
│  Panel   │         (fills remaining space)               │
│  [340px] │                                               │
│          │                                               │
│  ┌──────┐│                                               │
│  │Preset││                                               │
│  │Grid  ││                                               │
│  └──────┘│                                               │
│  ┌──────┐│                                               │
│  │Param ││                                               │
│  │Editor││                                               │
│  └──────┘│                                               │
│  ┌──────┐│                                               │
│  │Code  ││                                               │
│  │Export││                                               │
│  └──────┘│                                               │
│          │                                               │
├──────────┴───────────────────────────────────────────────┤
│  Footer (copy status toast appears here)                  │
└──────────────────────────────────────────────────────────┘
```

### Responsive Behavior

- **≥1024px**: Side-by-side layout (panel + preview)
- **768–1023px**: Preview on top (60vh), scrollable panel below
- **<768px**: Full-screen preview with slide-out panel (toggle button)

---

## 3. Component Hierarchy

```
EffectPlayground.razor          ← Main page, owns layout + state
├── EffectSelector.razor        ← Top nav: switch between effects (5 items)
├── LivePreview.razor           ← DynamicComponent rendering the active effect
├── ControlPanel.razor          ← Left sidebar container (scrollable)
│   ├── PresetGallery.razor     ← Visual preset thumbnails
│   ├── ParameterEditor.razor   ← Dynamic parameter controls
│   │   ├── RangeSlider.razor   ← EffectParameterType.Range
│   │   ├── IntegerInput.razor  ← EffectParameterType.Integer
│   │   ├── ToggleSwitch.razor  ← EffectParameterType.Toggle
│   │   ├── ColorPicker.razor   ← EffectParameterType.Color
│   │   ├── ColorArrayEditor.razor ← EffectParameterType.ColorArray
│   │   ├── TextInput.razor     ← EffectParameterType.Text
│   │   └── SelectDropdown.razor ← EffectParameterType.Select
│   └── CodeExporter.razor      ← Generated code snippet + copy button
```

---

## 4. Component Designs

### 4.1 EffectPlayground.razor (Main Page)

**Layout:** CSS Grid — `grid-template-columns: 340px 1fr` on desktop, stacked on mobile.

**State management:**
- `CurrentDescriptor` — the active `IEffectDescriptor<TConfig>`
- `CurrentConfig` — the current config state (mutable, updated by parameter changes)
- `ActivePresetName` — tracks which preset is highlighted (null = custom)

**Dark mode:** The playground is *always dark* regardless of theme toggle. The control panel uses `--color-surface` from dark theme. The live preview renders on a black canvas background.

```razor
@* Structure sketch *@
<div class="playground-grid h-[calc(100vh-48px)]">
    <aside class="control-panel overflow-y-auto border-r border-white/10 bg-[#0f172a]/95 backdrop-blur-sm">
        <PresetGallery Presets="presets" ActivePreset="activePreset" OnSelect="ApplyPreset" />
        <ParameterEditor Definitions="definitions" Config="config" OnChange="HandleParameterChange" />
        <CodeExporter Config="config" ComponentType="descriptor.ComponentType" />
    </aside>
    <main class="relative bg-black">
        <LivePreview ComponentType="descriptor.ComponentType" Config="currentConfig" />
    </main>
</div>
```

### 4.2 PresetGallery.razor

**Visual:** Horizontal scrolling row of **gradient thumbnail cards**, each showing the preset's `PreviewGradient`. Selected preset has an accent ring.

```
┌────────┐ ┌────────┐ ┌────────┐ ┌────────┐ ┌────────┐
│ ████▓▓ │ │ ████▓▓ │ │ ████▓▓ │ │ ████▓▓ │ │ ████▓▓ │
│ ████▓▓ │ │ ████▓▓ │ │ ████▓▓ │ │ ████▓▓ │ │ ████▓▓ │
├────────┤ ├────────┤ ├────────┤ ├────────┤ ├────────┤
│ Classic│ │ Aurora │ │ Sunset │ │  Ocean │ │   Lava │
└────────┘ └────────┘ └────────┘ └────────┘ └────────┘
  ▲ selected — ring-2 ring-accent-500
```

**Tailwind classes for each preset card:**
```
w-[60px] h-[40px] rounded-lg cursor-pointer transition-all duration-150
hover:scale-105 hover:ring-1 hover:ring-white/30
ring-2 ring-accent-500 ring-offset-2 ring-offset-[#0f172a]   ← selected
ring-1 ring-white/10                                              ← unselected
```

The gradient is applied via `style="background: @preset.PreviewGradient"`.

**Section header:** "Presets" in `text-xs font-medium uppercase tracking-wider text-white/40`.

### 4.3 ParameterEditor.razor

Renders a dynamic list of parameter controls based on `EffectParameterDefinition.Type`. Each parameter row follows a consistent pattern:

```
┌─────────────────────────────────────┐
│ Parameter Name            value     │
│ [═══════════●═══════════]           │
│ Tooltip description (subtle)        │
└─────────────────────────────────────┘
```

**Section header:** "Parameters" in same style as Presets header.

**Parameter row wrapper:**
```
py-3 px-4 border-b border-white/5
```

**Label + value row:**
```
flex justify-between items-center mb-1.5
```

- Label: `text-sm font-medium text-white/80`
- Value: `text-sm font-mono text-accent-400 tabular-nums`

**Description tooltip:** `text-xs text-white/30 mt-0.5`

### 4.4 RangeSlider.razor (EffectParameterType.Range)

Custom-styled range input. Native `<input type="range">` with CSS custom styling.

**Key design decisions:**
- Track: `h-1.5 rounded-full bg-white/10`
- Filled portion: `bg-accent-500` (achieved via CSS gradient trick or JS)
- Thumb: `w-4 h-4 rounded-full bg-white shadow-md shadow-black/50 border-2 border-accent-500`
- Value display: Shows current value formatted to appropriate decimal places

**CSS for the range input:**
```css
/* Tailwind v4 — playground slider styling */
.playground-slider {
    @apply w-full h-1.5 rounded-full appearance-none cursor-pointer;
    background: linear-gradient(to right, var(--color-accent-500) 0%, var(--color-accent-500) var(--fill-pct, 50%), rgb(255 255 255 / 0.1) var(--fill-pct, 50%), rgb(255 255 255 / 0.1) 100%);
}

.playground-slider::-webkit-slider-thumb {
    @apply w-4 h-4 rounded-full bg-white appearance-none cursor-pointer;
    border: 2px solid var(--color-accent-500);
    box-shadow: 0 2px 6px rgb(0 0 0 / 0.5);
}

.playground-slider::-moz-range-thumb {
    @apply w-4 h-4 rounded-full bg-white appearance-none cursor-pointer border-0;
    border: 2px solid var(--color-accent-500);
    box-shadow: 0 2px 6px rgb(0 0 0 / 0.5);
}
```

The `--fill-pct` is calculated in Blazor: `(value - min) / (max - min) * 100`.

### 4.5 IntegerInput.razor (EffectParameterType.Integer)

Same slider as RangeSlider but with **step=1** and integer formatting. Value display shows no decimal places.

Alternatively, for small integer ranges (< 20 options), render a **button group**:

```
[1] [2] [3] [4] [5] [6] [7] [8]
 ▲ active — bg-accent-500 text-white
   inactive — bg-white/5 text-white/60 hover:bg-white/10
```

### 4.6 ToggleSwitch.razor (EffectParameterType.Toggle)

Custom toggle switch, not a checkbox.

```
    ON  ╭─────────╮
       │  ⬤      │
       ╰─────────╯

   OFF  ╭─────────╮
       │      ⬤  │
       ╰─────────╯
```

**Tailwind:**
```
/* Track */
w-10 h-5 rounded-full transition-colors duration-150
bg-accent-500    ← on
bg-white/10      ← off

/* Thumb */
w-4 h-4 rounded-full bg-white shadow-sm
translate-x-5    ← on
translate-x-0.5  ← off
transition-transform duration-150
```

Label goes *before* the switch, value text ("On"/"Off") is implicit in the visual state.

### 4.7 ColorPicker.razor (EffectParameterType.Color)

Native color input with a styled wrapper:

```
┌──────┐ #6366f1
│ ████ │  (hex display in mono font)
└──────┘
```

**Styling the color swatch:**
```
w-8 h-8 rounded-md border border-white/20 cursor-pointer
overflow-hidden
```

The `<input type="color">` is positioned absolutely inside the swatch div with `opacity-0 w-full h-full cursor-pointer` to make the entire swatch clickable.

### 4.8 ColorArrayEditor.razor (EffectParameterType.ColorArray)

Shows gradient preview + individual color stops:

```
┌─────────────────────────────────────┐
│ ███████████████████████████████████ │  ← gradient preview bar
└─────────────────────────────────────┘
 ● #0f172a  ● #6366f1  ● #a855f7  ●  + Add stop
```

Each stop is a small color swatch (20x20) with hex label underneath. Clicking a swatch opens its native color picker. The `+ Add stop` button appends a new color.

**Gradient preview:** `h-3 rounded-full` with `background: linear-gradient(90deg, ...)` built from the color array.

### 4.9 TextInput.razor (EffectParameterType.Text)

Styled text input:

```
bg-white/5 border border-white/10 rounded-md px-3 py-1.5
text-sm text-white/90 font-mono
placeholder:text-white/20
focus:border-accent-500 focus:ring-1 focus:ring-accent-500/30
outline-none transition-colors duration-150
```

### 4.10 SelectDropdown.razor (EffectParameterType.Select)

Styled select element:

```
bg-white/5 border border-white/10 rounded-md px-3 py-1.5
text-sm text-white/90
appearance-none cursor-pointer
focus:border-accent-500 focus:ring-1 focus:ring-accent-500/30
```

With a custom chevron-down SVG icon positioned `right-3 top-1/2 -translate-y-1/2 pointer-events-none text-white/40`.

### 4.11 CodeExporter.razor

**Layout:**

```
┌───────────────────────────────────┐
│ Generated Code            [Copy]  │
├───────────────────────────────────┤
│ <NoiseField                       │
│     ColorStops="@(["#0f172a",..." │
│     NoiseScale="0.005"            │
│     Speed="0.003"                 │
│     Octaves="3" />                │
└───────────────────────────────────┘
```

**Code block:**
```
bg-black/40 rounded-lg p-4 font-mono text-xs text-green-400
overflow-x-auto whitespace-pre border border-white/5
```

**Copy button:**
```
px-3 py-1 text-xs font-medium rounded-md
bg-white/5 text-white/60 hover:bg-white/10 hover:text-white/80
transition-colors duration-150
```

On click, copy to clipboard and flash the button:
- `bg-accent-500/20 text-accent-400` for 1.5 seconds
- Button text changes from "Copy" → "Copied!"

### 4.12 LivePreview.razor

Uses Blazor's `DynamicComponent` to render the active effect:

```razor
<DynamicComponent Type="ComponentType" Parameters="componentParameters" />
```

The preview area fills the remaining space. The component renders in a `relative w-full h-full bg-black` container. No extra chrome — the effect IS the background.

A subtle FPS counter can appear in the top-right corner:
```
absolute top-3 right-3 text-xs font-mono text-white/20
```

### 4.13 EffectSelector.razor (Header)

Horizontal tab bar in the header:

```
┌──────────────────────────────────────────────────────────┐
│  BlazorEffects Playground    Noise | Blobs | Aurora | …  │
└──────────────────────────────────────────────────────────┘
```

- Brand: `text-sm font-semibold text-white/60 tracking-wide`
- Tab: `px-3 py-1 text-sm rounded-md transition-colors duration-150`
  - Active: `bg-white/10 text-white`
  - Inactive: `text-white/40 hover:text-white/60 hover:bg-white/5`

---

## 5. Interaction Patterns

### Live Updates
All parameter changes apply **immediately** to the preview. No "Apply" button. The `ParameterEditor` calls `OnParameterChange` on every `oninput` event (not `onchange`), so sliders update in real-time as the user drags.

### Preset → Custom Transition
When a user selects a preset, all parameters snap to preset values. If the user then adjusts any parameter, the `ActivePresetName` clears (becomes null) and the preset gallery shows no selection — indicating the user is now in "custom" mode.

### Reset
A small "Reset" icon button (↺) next to the "Parameters" header resets all values to the descriptor's defaults.

### Code Export
The code export section auto-updates as parameters change. The generated code is a valid Blazor component tag matching the current config state:

```razor
<NoiseField
    ColorStops="@(new[] { "#0f172a", "#6366f1", "#a855f7", "#ec4899", "#0f172a" })"
    NoiseScale="0.005"
    Speed="0.003"
    Octaves="3"
    Persistence="0.5"
    Lacunarity="2.0"
    Brightness="1.0"
    Opacity="0.8" />
```

### Copy Feedback
When the copy button is clicked:
1. Button flashes accent color
2. Text changes to "Copied!"
3. A toast notification slides in from the bottom: `fixed bottom-4 left-1/2 -translate-x-1/2 bg-accent-500 text-white text-sm px-4 py-2 rounded-lg shadow-lg`
4. Toast auto-dismisses after 2 seconds

---

## 6. CSS Custom Properties for Playground

Add these to `app.css` within the `@theme` block:

```css
/* Playground-specific tokens */
--color-playground-bg: #0f172a;
--color-playground-surface: #1e293b;
--color-playground-border: rgb(255 255 255 / 0.1);
--color-playground-text: rgb(255 255 255 / 0.8);
--color-playground-text-muted: rgb(255 255 255 / 0.4);
--color-playground-accent: var(--color-accent-500);
```

---

## 7. Accessibility

All controls must be WCAG 2.1 AA:
- Range sliders: `aria-label="{Name}"`, `aria-valuemin`, `aria-valuemax`, `aria-valuenow`
- Toggle switches: `role="switch"`, `aria-checked`
- Color pickers: `aria-label="{Name} color"`
- Preset cards: `aria-label="{preset.Name} preset"`, `aria-pressed` for selected state
- Code export: Copy button has `aria-label="Copy generated code"`
- All interactive elements are keyboard navigable (Tab order follows visual order)
- Focus rings: `focus-visible:ring-2 focus-visible:ring-accent-500 focus-visible:ring-offset-2 focus-visible:ring-offset-[#0f172a]`

---

## 8. Animation Details

| Element | Animation | CSS |
|---------|-----------|-----|
| Preset card hover | Subtle scale | `transition-transform duration-150 hover:scale-105` |
| Parameter value change | Brief flash | `transition-colors duration-150` |
| Toggle switch | Slide | `transition-transform duration-150` |
| Copy flash | Accent pulse | `transition-colors duration-300` |
| Toast notification | Slide up + fade | `animate-slide-up-fade` (custom keyframe) |
| Panel collapse (mobile) | Slide in from left | `transition-transform duration-300 ease-out` |

**Custom keyframe:**
```css
@keyframes slide-up-fade {
    from { opacity: 0; transform: translateX(-50%) translateY(1rem); }
    to { opacity: 1; transform: translateX(-50%) translateY(0); }
}
```

---

## 9. File Structure (Implementation)

```
src/BlazorEffects.Playground/
├── BlazorEffects.Playground.csproj
├── wwwroot/
│   └── playground.css           ← Playground-specific styles
├── _Imports.razor
├── EffectPlayground.razor        ← Main page
├── EffectPlayground.razor.cs     ← Code-behind (state management)
├── EffectSelector.razor          ← Header tab bar
├── LivePreview.razor             ← DynamicComponent wrapper
├── ControlPanel.razor            ← Sidebar container
├── PresetGallery.razor           ← Preset thumbnail grid
├── ParameterEditor.razor         ← Dynamic parameter renderer
├── Controls/
│   ├── RangeSlider.razor
│   ├── IntegerInput.razor
│   ├── ToggleSwitch.razor
│   ├── ColorPicker.razor
│   ├── ColorArrayEditor.razor
│   ├── TextInput.razor
│   └── SelectDropdown.razor
└── CodeExporter.razor
```

---

## 10. Design Rationale

**Why sidebar, not tabs/modals?**  
The entire value of the playground is seeing changes in real-time. A sidebar keeps controls visible alongside the preview at all times. No context switching.

**Why dark-only playground?**  
The effects render on black canvas backgrounds. A dark control panel creates visual continuity — the UI disappears and the effect takes center stage. A light panel would be jarring against the preview.

**Why gradient thumbnails, not dropdown?**  
Presets are visual by nature. A dropdown forces users to read names and guess. Gradient thumbnails let users *see* the palette before clicking, reducing exploration friction.

**Why `oninput` not `onchange`?**  
`onchange` fires on release. `oninput` fires continuously during drag. For real-time preview, continuous is essential — the user sees the effect evolve as they drag the slider.

**Why CSS-only animations?**  
JS interop calls in Blazor are relatively expensive. CSS transitions are GPU-accelerated and don't trigger re-renders. For a tool that's already running a canvas animation loop, minimizing JS overhead matters.

---

*End of Playground UI Design Spec*
