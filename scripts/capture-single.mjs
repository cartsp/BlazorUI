#!/usr/bin/env node
/**
 * Capture a single effect screenshot (for re-capturing problematic effects).
 * Usage: node scripts/capture-single.mjs <effect> <output-file> <cw> <ch> [waitMs]
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

const effect = process.argv[2] || 'fireembers';
const file = process.argv[3] || 'fire-embers.png';
const cw = parseInt(process.argv[4] || '1280', 10);
const ch = parseInt(process.argv[5] || '720', 10);
const waitMs = parseInt(process.argv[6] || '5000', 10);

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
      reject(new Error(`Server startup timed out after 60s.`));
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
    server.on('error', (err) => { clearTimeout(timeout); reject(err); });
    server.on('exit', (code) => { if (!serverReady) { clearTimeout(timeout); reject(new Error(`Server exited: ${code}`)); } });
  });
}

function killServer() {
  if (!server || server.exitCode !== null) return;
  console.log('Shutting down server...');
  try { server.kill('SIGTERM'); } catch {}
  setTimeout(() => { try { server.kill('SIGKILL'); } catch {} }, 2000);
}

process.on('SIGINT', () => { killServer(); process.exit(130); });
process.on('SIGTERM', () => { killServer(); process.exit(143); });

async function main() {
  await startServer();
  console.log('Server ready.\n');

  try {
    const browser = await chromium.launch({ headless: true, args: ['--no-sandbox'] });
    try {
      console.log(`Capturing ${file} (${effect}, container ${cw}×${ch})...`);
      const ctx = await browser.newContext({ viewport: { width: cw, height: ch }, deviceScaleFactor: 1 });
      const page = await ctx.newPage();

      const url = `${CAPTURE_URL}?effect=${effect}&cw=${cw}&ch=${ch}`;
      await page.goto(url, { waitUntil: 'load', timeout: 30000 });
      await page.waitForFunction('window.__effectReady === true', { timeout: 15000 })
        .catch(() => console.log('⚠ Readiness timeout, proceeding'));
      await page.waitForTimeout(waitMs);

      const dims = await page.evaluate(() => {
        const c = document.getElementById('effect-canvas');
        return { w: c ? c.width : 0, h: c ? c.height : 0 };
      });
      console.log(`Canvas internal: ${dims.w}×${dims.h}`);

      const screenshot = await page.locator('#effect-canvas').screenshot({ type: 'png' });
      await sharp(screenshot).resize(OUTPUT_W, OUTPUT_H, { fit: 'fill' }).png({ compressionLevel: 6 }).toFile(join(SCREENSHOTS_DIR, file));
      console.log(`✓ Saved ${file}`);

      await ctx.close();
    } finally {
      await browser.close();
    }
  } finally {
    killServer();
  }
}

main().catch((err) => { console.error('Fatal:', err.message); killServer(); process.exit(1); });
