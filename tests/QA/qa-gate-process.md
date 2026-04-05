# QA Gate Process — BlazorEffects

> **Owner:** QA Engineer  
> **Purpose:** Prevent visual quality regressions from reaching the board/production  
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
- [ ] All tests pass (current baseline: 315 tests)

### 2.2 Screenshot Quality (Visual Deliverables)

For every screenshot in `screenshots/`:

- [ ] **Canvas-only capture** — No UI controls, navigation, scrollbars, or browser chrome visible
- [ ] **Effect renders correctly** — Not static/blank/noise/broken; animation patterns are visible
- [ ] **Color quality** — Colors are vibrant and match the effect's intended palette
- [ ] **Resolution** — 1280×720 minimum for effect screenshots; 1280×640 for hero banner
- [ ] **File size reasonable** — Under 500KB for compressed effects; noise effect may be larger due to compression characteristics
- [ ] **README table** — All effect images referenced in README are present and up-to-date

### 2.3 Component Test Coverage

For every effect component (`src/BlazorEffects.{Name}/`):

- [ ] **Component tests** — Renders canvas, applies parameters, handles child content, applies presets
- [ ] **Config tests** — Default values, `IEffectConfig` implementation, equality, hash consistency
- [ ] **Presets tests** — Non-null presets, valid hex colors, expected values
- [ ] **Descriptor tests** — Parameter definitions, apply/update logic, preset metadata

### 2.4 Playground Integration

For any playground change (`src/BlazorEffects.Playground/`):

- [ ] All 5 effects appear in the effect selector
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

### Visual Quality (if applicable)
- [ ] Screenshots: ✅ Canvas-only / ❌ Contains UI chrome
- [ ] Effects render: ✅ All 5 effects verified / ❌ [details]
- [ ] File quality: ✅ Resolution and size acceptable / ❌ [details]

### Test Coverage
- [ ] Component tests: ✅ N tests / ❌ Missing coverage
- [ ] Config tests: ✅ N tests / ❌ Missing coverage  
- [ ] Presets tests: ✅ N tests / ❌ Missing coverage

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

### Screenshot Regression

After any change to effect rendering or screenshot capture:

1. Run `node scripts/capture-screenshots.mjs` to regenerate all screenshots
2. Compare new screenshots against previous versions
3. Verify all 5 effects + hero banner are present
4. Check for visual regressions (color shifts, blank canvases, UI chrome)

### New Effect Checklist

When a new effect is added:

1. Create test project at `tests/BlazorEffects.{Name}.Tests/`
2. Implement all 3 standard test files (Component, Config, Presets)
3. Add descriptor tests if playground integration is included
4. Add entry to `tests/QA/visual-qa-checklist.md` Component Status Tracker
5. Add screenshot to `screenshots/` directory
6. Update README effects table

---

## 5. Escalation Path

| Situation | Action |
|-----------|--------|
| Quality issue found in review | Post comment on issue, assign back to implementer |
| Implementer disagrees with QA finding | Escalate to CTO (`be84c30a-d010-4f3c-8232-e3bebb82b719`) |
| Board reports a visual defect | QA creates an issue, assigns to implementer, tracks to resolution |
| QA blocked on testing (e.g., can't run playground) | Post blocked comment, escalate to CTO |

---

## 6. Current Codebase Status (as of 2026-04-05)

### Build & Tests
- **Build:** ✅ Clean (0 errors, 0 warnings)
- **Tests:** ✅ 315 tests passing across 10 test projects

### Screenshot Review
| Screenshot | Canvas-Only | Renders Correctly | Quality | Size |
|------------|------------|-------------------|---------|------|
| aurora.png | ✅ | ✅ Smooth flowing aurora bands | ✅ Excellent | 130KB |
| blobs.png | ✅ | ✅ Organic gradient blobs | ✅ Excellent | 432KB |
| matrix-rain.png | ✅ | ✅ Cascading character streams | ✅ Excellent | 405KB |
| noise.png | ✅ | ✅ Simplex noise texture | ✅ Good | 2.0MB* |
| particles.png | ✅ | ✅ Connected particle network | ✅ Excellent | 303KB |
| hero-banner.png | ✅ | ✅ Morphing blob composition | ✅ Excellent | 405KB |

*\* noise.png is 2MB because simplex noise produces near-random pixel data that compresses poorly. This is expected and not a quality issue.*

### Test Coverage per Effect
| Effect | Component | Config | Presets | Descriptor | Total |
|--------|-----------|--------|---------|------------|-------|
| MatrixRain | 10 | 4 | 5 | 14 | 33 |
| Aurora | 16 | 6 | 5 | 18 | 45 |
| Particles | 12 | 5 | 7 | 14 | 38 |
| Blobs | 17 | 6 | 7 | 20* | 50 |
| Noise | 19 | 8 | 8 | 20 | 55 |

*\* Blobs descriptor tests are in a separate MorphingBlobsDescriptorTests.cs file.*

### Outstanding Issues
- **None** — All 5 effects have complete test coverage and render correctly
- The visual QA checklist tracker has been updated to reflect current status

---

*Last reviewed: 2026-04-05 by QAEngineer*
