# Contributing to BlazorEffects

Thank you for your interest in contributing! We're building visually stunning animated effects for Blazor, and we'd love your help.

## Quick Start

1. **Fork** the repository
2. **Clone** your fork: `git clone https://github.com/YOUR_USERNAME/BlazorUI.git`
3. **Build**: `./build.sh` (restores, builds, tests, and checks formatting)
4. **Create a branch**: `git checkout -b feature/your-effect-name`
5. **Make changes** following the patterns below
6. **Submit a PR**

## Development Setup

### Prerequisites

- .NET 10 SDK
- Node.js (for Tailwind CSS in the demo app)

### Build & Test

```bash
# Full verification (build + test + format)
./build.sh

# Individual steps
dotnet build BlazorUI.slnx
dotnet test BlazorUI.slnx --verbosity normal
dotnet format BlazorUI.slnx --verify-no-changes
```

All three must pass before submitting a PR.

## Creating a New Effect

Each effect is an independent Razor Class Library. Follow this checklist:

### 1. Create the Project

```bash
# Under src/
mkdir -p src/BlazorEffects.YourEffect/wwwroot
```

Create `src/BlazorEffects.YourEffect/BlazorEffects.YourEffect.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">
  <PropertyGroup>
    <TargetFrameworks>net8.0;net9.0;net10.0</TargetFrameworks>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <IsPackable>true</IsPackable>
    <PackageId>BlazorEffects.YourEffect</PackageId>
    <Description>Your effect description</Description>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\BlazorEffects.Core\BlazorEffects.Core.csproj" />
  </ItemGroup>

  <!-- Multi-target Blazor references (copy from an existing effect) -->
</Project>
```

### 2. Required Files

Every effect MUST include:

| File | Purpose |
|------|---------|
| `YourEffect.razor` | Blazor component inheriting `EffectComponentBase` |
| `YourEffectConfig.cs` | Typed config with sensible defaults |
| `YourEffectPresets.cs` | Curated preset configurations |
| `YourEffectDescriptor.cs` | Implements `IEffectDescriptor<TConfig>` for playground |
| `wwwroot/your-effect.js` | Canvas animation (init/update/dispose lifecycle) |
| `GlobalUsings.cs` | Common using statements |

### 3. Component Pattern

```csharp
// YourEffect.razor.cs or @code block in .razor
@inherits BlazorEffects.Core.Animation.EffectComponentBase

<div style="position:relative;height:@Height;overflow:hidden;">
    <canvas @ref="CanvasElement"
            style="position:absolute;top:0;left:0;width:100%;height:100%;">
    </canvas>
    @if (ChildContent is not null)
    {
        <div style="position:relative;z-index:1;">
            @ChildContent
        </div>
    }
</div>

@code {
    // Expose parameters with defaults
    [Parameter] public string Color { get; set; } = "#ffffff";
    
    protected override string ModulePath => "./_content/BlazorEffects.YourEffect/your-effect.js";
    
    protected override object BuildConfig() => new YourEffectConfig { Color = Color };
    
    protected override string ComputeConfigHash() => $"{Color}";
}
```

### 4. JavaScript Module Contract

Your JS module MUST export three functions:

```javascript
export function init(canvas, config) {
    // Set up canvas, start animation loop
    // Return a unique instance ID (for update/dispose)
    return crypto.randomUUID();
}

export function update(instanceId, config) {
    // Apply updated config to running instance
}

export function dispose(instanceId) {
    // Cancel animation frame, clean up
}
```

### 5. Performance Requirements

- **60fps target** — Use `requestAnimationFrame`, not `setInterval`
- **Pause when hidden** — Check `document.hidden` or use `requestAnimationFrame` (auto-pauses)
- **Config diffing** — Only re-render when parameters change (handled by `EffectComponentBase`)
- **Proper disposal** — Cancel animation frames, dereference canvases on dispose

### 6. Testing

Create `tests/BlazorEffects.YourEffect.Tests/` with tests for:

- Config default values
- Preset correctness (colors valid, values in range)
- Descriptor returns correct parameter definitions
- Descriptor applies parameter changes correctly

### 7. Add to Solution

Add both the project and test project to `BlazorUI.slnx`.

## Code Style

- **C# 14** features: primary constructors, collection expressions, pattern matching
- **File-scoped namespaces**
- **One class per file**, file name matches class name
- **PascalCase** for public members, `_camelCase` for private fields
- **No compiler warnings** — treat warnings as errors

## Commit Messages

Follow [Conventional Commits](https://www.conventionalcommits.org/):

```
feat(effects): add Starfield animated background effect
fix(particles): resolve memory leak on rapid config changes
test(aurora): add preset validation tests
docs(readme): update installation instructions
```

## PR Checklist

Before submitting:

- [ ] `./build.sh` passes cleanly
- [ ] New effect follows the component pattern (inherits `EffectComponentBase`)
- [ ] Config has sensible defaults
- [ ] At least 3 presets provided
- [ ] JS module implements init/update/dispose
- [ ] Tests cover config, presets, and descriptor
- [ ] Project added to `BlazorUI.slnx`
- [ ] No compiler warnings
- [ ] Code formatted (`dotnet format`)

## Questions?

Open an issue with the `question` label and we'll help you out.
