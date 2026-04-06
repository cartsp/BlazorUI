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
- [ ] Applies `CssClass` parameter
- [ ] Has `aria-hidden="true"` and `role="presentation"` on canvas

#### Config Tests (`{ComponentName}ConfigTests.cs`)
- [ ] Default config has expected values
- [ ] Config implements `IEffectConfig`
- [ ] Config equality works
- [ ] Config hash is consistent
- [ ] `ReducedMotionBehavior` property present with default "Minimal"

#### Presets Tests (`{ComponentName}PresetsTests.cs`)
- [ ] All presets are non-null
- [ ] All color values are valid hex
- [ ] Expected preset names exist

#### Descriptor Tests (`{ComponentName}DescriptorTests.cs`)
- [ ] Descriptor implements `IEffectDescriptor<TConfig>`
- [ ] Parameter definitions are correct
- [ ] Apply/update logic works
- [ ] Preset metadata is valid

---

## Component Status Tracker

| Component | Issue | Component Tests | Config Tests | Presets Tests | Descriptor Tests | Total | QA Status |
|-----------|-------|----------------|-------------|--------------|-----------------|-------|-----------|
| MatrixRain | [AIE-18](/AIE/issues/AIE-18) | 10 | 4 | 5 | 14 | 33 | ✅ QA Reviewed |
| Aurora | [AIE-14](/AIE/issues/AIE-14) | 16 | 6 | 5 | 18 | 45 | ✅ QA Reviewed |
| Particles | [AIE-16](/AIE/issues/AIE-16) | 12 | 5 | 7 | 14 | 38 | ✅ QA Reviewed |
| Blobs | [AIE-17](/AIE/issues/AIE-17) | 17 | 6 | 7 | 20 | 50 | ✅ QA Reviewed |
| Noise | [AIE-19](/AIE/issues/AIE-19) | 19 | 8 | 8 | 20 | 55 | ✅ QA Reviewed |
| Gradient Waves | [AIE-44](/AIE/issues/AIE-44) | — | — | — | — | 56 | ✅ QA Reviewed |
| Starfield | [AIE-73](/AIE/issues/AIE-73) | — | — | — | — | 37 | ✅ QA Reviewed (retroactive) |
| Fire/Embers | [AIE-73](/AIE/issues/AIE-73) | — | — | — | — | 37 | ✅ QA Reviewed (retroactive) |
| Ripple | [AIE-74](/AIE/issues/AIE-74) | — | — | — | — | 37 | ✅ QA Reviewed (retroactive) |
| Vortex/Tunnel | [AIE-74](/AIE/issues/AIE-74) | — | — | — | — | 40 | ✅ QA Reviewed (retroactive) |
| **TOTAL** | | | | | | **613** | |

> **Note:** When adding a new effect component, create a test project at `tests/BlazorEffects.{ComponentName}.Tests/` with the four standard test files. Follow the established pattern from MatrixRain or Blobs.
