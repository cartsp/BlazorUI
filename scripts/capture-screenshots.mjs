#!/usr/bin/env node
/**
 * Capture clean effect screenshots for the GitHub README.
 *
 * Uses a dedicated capture page that renders each effect via its JS module.
 * Container sizes are chosen so the internal canvas resolution matches the
 * desired output (1280x720):
 *
 *   Aurora/Noise:  0.25x scale → container 5120x2880 → canvas 1280x720
 *   Blobs/Matrix:  0.5x scale → container 2560x1440 → canvas 1280x720
 *   Particles:     1.0x scale → container 1280x720 → canvas 1280x720
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

const BASE_URL = 'http://localhost:5139';
const CAPTURE_URL = `${BASE_URL}/screenshot-capture.html`;
const OUTPUT_W = 1280;
const OUTPUT_H = 720;

// Container dimensions per effect to produce 1280x720 internal canvas
const EFFECTS = [
  { effect: 'aurora',     file: 'aurora.png',      cw: 5120, ch: 2880, waitMs: 6000 },
  { effect: 'noise',      file: 'noise.png',       cw: 5120, ch: 2880, waitMs: 5000 },
  { effect: 'blobs',      file: 'blobs.png',       cw: 2560, ch: 1440, waitMs: 5000 },
  { effect: 'matrixrain', file: 'matrix-rain.png', cw: 2560, ch: 1440, waitMs: 4000 },
  { effect: 'particles',  file: 'particles.png',   cw: 1280, ch: 720,  waitMs: 4000 },
];

// Start the Blazor server
console.log('Starting Blazor server...');
const server = spawn('dotnet', ['run', '--project', join(ROOT, 'src/AppHost')], {
  cwd: ROOT,
  stdio: ['ignore', 'pipe', 'pipe'],
  env: { ...process.env, ASPNETCORE_ENVIRONMENT: 'Development' },
});

await new Promise((resolve, reject) => {
  let output = '';
  const timeout = setTimeout(() => reject(new Error('Server startup timed out')), 60000);
  const check = (data) => {
    output += data.toString();
    if (output.includes('Now listening')) { clearTimeout(timeout); resolve(); }
  };
  server.stdout.on('data', check);
  server.stderr.on('data', check);
});

console.log('Server is ready.');

try {
  const browser = await chromium.launch({ headless: true, args: ['--no-sandbox'] });

  for (const eff of EFFECTS) {
    console.log(`Capturing ${eff.file} (${eff.effect}, container ${eff.cw}x${eff.ch})...`);

    const ctx = await browser.newContext({
      viewport: { width: eff.cw, height: eff.ch },
      deviceScaleFactor: 1,
    });
    const page = await ctx.newPage();

    const url = `${CAPTURE_URL}?effect=${eff.effect}&cw=${eff.cw}&ch=${eff.ch}`;
    await page.goto(url, { waitUntil: 'load' });

    // Wait for effect JS to initialize
    await page.waitForFunction('window.__effectReady === true', { timeout: 15000 })
      .catch(() => console.log(`  ⚠ Effect readiness timeout, proceeding anyway`));

    // Let animation run for good visual quality
    await page.waitForTimeout(eff.waitMs);

    // Verify canvas dimensions
    const dims = await page.evaluate(() => {
      const c = document.getElementById('effect-canvas');
      return { w: c.width, h: c.height };
    });
    console.log(`  Canvas internal: ${dims.w}x${dims.h}`);

    // Screenshot the canvas element
    const screenshot = await page.locator('#effect-canvas').screenshot({ type: 'png' });

    // Resize to exact output dimensions
    await sharp(screenshot)
      .resize(OUTPUT_W, OUTPUT_H, { fit: 'fill' })
      .png({ compressionLevel: 6 })
      .toFile(join(SCREENSHOTS_DIR, eff.file));

    console.log(`  ✓ Saved ${eff.file}`);
    await ctx.close();
  }

  // Hero banner
  console.log('Capturing hero-banner.png...');
  const heroCtx = await browser.newContext({
    viewport: { width: 2560, height: 1280 },
    deviceScaleFactor: 1,
  });
  const heroPage = await heroCtx.newPage();
  await heroPage.goto(`${CAPTURE_URL}?effect=blobs&cw=2560&ch=1280`, { waitUntil: 'load' });
  await heroPage.waitForFunction('window.__effectReady === true', { timeout: 15000 })
    .catch(() => {});
  await heroPage.waitForTimeout(4000);

  const heroScreenshot = await heroPage.locator('#effect-canvas').screenshot({ type: 'png' });
  await sharp(heroScreenshot)
    .resize(1280, 640, { fit: 'fill' })
    .png({ compressionLevel: 6 })
    .toFile(join(SCREENSHOTS_DIR, 'hero-banner.png'));
  console.log('  ✓ Saved hero-banner.png');
  await heroCtx.close();

  await browser.close();
  console.log('\nAll screenshots captured successfully!');
} finally {
  console.log('Shutting down server...');
  server.kill('SIGTERM');
  await new Promise(r => setTimeout(r, 2000));
  try { server.kill('SIGKILL'); } catch {}
}
