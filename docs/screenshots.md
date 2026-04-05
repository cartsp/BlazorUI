# Screenshot Capture Guide

This document explains how to regenerate the effect screenshots used in the README and GitHub repository.

## Overview

Screenshots are captured using a dedicated **screenshot-capture.html** page that renders each effect on a full-viewport canvas — no Blazor UI controls, no chrome, just the raw animated effect. A Node.js script automates the process:

1. Starts the Blazor AppHost server
2. Opens each effect in Playwright (headless Chromium)
3. Waits for the animation to reach visual maturity
4. Screenshots the `<canvas>` element directly
5. Resizes to the target dimensions with sharp
6. Writes to `screenshots/*.png`

## Prerequisites

- **.NET 10 SDK** — to run the Blazor server
- **Node.js** (v18+) — to run the capture script
- **npm packages** — `playwright` and `sharp` (already in `package.json`)
- **Playwright Chromium** — install with:

  ```bash
  npm run screenshots:install
  # or: npx playwright install chromium
  ```

## Quick Start

```bash
# One-time: install Playwright browser
npm run screenshots:install

# Regenerate all screenshots
npm run screenshots
```

This will:
- Start the Blazor server on port 5139
- Capture 6 screenshots (~30 seconds total)
- Save them to the `screenshots/` directory
- Shut down the server automatically

## Output Files

| File | Effect | Resolution | Notes |
|------|--------|------------|-------|
| `aurora.png` | Aurora Borealis | 1280×720 | Flowing light ribbons |
| `noise.png` | Noise Field | 1280×720 | Animated gradient noise |
| `blobs.png` | Morphing Blobs | 1280×720 | Organic gradient blobs |
| `matrix-rain.png` | Matrix Rain | 1280×720 | Cascading character rain |
| `particles.png` | Particle Constellation | 1280×720 | Connected particle network |
| `hero-banner.png` | Morphing Blobs | 1280×640 | Wide banner variant |

## How It Works

### Resolution Scaling

Each effect renders at a different internal scale factor for performance:

| Effect | Scale Factor | Container Size | Canvas Resolution |
|--------|-------------|----------------|-------------------|
| Aurora | 0.25× | 5120×2880 | 1280×720 |
| Noise | 0.5× | 2560×1440 | 1280×720 |
| Blobs | 0.5× | 2560×1440 | 1280×720 |
| MatrixRain | 0.5× | 2560×1440 | 1280×720 |
| Particles | 1.0× | 1280×720 | 1280×720 |

The capture script sets the browser viewport to the container size. Each effect's JavaScript reads the container dimensions and applies its internal scale factor, producing the exact canvas resolution needed.

### Capture Page

`src/AppHost/wwwroot/screenshot-capture.html` is a static HTML page that:

1. Parses `?effect=<name>&cw=<width>&ch=<height>` from the URL
2. Dynamically imports the correct effect JS module
3. Creates a full-viewport `<canvas>` inside a container div
4. Initializes the effect with default configuration
5. Sets `window.__effectReady = true` to signal readiness

### Capture Script

`scripts/capture-screenshots.mjs` handles:

- Server lifecycle (start, wait for "Now listening", shutdown on exit/signal)
- Browser context creation with correct viewport per effect
- Effect readiness detection via `window.__effectReady`
- Canvas element screenshot (not viewport — avoids capturing any chrome)
- Resize/crop to exact target dimensions via sharp

## Customization

### Change the Server Port

```bash
CAPTURE_PORT=8080 npm run screenshots
```

### Add a New Effect

1. Add the effect module path to `EFFECT_MODULES` in `screenshot-capture.html`
2. Add default config to `DEFAULT_CONFIGS` in `screenshot-capture.html`
3. Add an entry to the `EFFECTS` array in `scripts/capture-screenshots.mjs`:

   ```js
   { effect: 'myeffect', file: 'my-effect.png', cw: 2560, ch: 1440, waitMs: 5000 },
   ```

   Set `cw`/`ch` based on the effect's scale factor to produce 1280×720 canvas output.

### Adjust Wait Times

Increase `waitMs` in the `EFFECTS` array for effects that need more time to develop visually interesting patterns.

## Troubleshooting

| Problem | Solution |
|---------|----------|
| `Server startup timed out` | Check port 5139 isn't in use: `lsof -i :5139` |
| `browserType.launch: Executable doesn't exist` | Run `npm run screenshots:install` |
| Blank/black screenshots | Check the effect JS loads correctly — open `http://localhost:5139/screenshot-capture.html?effect=aurora&cw=1280&ch=720` in a browser |
| Canvas dimensions are 0×0 | The effect's `getCanvasSize()` reads the container's bounding rect — ensure `cw`/`ch` params are valid |
| `Error: Cannot find module 'playwright'` | Run `npm install` |

## File Locations

```
src/AppHost/wwwroot/screenshot-capture.html  ← Capture page (served as static file)
scripts/capture-screenshots.mjs              ← Capture automation script
screenshots/                                  ← Output directory
```
