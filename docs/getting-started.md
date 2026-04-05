# Getting Started with BlazorEffects

## Prerequisites

- **.NET SDK**: 8.0, 9.0, or 10.0
- **Project type**: Blazor Server or Blazor WebAssembly

## Installation

BlazorEffects uses a modular package system — install only the effects you need. Every effect package depends on `BlazorEffects.Core`, which is pulled in automatically.

### Install an Effect Package

```bash
# Matrix Rain
dotnet add package BlazorEffects.MatrixRain

# Particle Constellation
dotnet add package BlazorEffects.Particles

# Aurora Borealis
dotnet add package BlazorEffects.Aurora

# Morphing Blobs
dotnet add package BlazorEffects.Blobs

# Noise Field
dotnet add package BlazorEffects.Noise

# Gradient Waves
dotnet add package BlazorEffects.GradientWaves

# Fire / Embers
dotnet add package BlazorEffects.FireEmbers

# Starfield
dotnet add package BlazorEffects.Starfield

# Ripple
dotnet add package BlazorEffects.Ripple

# Vortex Tunnel
dotnet add package BlazorEffects.VortexTunnel
```

> **Note:** You don't need to install `BlazorEffects.Core` explicitly — each effect package pulls it in as a dependency.

## Basic Usage

Add an effect component to any `.razor` file. The effect renders as a full-screen animated canvas with your content layered on top.

### Matrix Rain Example

```razor
@page "/"
@using BlazorEffects.MatrixRain

<MatrixRain>
    <div class="flex items-center justify-center h-screen">
        <h1 class="text-4xl text-white font-bold">Welcome to the Matrix</h1>
    </div>
</MatrixRain>
```

### Particle Constellation with a Preset

```razor
@using BlazorEffects.Particles

<ParticleConstellation Config="ParticleConstellationPresets.DeepSpace">
    <MyPageContent />
</ParticleConstellation>
```

### Aurora with Custom Colors

```razor
@using BlazorEffects.Aurora

<AuroraBorealis Colors="@(["#00ff87", "#7b2ff7", "#00b4d8"])" />
```

### Gradient Waves (Stripe-style Background)

```razor
@using BlazorEffects.GradientWaves

<GradientWaves Colors="GradientWavesPresets.Stripe" />
```

## Configuring Parameters

Every effect component exposes `[Parameter]` properties for fine-grained control. Parameters have sensible defaults, so you only need to set what you want to change.

### Common Parameters (All Effects)

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `TargetFps` | `int` | `60` | Target frames per second. Clamped to 1–120. Lower values reduce CPU/GPU usage. |
| `Height` | `string` | `"100vh"` | CSS height of the effect container. Any valid CSS value (`"100vh"`, `"500px"`, `"50%"`). |
| `CssClass` | `string?` | `null` | Optional CSS class applied to the content overlay div. |
| `ChildContent` | `RenderFragment?` | `null` | Content rendered over the animated canvas. |

### Example: Customized Matrix Rain

```razor
<MatrixRain Characters="@MatrixRainPresets.Katakana"
            Color="#ff006e"
            Speed="1.5"
            FontSize="14"
            TargetFps="30"
            Height="100vh">
    <HeroSection />
</MatrixRain>
```

### Example: Click-to-Ripple Interaction

```razor
@using BlazorEffects.Ripple

<Ripple Trigger="click"
        Color="#a78bfa"
        MaxRadius="350"
        Speed="3.5"
        BackgroundColor="#0c0a1f">
    <div class="p-8">
        <h1>Click anywhere to create ripples</h1>
    </div>
</Ripple>
```

### Example: Vortex Tunnel with Multi-Color Rings

```razor
@using BlazorEffects.VortexTunnel

<VortexTunnel Colors="@(["#ef4444", "#f97316", "#eab308", "#22c55e", "#3b82f6", "#8b5cf6"])"
              Shape="polygon"
              PolygonSides="6"
              RotationSpeed="0.04" />
```

## Using Presets

Every effect ships with curated presets for instant professional looks. Presets are static properties on the effect's `*Presets` class.

```csharp
// Particle Constellation presets
ParticleConstellationPresets.Default     // Indigo network
ParticleConstellationPresets.Cyberpunk   // Neon green on dark
ParticleConstellationPresets.DeepSpace   // Cool blue constellation
ParticleConstellationPresets.Amber       // Warm amber particles
ParticleConstellationPresets.Minimal     // Sparse minimal
ParticleConstellationPresets.Dense       // Dense mesh

// Aurora Borealis color palettes
AuroraBorealisPresets.Classic   // Green-purple aurora
AuroraBorealisPresets.Arctic    // Cool blue tones
AuroraBorealisPresets.Sunset    // Warm magenta/rose
AuroraBorealisPresets.Emerald   // Deep greens
AuroraBorealisPresets.Cosmic    // Purple/violet

// Gradient Waves palettes
GradientWavesPresets.Stripe          // Stripe-inspired vibrant
GradientWavesPresets.VibrantSunset   // Orange/magenta
GradientWavesPresets.OceanDeep       // Navy/teal/aqua
GradientWavesPresets.NorthernLights  // Emerald/violet/indigo
GradientWavesPresets.MinimalMono     // Warm grays
GradientWavesPresets.Cyberpunk       // Neon green/magenta

// Matrix Rain character sets
MatrixRainPresets.Classic    // A-Z, 0-9, symbols
MatrixRainPresets.Katakana   // Japanese Katakana
MatrixRainPresets.Binary     // 0s and 1s
MatrixRainPresets.Hex        // 0-9, A-F
MatrixRainPresets.Emojis     // Rocket, sparkles, etc.
```

## Running the Playground / Demo

The **BlazorEffects.Playground** package provides an interactive UI for exploring effects, tweaking parameters in real-time, and exporting ready-to-paste Razor code.

### Install the Playground

```bash
dotnet add package BlazorEffects.Playground
```

### Run the Demo App

If you've cloned the repository:

```bash
git clone https://github.com/cartsp/BlazorUI.git
cd BlazorUI

# Run the AppHost (Blazor Server demo)
dotnet run --project src/AppHost
```

Then navigate to `http://localhost:5000` to see all effects with live controls.

### Run the WebAssembly Demo

```bash
dotnet run --project src/BlazorEffects.Demo.Wasm
```

## Building from Source

```bash
# Clone
git clone https://github.com/cartsp/BlazorUI.git
cd BlazorUI

# Restore, build, test, format check — all in one
./build.sh

# Or step by step
dotnet build BlazorUI.slnx
dotnet test BlazorUI.slnx --verbosity normal
dotnet format BlazorUI.slnx --verify-no-changes
```

### Requirements

- .NET 10 SDK (multi-targets net8.0, net9.0, net10.0)
- Node.js (for Tailwind CSS in the demo app)

## What's Next?

- [API Reference](api-reference.md) — Complete effect parameter documentation
- [CONTRIBUTING.md](../CONTRIBUTING.md) — Creating new effects and submitting PRs
- [Live Demo](https://cartsp.github.io/BlazorUI/) — See all effects in action
