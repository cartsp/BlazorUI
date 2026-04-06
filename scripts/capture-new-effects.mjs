#!/usr/bin/env node
/**
 * Capture screenshots for the 4 new v0.3.0 effects only:
 * Starfield, FireEmbers, Ripple, VortexTunnel
 */

import { chromium } from 'playwright';
import sharp from 'sharp';
import { spawn } from 'child_process';
import { mkdirSync } from 'fs';
import { join, dirname } from 'path';
import { fileURLToPath } from 'url';

const __dirname = dirname(fileURLToPath(import.meta.url));
const ROOT = join(__dirname, '..');
const SCREENSHOTS_DIR = join(ROOT, 'screenshots');
mkdirSync(SCREENSHOTS_DIR, { recursive: true });

const PORT = process.env.CAPTURE_PORT || 5139;
const BASE_URL = `http://localhost:${PORT}`;
const CAPTURE_URL = `${BASE_URL}/screenshot-capture.html`;

const OUTPUT_W = 1280;
const OUTPUT_H = 720;

const EFFECTS = [
  { effect: 'starfield',      file: 'starfield.png',       cw: 1280, ch: 720,  waitMs: 4000 },
  { effect: 'fireembers',     file: 'fire-embers.png',     cw: 1280, ch: 720,  waitMs: 5000 },
  { effect: 'ripple',         file: 'ripple.png',          cw: 1280, ch: 720,  waitMs: 5000 },
  { effect: 'vortextunnel',   file: 'vortex-tunnel.png',   cw: 1280, ch: 720,  waitMs: 4000 },
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
  setTimeout(() => {
    try { server.kill('SIGKILL'); } catch {}
  }, 2000);
}

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

    await page.waitForFunction('window.__effectReady === true', { timeout: 15000 })
      .catch(() => console.log(`    ⚠ Effect readiness timeout for ${effect.effect}, proceeding anyway`));

    await page.waitForTimeout(effect.waitMs);

    const dims = await page.evaluate(() => {
      const c = document.getElementById('effect-canvas');
      return { w: c ? c.width : 0, h: c ? c.height : 0 };
    });
    console.log(`    Canvas internal: ${dims.w}×${dims.h}`);

    if (dims.w === 0 || dims.h === 0) {
      console.log(`    ⚠ Canvas has zero dimensions, skipping ${effect.file}`);
      return false;
    }

    const screenshot = await page.locator('#effect-canvas').screenshot({ type: 'png' });

    await sharp(screenshot)
      .resize(OUTPUT_W, OUTPUT_H, { fit: 'fill' })
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
  await startServer();
  console.log('Server is ready.\n');

  let captured = 0;
  let failed = 0;

  try {
    const browser = await chromium.launch({ headless: true, args: ['--no-sandbox'] });

    try {
      for (const eff of EFFECTS) {
        const ok = await captureEffect(browser, eff);
        if (ok) captured++; else failed++;
      }

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
