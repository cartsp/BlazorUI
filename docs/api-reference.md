# API Reference

## Core Abstractions

### EffectComponentBase

The abstract base class all effect components inherit from. Handles canvas lifecycle, JavaScript module loading, config-hash diffing, and disposal.

**Namespace:** `BlazorEffects.Core.Animation`

**Inherits:** `ComponentBase`, implements `IAsyncDisposable`

#### Common Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `TargetFps` | `int` | `60` | Target frames per second (clamped 1–120). Lower values reduce CPU/GPU usage. |
| `Height` | `string` | `"100vh"` | CSS height of the effect container. Accepts any valid CSS value. |
| `CssClass` | `string?` | `null` | Optional CSS class applied to the content overlay `<div>`. |
| `ChildContent` | `RenderFragment?` | `null` | Child content rendered over the animated canvas. |

#### Protected Members (for custom effects)

| Member | Type | Description |
|--------|------|-------------|
| `ModulePath` | `string` | **Abstract.** Path to the effect's JS module (e.g., `"./_content/BlazorEffects.MatrixRain/matrix-rain.js"`). |
| `BuildConfig()` | `object` | **Abstract.** Builds the config object to pass to the JS module's `init` and `update` functions. |
| `ComputeConfigHash()` | `string` | **Abstract.** Returns a hash string representing current parameter values. Used for change detection. |
| `CanvasElement` | `ElementReference` | Reference to the `<canvas>` element. |
| `Module` | `IJSObjectReference?` | Reference to the loaded JS module. |

### IEffectConfig

Marker interface for effect configuration objects passed to JS modules.

**Namespace:** `BlazorEffects.Core.Animation`

```csharp
public interface IEffectConfig;
```

### IEffectDescriptor\<TConfig\>

Provides parameter definitions and presets for playground integration. Implement this on each effect's config type.

**Namespace:** `BlazorEffects.Core.Animation`

| Member | Type | Description |
|--------|------|-------------|
| `GetParameterDefinitions()` | `IReadOnlyList<EffectParameterDefinition>` | All parameter definitions (drives playground controls). |
| `GetPresets()` | `IReadOnlyList<EffectPreset<TConfig>>` | All available presets. |
| `ComponentType` | `Type` | The Blazor component type that renders this effect. |
| `EffectName` | `string` | Display name for the playground UI. |
| `ApplyParameter(config, name, value)` | `TConfig` | Apply a single parameter value, returning a new config. |

### EffectParameterDefinition

Describes a single effect parameter for the playground UI.

| Property | Type | Description |
|----------|------|-------------|
| `Name` | `string` | Display name. |
| `PropertyName` | `string` | Config property name (for code generation). |
| `Type` | `EffectParameterType` | Control type: `Range`, `Integer`, `Toggle`, `Color`, `ColorArray`, `Text`, `Select`. |
| `DefaultValue` | `object` | Default value. |
| `MinValue` | `double?` | Minimum (for `Range` type). |
| `MaxValue` | `double?` | Maximum (for `Range` type). |
| `Step` | `double?` | Step increment (for `Range` type). |
| `Description` | `string?` | Tooltip text. |
| `Order` | `int` | Display order (lower = higher in UI). |

### EffectPreset\<TConfig\>

Represents a named preset — curated parameter values.

| Property | Type | Description |
|----------|------|-------------|
| `Name` | `string` | Preset display name (e.g., "Cyberpunk"). |
| `Description` | `string` | Visual character description. |
| `Config` | `TConfig` | Config with all preset values applied. |
| `PreviewGradient` | `string?` | Optional CSS gradient for the preset gallery thumbnail. |

### EffectDefaults

Constants for default parameter values.

| Constant | Value | Description |
|----------|-------|-------------|
| `MinFps` | `1` | Minimum target FPS. |
| `MaxFps` | `120` | Maximum target FPS. |
| `DefaultFps` | `60` | Default target FPS. |
| `DefaultHeight` | `"100vh"` | Default container height. |

### IJsEffectBridge

Interface for JS effect bridge modules.

**Namespace:** `BlazorEffects.Core.Interop`

| Method | Returns | Description |
|--------|---------|-------------|
| `InitAsync(canvas, config)` | `Task<string>` | Initialize effect on canvas, return unique instance ID. |
| `UpdateAsync(instanceId, config)` | `Task` | Update existing instance with new config. |
| `DisposeInstanceAsync(instanceId)` | `Task` | Stop animation and cleanup resources. |

---

## Effect Reference

### 🌧 Matrix Rain

**Package:** `BlazorEffects.MatrixRain` · **Component:** `<MatrixRain>` · **Namespace:** `BlazorEffects.MatrixRain`

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Characters` | `string` | `MatrixRainPresets.Classic` | Character pool for rain streams. |
| `FontSize` | `double` | `16` | Font size in px (also determines column width). |
| `FontFamily` | `string` | `"monospace"` | Canvas font family. |
| `Color` | `string` | `"#00ff41"` | Lead character color. |
| `FadeColor` | `string` | `"#003b00"` | Background fade color for the trailing effect. |
| `Speed` | `double` | `1.0` | Fall speed multiplier. |
| `Density` | `double` | `1.0` | Active column fraction (0.0–1.0). |
| `Opacity` | `double` | `0.8` | Overall effect opacity. |

**Presets** (`MatrixRainPresets`):

| Preset | Description |
|--------|-------------|
| `Classic` | A-Z, 0-9, and symbols |
| `Katakana` | Japanese Katakana characters |
| `Binary` | 0s and 1s |
| `Hex` | Hexadecimal characters |
| `Emojis` | Rocket, sparkles, gems, etc. |

---

### 🌌 Particle Constellation

**Package:** `BlazorEffects.Particles` · **Component:** `<ParticleConstellation>` · **Namespace:** `BlazorEffects.Particles`

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Config` | `ParticleConstellationConfig` | `Default` | Full config object (overrides individual params). |
| `ParticleCount` | `int` | `150` | Number of particles. |
| `ParticleColor` | `string` | `"#6366f1"` | Particle fill color. |
| `ParticleSize` | `double` | `2` | Particle radius in px. |
| `ConnectionDistance` | `double` | `120` | Max distance for drawing connection lines. |
| `ConnectionColor` | `string` | `"#6366f1"` | Connection line color. |
| `Speed` | `double` | `0.5` | Movement speed multiplier. |
| `MouseInteraction` | `bool` | `true` | Enable mouse-based interaction. |
| `MouseRadius` | `double` | `150` | Mouse influence radius in px. |
| `MouseForce` | `string` | `"attract"` | Mouse force direction: `"attract"` or `"repel"`. |
| `Opacity` | `double` | `0.6` | Overall effect opacity. |

**Presets** (`ParticleConstellationPresets`):

| Preset | Description |
|--------|-------------|
| `Default` | Indigo network — tech/SaaS aesthetic |
| `Cyberpunk` | Neon green on dark |
| `DeepSpace` | Cool blue constellation |
| `Amber` | Warm amber particles |
| `Minimal` | Sparse, subtle background |
| `Dense` | Dense mesh connectivity |

---

### 🌈 Aurora Borealis

**Package:** `BlazorEffects.Aurora` · **Component:** `<AuroraBorealis>` · **Namespace:** `BlazorEffects.Aurora`

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Colors` | `string[]` | `Classic` | Color palette for the aurora ribbons. |
| `RibbonCount` | `int` | `4` | Number of aurora ribbons. |
| `Amplitude` | `double` | `120` | Vertical displacement of ribbons. |
| `Speed` | `double` | `0.008` | Animation speed. |
| `Opacity` | `double` | `0.5` | Overall effect opacity. |
| `BlendMode` | `string` | `"screen"` | CSS blend mode for the ribbons. |

**Presets** (`AuroraBorealisPresets`):

| Preset | Description |
|--------|-------------|
| `Classic` | Green-purple with teal accents |
| `Arctic` | Cool blue and cyan tones |
| `Sunset` | Warm magenta and rose |
| `Emerald` | Deep greens |
| `Cosmic` | Purple and violet |

---

### 🫧 Morphing Blobs

**Package:** `BlazorEffects.Blobs` · **Component:** `<MorphingBlobs>` · **Namespace:** `BlazorEffects.Blobs`

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `BlobCount` | `int` | `4` | Number of organic blobs. |
| `Colors` | `string[]` | `Default` | Color palette. |
| `BlobSize` | `double` | `300` | Base blob diameter in px. |
| `Speed` | `double` | `0.005` | Morphing speed. |
| `MorphIntensity` | `double` | `80` | Shape deformation amount in px. |
| `BlendMode` | `string` | `"screen"` | CSS blend mode between blobs. |
| `Opacity` | `double` | `0.7` | Overall effect opacity. |

**Presets** (`MorphingBlobsPresets`):

| Preset | Description |
|--------|-------------|
| `Default` | Indigo, pink, orange, cyan |
| `Sunset` | Deep purple, rose, amber, orange |
| `Ocean` | Deep blue, teal, cyan, sky |
| `Aurora` | Green, teal, purple, blue |
| `Neon` | Hot pink, electric blue, lime, violet |
| `Pastel` | Soft pink, lavender, peach, mint |
| `Monochrome` | Shades of gray/slate |

---

### 🌊 Noise Field

**Package:** `BlazorEffects.Noise` · **Component:** `<NoiseField>` · **Namespace:** `BlazorEffects.Noise`

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `ColorStops` | `string[]` | `Default` | Gradient color stops for the noise texture. |
| `NoiseScale` | `double` | `0.003` | Zoom level of the noise field. |
| `Speed` | `double` | `0.005` | Flow animation speed. |
| `Octaves` | `int` | `4` | Number of noise octaves (detail layers). |
| `Persistence` | `double` | `0.5` | Amplitude multiplier per octave. |
| `Lacunarity` | `double` | `2.0` | Frequency multiplier per octave. |
| `Brightness` | `double` | `1.0` | Overall brightness multiplier. |
| `Opacity` | `double` | `0.85` | Overall effect opacity. |

**Presets** (`NoiseFieldPresets`):

| Preset | Description |
|--------|-------------|
| `Default` | Deep indigo to violet to pink to rose |
| `Aurora` | Deep space to emerald to teal to violet |
| `Sunset` | Deep plum to coral to amber to violet |
| `Ocean` | Deep abyss to navy to teal to cyan |
| `Lava` | Deep black to crimson to orange to gold |
| `Monochrome` | Black to white grayscale |
| `Neon` | Dark to electric pink to cyan to violet |

---

### 🧿 Gradient Waves

**Package:** `BlazorEffects.GradientWaves` · **Component:** `<GradientWaves>` · **Namespace:** `BlazorEffects.GradientWaves`

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Colors` | `string[]` | `Stripe` | Color palette for the gradient mesh. |
| `PointCount` | `int` | `6` | Number of color points in the mesh. |
| `BlobSize` | `double` | `0.5` | Size of each color blob (fraction of canvas). |
| `Speed` | `double` | `0.004` | Drift animation speed. |
| `BlurAmount` | `double` | `80` | Gaussian blur amount in px (meshes the colors). |
| `BlendMode` | `string` | `"normal"` | CSS blend mode. |
| `Opacity` | `double` | `1.0` | Overall effect opacity. |

**Presets** (`GradientWavesPresets`):

| Preset | Description |
|--------|-------------|
| `Stripe` | Stripe-inspired: purple, blue, pink, cyan |
| `VibrantSunset` | Orange, magenta, gold, coral |
| `OceanDeep` | Navy, teal, aquamarine, sky |
| `NorthernLights` | Emerald, violet, indigo, teal, cyan, fuchsia |
| `MinimalMono` | Warm grays and off-whites |
| `Cyberpunk` | Neon green, magenta, electric blue, yellow |
| `RoseGarden` | Soft pinks, rose, blush, mauve |
| `Forest` | Deep greens, emerald, moss, sage |

---

### 🔥 Fire / Embers

**Package:** `BlazorEffects.FireEmbers` · **Component:** `<FireEmbers>` · **Namespace:** `BlazorEffects.FireEmbers`

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `ParticleCount` | `int` | `200` | Number of flame/ember particles. |
| `FlameColor` | `string` | `"#ff6600"` | Base flame color (blends to transparent at top). |
| `EmberColor` | `string` | `"#ffcc00"` | Ember spark color. |
| `Speed` | `double` | `1.5` | Particle rise speed multiplier. |
| `ParticleSize` | `double` | `4.0` | Maximum particle size in px. |
| `Turbulence` | `double` | `1.0` | Horizontal wobble intensity. |
| `EmberRatio` | `double` | `0.3` | Fraction of particles that are embers vs flames (0.0–1.0). |
| `BackgroundColor` | `string` | `"#0a0a0a"` | Background color. |
| `Opacity` | `double` | `0.9` | Overall effect opacity. |

**Presets** (`FireEmbersPresets`):

| Preset | Description |
|--------|-------------|
| `Default` | Warm orange flames with golden embers |
| `Bonfire` | Large, intense, many particles |
| `Candlelight` | Small, gentle, warm glow |
| `Inferno` | Aggressive, fast, lots of sparks |
| `BlueFlame` | Magical / gas burner aesthetic |
| `EmbersOnly` | Sparse sparks rising from coals |

---

### ⭐ Starfield

**Package:** `BlazorEffects.Starfield` · **Component:** `<Starfield>` · **Namespace:** `BlazorEffects.Starfield`

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `StarCount` | `int` | `800` | Number of stars. |
| `StarColor` | `string` | `"#ffffff"` | Star fill color. |
| `StarSize` | `double` | `2.0` | Maximum star radius in px. |
| `Speed` | `double` | `2.0` | Warp speed multiplier (how fast stars approach). |
| `TrailLength` | `double` | `0.6` | Speed trail length (0 = no trail, 1 = full trail). |
| `Depth` | `double` | `1000` | Star field depth (how far stars spawn). |
| `BackgroundColor` | `string` | `"#000000"` | Background color. |
| `Opacity` | `double` | `1.0` | Overall effect opacity. |

**Presets** (`StarfieldPresets`):

| Preset | Description |
|--------|-------------|
| `Default` | White stars on black — classic hyperspace warp |
| `WarpDrive` | Blue-tinted stars — deep space FTL feel |
| `GoldenDrift` | Warm gold stars — gentle ambient drift |
| `Sparse` | Bright stars — minimalist cosmos |
| `Blizzard` | Dense storm of stars |
| `RetroTerminal` | Neon green — retro terminal aesthetic |

---

### 💧 Ripple

**Package:** `BlazorEffects.Ripple` · **Component:** `<Ripple>` · **Namespace:** `BlazorEffects.Ripple`

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `MaxRipples` | `int` | `20` | Maximum number of active ripples. |
| `MaxRadius` | `double` | `300` | Maximum radius before a ripple is removed (px). |
| `Speed` | `double` | `3.0` | Expansion speed (px per frame). |
| `Color` | `string` | `"#60a5fa"` | Ripple stroke color. |
| `LineWidth` | `double` | `2.0` | Stroke width of each ring (px). |
| `Decay` | `double` | `0.02` | Opacity decay rate per frame (0.0–1.0). Higher = faster fade. |
| `Trigger` | `string` | `"auto"` | Trigger mode: `"auto"` (automatic) or `"click"` (click/touch). |
| `AutoRippleCount` | `int` | `5` | Number of auto-generated ripples (auto mode). |
| `AutoInterval` | `int` | `800` | Interval between auto-generated ripples (ms). |
| `BackgroundColor` | `string` | `"#0f172a"` | Background color. |
| `Opacity` | `double` | `1.0` | Overall effect opacity. |

**Presets** (`RipplePresets`):

| Preset | Description |
|--------|-------------|
| `Default` | Blue ripples on dark — calming water |
| `NeonElectric` | Neon cyan — electric sci-fi |
| `Sunset` | Warm coral tones |
| `Minimal` | Clean subtle white ripples |
| `RainDrops` | Green tones simulating rain on water |
| `Interactive` | Click-to-ripple with purple ripples |

---

### 🌀 Vortex Tunnel

**Package:** `BlazorEffects.VortexTunnel` · **Component:** `<VortexTunnel>` · **Namespace:** `BlazorEffects.VortexTunnel`

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `RingCount` | `int` | `20` | Number of concentric rings. |
| `RotationSpeed` | `double` | `0.02` | Rotation speed (radians per frame). |
| `Color` | `string` | `"#8b5cf6"` | Primary ring color (used when `Colors` is empty). |
| `Colors` | `string[]` | `[]` | Multi-colored rings. Overrides `Color` when non-empty. |
| `ScaleFactor` | `double` | `0.92` | Scale between successive rings (how much each shrinks). |
| `Shape` | `string` | `"circle"` | Ring shape: `"circle"`, `"polygon"`, or `"square"`. |
| `PolygonSides` | `int` | `6` | Number of sides when `Shape` is `"polygon"`. |
| `LineWidth` | `double` | `2.0` | Base stroke width for the outermost ring. Inner rings get thinner. |
| `BackgroundColor` | `string` | `"#030712"` | Background color. |
| `Opacity` | `double` | `1.0` | Overall effect opacity. |

**Presets** (`VortexTunnelPresets`):

| Preset | Description |
|--------|-------------|
| `Default` | Purple vortex — hypnotic spiral |
| `HypnoWheel` | Multi-colored psychedelic hexagonal rings |
| `DeepSpace` | Dark blue tunnel with subtle rotation |
| `CyberGrid` | Green square shapes — digital tunnel |
| `WarmPortal` | Orange/red spinning vortex |
| `IceCrystal` | Light blue hexagonal vortex |

---

## JS Interop Lifecycle

For advanced users who want to build custom effects or understand the internals.

### Module Contract

Every effect's JavaScript module implements three functions:

```javascript
// Initialize the effect on a canvas element with the given config.
// Returns a unique string instance ID.
export function init(canvasElement, config) { ... }

// Update an existing instance with new config (e.g., when Blazor parameters change).
export function update(instanceId, config) { ... }

// Dispose an instance — stop animation, free resources.
export function dispose(instanceId) { ... }
```

### Lifecycle Flow

```
1. Component renders → OnAfterRenderAsync(firstRender=true)
   └─ JS module loaded via import(ModulePath)
   └─ init(canvasElement, config) → instanceId

2. Blazor parameters change → OnParametersSetAsync()
   └─ ComputeConfigHash() compared to previous hash
   └─ If different → update(instanceId, newConfig)

3. Component disposes → DisposeAsync()
   └─ dispose(instanceId)
   └─ JS module reference disposed
```

### Config-Hash Diffing

BlazorEffects only re-sends config to JavaScript when parameters actually change. This is done via `ComputeConfigHash()`, which returns a string concatenation of all parameter values. If the hash matches the previous one, the update is skipped — avoiding unnecessary JS interop calls and animation restarts.

### Static Asset Convention

JS modules are served from the Razor Class Library's `wwwroot/` using Blazor's static asset convention:

```
./_content/{PackageId}/{filename}.js
```

For example:
- `./_content/BlazorEffects.MatrixRain/matrix-rain.js`
- `./_content/BlazorEffects.Particles/particles.js`
- `./_content/BlazorEffects.Aurora/aurora.js`

## Creating a Custom Effect

To create a new effect that integrates with the BlazorEffects ecosystem:

1. **Create a Razor Class Library** that references `BlazorEffects.Core`
2. **Create a config record** implementing `IEffectConfig`
3. **Create the Razor component** inheriting `EffectComponentBase`
4. **Implement the JS module** with `init`, `update`, and `dispose` exports
5. **Add presets** as static properties on a `*Presets` class
6. **Optionally implement `IEffectDescriptor<TConfig>`** for playground integration

See [CONTRIBUTING.md](../CONTRIBUTING.md) for detailed guidelines.
