# Visual QA Checklist — BlazorEffects Components

Use this template for each new effect component. Check off items as they are verified.

---

## Per-Component QA Checklist

### Component: __`{ComponentName}`__ | Issue: [AIE-XX](/AIE/issues/AIE-XX)

#### bUnit Component Tests (`{ComponentName}ComponentTests.cs`)
- [ ] Renders `<canvas>` element
- [ ] Renders with default height (`100vh`)
- [ ] Applies custom `Height` parameter
- [ ] Renders child content inside overlay div
- [ ] Does not render overlay when no child content
- [ ] Applies `CssClass` to overlay
- [ ] Applies canvas `Opacity` via inline style
- [ ] Renders with all presets without errors
- [ ] Renders with all custom parameters without errors
- [ ] Component-specific parameters (e.g., Colors, BlendMode) render correctly

#### Config Tests (`{ComponentName}ConfigTests.cs`)
- [ ] Default config has expected values matching component defaults
- [ ] Config implements `IEffectConfig`
- [ ] Custom values stored correctly
- [ ] Config equality/inequality works for scalar properties
- [ ] Array property element-wise matching works

#### Presets Tests (`{ComponentName}PresetsTests.cs`)
- [ ] Each preset returns expected colors/values
- [ ] All presets return non-null, non-empty arrays
- [ ] All preset hex colors are valid format (`#` + 7 chars)

#### Config-Hash Diffing
- [ ] `ComputeConfigHash()` returns consistent hash for same parameters
- [ ] Hash changes when any parameter changes (triggers JS `update()`)

#### Build Verification
- [ ] RCL project builds without errors (`dotnet build`)
- [ ] Test project builds and all tests pass (`dotnet test`)
- [ ] No compiler warnings

---

## Component Status Tracker

| Component | Issue | Component Tests | Config Tests | Presets Tests | Build | Status |
|-----------|-------|----------------|-------------|---------------|-------|--------|
| Matrix Rain | [AIE-18](/AIE/issues/AIE-18) | ✅ 10 tests | ✅ 4 tests | ✅ 5 tests | ✅ | Complete |
| Aurora Borealis | [AIE-14](/AIE/issues/AIE-14) | ✅ 15 tests | ✅ 5 tests | ✅ 7 tests | ✅ | Complete |
| Particle Constellation | [AIE-16](/AIE/issues/AIE-16) | ✅ 13 tests | ✅ 5 tests | ✅ 7 tests | ✅ | Complete |
| Morphing Gradient Blobs | [AIE-17](/AIE/issues/AIE-17) | ✅ 16 tests | ✅ 6 tests | ✅ 9 tests | ✅ | Complete |
| Noise Field | [AIE-19](/AIE/issues/AIE-19) | ⬚ Pending | ⬚ Pending | ⬚ Pending | ⬚ | Queued |

> **Note:** When adding a new effect component, create a test project at `tests/BlazorEffects.{ComponentName}.Tests/` with the three standard test files. Follow the established pattern from MatrixRain or Blobs.
