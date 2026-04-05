#!/usr/bin/env node
/**
 * Capture clean effect screenshots for the GitHub README.
 *
 * Uses a dedicated capture page (screenshot-capture.html) that renders each
 * effect via its JS module on a full-viewport canvas — no Blazor UI controls.
 *
 * Container sizes are chosen so the internal canvas resolution produces
 * high-quality output at the target size (1280×720):
 *
 *   Aurora:     0.25x scale → container 5120×2880 → canvas 1280×720
 *   Noise:      0.5x  scale → container 2560×1440 → canvas 1280×720
 *   Blobs:      0.5x  scale → container 2560×1440 → canvas 1280×720
 *   MatrixRain: 0.5x  scale → container 2560×1440 → canvas 1280×720
 *   Particles:  1.0x  scale → container 1280×720  → canvas 1280×720
 *
 * Usage:
 *   node scripts/capture-screenshots.mjs
 *   npm run screenshots
 *
 * Requirements:
 *   - .NET 10 SDK (to run the Blazor server)
 *   - playwright and sharp npm packages (already in package.json)
 *   - Playwright browsers installed: npx playwright install chromium
 */

import { chromium } from 'playwright';
import sharp from 'sharp';
import { spawn } from 'child_process';
import { mkdirSync, existsSync } from 'fs';
import { join, dirname } from 'path';
import { fileURLToPath } from 'url';

const __dirname = dirname(fileURLToPath(import.meta.url));
const ROOT = join(__dirname, '..');
const SCREENSHOTS_DIR = join(ROOT, 'screenshots');
mkdirSync(SCREENSHOTS_DIR, { recursive: true });

// The AppHost serves static files on this port (configured in launchSettings.json)
const PORT = process.env.CAPTURE_PORT || 5139;
const BASE_URL = `http://localhost:${PORT}`;
const CAPTURE_URL = `${BASE_URL}/screenshot-capture.html`;

const OUTPUT_W = 1280;
const OUTPUT_H = 720;

// Container dimensions per effect to produce 1280×720 internal canvas.
// Each effect uses a different internal scale factor, so the container
// must compensate to yield the correct canvas resolution.
const EFFECTS = [
  { effect: 'aurora',         file: 'aurora.png',          cw: 5120, ch: 2880, waitMs: 6000 },
  { effect: 'noise',          file: 'noise.png',           cw: 2560, ch: 1440, waitMs: 5000 },
  { effect: 'blobs',          file: 'blobs.png',           cw: 2560, ch: 1440, waitMs: 5000 },
  { effect: 'matrixrain',     file: 'matrix-rain.png',     cw: 2560, ch: 1440, waitMs: 4000 },
  { effect: 'particles',      file: 'particles.png',       cw: 1280, ch: 720,  waitMs: 4000 },
  { effect: 'gradientwaves',  file: 'gradient-waves.png',  cw: 2560, ch: 1440, waitMs: 5000 },
];

// ─── Server Management ─────────────────────────────────────────

let server = null;
let serverReady = false;

function startServer() {
  return new Promise((resolve, reject) => {
    console.log('Starting Blazor server...');
    server = spawn('dotnet', ['run', '--project', join(ROOT, 'src/AppHost'), '--urls', `http://0.0.0.0:${PORT}`], {
      cwd: ROOT,
      stdio: ['ignore', 'pipe', 'pipe'],
      env: { ...process.env, ASPNETCORE_ENVIRONMENT: 'Development' },
    });

    let output = '';
    const timeout = setTimeout(() => {
      killServer();
      reject(new Error(`Server startup timed out after 60s. Last output:\n${output.slice(-500)}`));
    }, 60000);

    const check = (data) => {
      output += data.toString();
      if (output.includes('Now listening')) {
        clearTimeout(timeout);
        serverReady = true;
        resolve();
      }
    };

    server.stdout.on('data', check);
    server.stderr.on('data', check);

    server.on('error', (err) => {
      clearTimeout(timeout);
      reject(new Error(`Failed to start server: ${err.message}`));
    });

    server.on('exit', (code) => {
      if (!serverReady) {
        clearTimeout(timeout);
        reject(new Error(`Server exited unexpectedly with code ${code}. Output:\n${output.slice(-1000)}`));
      }
    });
  });
}

function killServer() {
  if (!server || server.exitCode !== null) return;
  console.log('Shutting down server...');
  try { server.kill('SIGTERM'); } catch {}
  // Give it 2 seconds, then force kill
  setTimeout(() => {
    try { server.kill('SIGKILL'); } catch {}
  }, 2000);
}

// ─── Graceful Shutdown ─────────────────────────────────────────

process.on('SIGINT', () => {
  console.log('\nInterrupted, cleaning up...');
  killServer();
  process.exit(130);
});

process.on('SIGTERM', () => {
  killServer();
  process.exit(143);
});

// ─── Main ──────────────────────────────────────────────────────

async function captureEffect(browser, effect) {
  console.log(`  Capturing ${effect.file} (${effect.effect}, container ${effect.cw}×${effect.ch})...`);

  const ctx = await browser.newContext({
    viewport: { width: effect.cw, height: effect.ch },
    deviceScaleFactor: 1,
  });
  const page = await ctx.newPage();

  try {
    const url = `${CAPTURE_URL}?effect=${effect.effect}&cw=${effect.cw}&ch=${effect.ch}`;
    await page.goto(url, { waitUntil: 'load', timeout: 30000 });

    // Wait for effect JS to initialize
    await page.waitForFunction('window.__effectReady === true', { timeout: 15000 })
      .catch(() => console.log(`    ⚠ Effect readiness timeout for ${effect.effect}, proceeding anyway`));

    // Let animation run for good visual quality
    await page.waitForTimeout(effect.waitMs);

    // Verify canvas dimensions
    const dims = await page.evaluate(() => {
      const c = document.getElementById('effect-canvas');
      return { w: c ? c.width : 0, h: c ? c.height : 0 };
    });
    console.log(`    Canvas internal: ${dims.w}×${dims.h}`);

    if (dims.w === 0 || dims.h === 0) {
      console.log(`    ⚠ Canvas has zero dimensions, skipping ${effect.file}`);
      return false;
    }

    // Screenshot the canvas element directly (no UI chrome)
    const screenshot = await page.locator('#effect-canvas').screenshot({ type: 'png' });

    // Resize to exact output dimensions
    await sharp(screenshot)
      .resize(OUTPUT_W, effect.hero ? 640 : OUTPUT_H, { fit: 'fill' })
      .png({ compressionLevel: 6 })
      .toFile(join(SCREENSHOTS_DIR, effect.file));

    console.log(`    ✓ Saved ${effect.file}`);
    return true;
  } catch (err) {
    console.error(`    ✗ Failed to capture ${effect.file}: ${err.message}`);
    return false;
  } finally {
    await ctx.close();
  }
}

async function main() {
  // Start the Blazor server
  await startServer();
  console.log('Server is ready.\n');

  let captured = 0;
  let failed = 0;

  try {
    const browser = await chromium.launch({ headless: true, args: ['--no-sandbox'] });

    try {
      // Capture each effect
      for (const eff of EFFECTS) {
        const ok = await captureEffect(browser, eff);
        if (ok) captured++; else failed++;
      }

      // Hero banner (blobs effect, wider aspect ratio)
      console.log('  Capturing hero-banner.png...');
      const heroOk = await captureEffect(browser, {
        effect: 'blobs',
        file: 'hero-banner.png',
        cw: 2560,
        ch: 1280,
        waitMs: 4000,
        hero: true,
      });
      if (heroOk) captured++; else failed++;

      console.log(`\n✓ Done: ${captured} captured, ${failed} failed.`);
    } finally {
      await browser.close();
    }
  } finally {
    killServer();
  }

  if (failed > 0) {
    process.exit(1);
  }
}

main().catch((err) => {
  console.error('Fatal error:', err.message);
  killServer();
  process.exit(1);
});
