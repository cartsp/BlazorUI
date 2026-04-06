# QA Gate Process — BlazorEffects

> **Owner:** QA Engineer  
> **Purpose:** Prevent visual quality regressions from reaching production  
> **Trigger:** Any PR or task that touches effects, screenshots, playground, or visual assets

---

## 1. When QA Review Is Required

A QA review gate **MUST** be applied before closing any task that involves:

| Category | Examples |
|----------|---------|
| Effect components | New effect, parameter change, JS animation change |
| Screenshots/visual assets | New screenshots, updated README images, hero banner |
| Playground | Parameter editor changes, preset gallery, live preview |
| Build/packaging | Changes to RCL output, JS bundling, CSS |
| Core abstractions | `EffectComponentBase`, `IEffectConfig`, JS interop contract |
| Accessibility | `prefers-reduced-motion`, `aria-hidden`, contrast ratios |

**Rule of thumb:** If the change touches a `.razor` file, a `.js` file in `wwwroot/`, or anything in `screenshots/`, QA must review.

---

## 2. QA Review Checklist

### 2.1 Build Verification (Automated)

Run before every review:

```bash
dotnet build BlazorUI.slnx --no-incremental
dotnet test BlazorUI.slnx --verbosity normal
```

- [ ] Zero build errors
- [ ] Zero build warnings
- [ ] All tests pass (current baseline: **613 tests** across **15 test projects**)

### 2.2 Component Test Coverage

For every effect component (`src/BlazorEffects.{Name}/`):

- [ ] **Component tests** — Renders canvas, applies parameters, handles child content, applies presets, aria-hidden
- [ ] **Config tests** — Default values, `IEffectConfig` implementation, equality, hash consistency, ReducedMotionBehavior
- [ ] **Presets tests** — Non-null presets, valid hex colors, expected values
- [ ] **Descriptor tests** — Parameter definitions, apply/update logic, preset metadata

### 2.3 Accessibility Verification

For every effect:

- [ ] Canvas has `aria-hidden="true"` and `role="presentation"`
- [ ] Config has `ReducedMotionBehavior` property
- [ ] JS implements `applyReducedMotion()` (or equivalent) correctly
- [ ] No erroneous `applyReducedMotion` calls on non-config objects (e.g., `getCanvasSize`, `hexToRgb`)

### 2.4 Playground Integration

For any playground change (`src/BlazorEffects.Playground/`):

- [ ] All 10 effects appear in the effect selector
- [ ] Each effect's parameter editor renders without errors
- [ ] Preset gallery displays gradient thumbnails
- [ ] Live preview renders the selected effect
- [ ] Code export generates valid Razor markup

---

## 3. Standard QA Review Comment Template

When QA reviews a completed task, post this comment on the issue:

```markdown
## 🔍 QA Review — [Component/Feature Name]

**Status:** ✅ Approved / ❌ Issues Found / ⚠️ Conditional

### Build Verification
- [ ] Build: ✅ Clean (0 errors, 0 warnings)
- [ ] Tests: ✅ All N tests pass / ❌ X failures

### Test Coverage
- [ ] Component tests: ✅ N tests / ❌ Missing coverage
- [ ] Config tests: ✅ N tests / ❌ Missing coverage  
- [ ] Presets tests: ✅ N tests / ❌ Missing coverage
- [ ] Descriptor tests: ✅ N tests / ❌ Missing coverage

### Accessibility
- [ ] aria-hidden on canvas: ✅ / ❌
- [ ] ReducedMotionBehavior in config: ✅ / ❌
- [ ] JS applyReducedMotion working: ✅ / ❌

### Issues Found
1. [Description of issue if any]

### Recommendation
[APPROVED for merge / NEEDS FIXES before merge / BLOCKED — escalate to CTO]
```

---

## 4. Regression Prevention

### Pre-Merge QA Gate

1. **Agent completes work** → marks issue `in_review`
2. **QA Engineer reviews** using this checklist
3. **QA posts review comment** with findings
4. If **approved** → QA updates status to `done`
5. If **issues found** → QA adds a comment describing what needs fixing, keeps status `in_review`, and assigns back to the implementer

### New Effect Checklist

When a new effect is added:

1. Create test project at `tests/BlazorEffects.{Name}.Tests/`
2. Implement all 4 standard test files (Component, Config, Presets, Descriptor)
3. Add entry to `tests/QA/visual-qa-checklist.md` Component Status Tracker
4. Add screenshot to `screenshots/` directory
5. Update README effects table
6. Add Playground integration (descriptor, presets, effect registration)
7. Ensure `aria-hidden`, `role="presentation"`, and `ReducedMotionBehavior` are present

---

## 5. Escalation Path

| Situation | Action |
|-----------|--------|
| Quality issue found in review | Post comment on issue, assign back to implementer |
| Implementer disagrees with QA finding | Escalate to CTO (`be84c30a-d010-4f3c-8232-e3bebb82b719`) |
| Feature shipped without QA review | Flag as process violation, file retroactive QA review, escalate to CTO |
| QA blocked on testing | Post blocked comment, escalate to CTO |

---

## 6. Current Codebase Status (as of 2026-04-06)

### Build & Tests
- **Build:** ✅ Clean (0 errors, 0 warnings)
- **Tests:** ✅ **613 tests** passing across **15 test projects**

### Test Coverage per Effect
| Effect | Tests | QA Status |
|--------|-------|-----------|
| MatrixRain | 33 | ✅ Reviewed |
| Aurora | 45 | ✅ Reviewed |
| Particles | 38 | ✅ Reviewed |
| Blobs | 50 | ✅ Reviewed |
| Noise | 55 | ✅ Reviewed |
| Gradient Waves | 56 | ✅ Reviewed |
| Starfield | 37 | ✅ Reviewed (retroactive) |
| Fire/Embers | 37 | ✅ Reviewed (retroactive) |
| Ripple | 37 | ✅ Reviewed (retroactive) |
| Vortex/Tunnel | 40 | ✅ Reviewed (retroactive) |
| Playground | 141 | ✅ Reviewed |
| Domain | 6 | ✅ |
| Application | 2 | ✅ |
| Infrastructure | 1 | ✅ |
| Presentation | 35 | ✅ |
| **TOTAL** | **613** | |

### Known Issues (Tracked)
- GradientWaves has inline reduced-motion check that doesn't respect `ReducedMotionBehavior` enum (always pauses) — minor inconsistency, not blocking
- 8 effects duplicate `applyReducedMotion` inline instead of importing from shared `reduced-motion.js` — code duplication, recommend refactoring post-ship

---

*Last reviewed: 2026-04-06 by QAEngineer*
