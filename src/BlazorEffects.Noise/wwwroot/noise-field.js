/**
 * BlazorEffects.Noise - Noise Field animated texture effect
 *
 * Exports: init, update, dispose (standard EffectComponentBase contract)
 *
 * Rendering technique: Simplex noise (3D: x, y, time) with fractal Brownian motion
 * for rich, layered textures. Noise values mapped to a configurable color gradient
 * via a pre-computed 256-entry palette LUT for maximum per-pixel performance.
 *
 * Computed at 0.5x resolution with bilinear canvas upscaling for smooth,
 * organic visuals. Uses domain warping (noise-of-noise) for flowing,
 * lava-lamp-like motion with multi-stop color gradients.
 */

// Instance management
const instances = new Map();
let nextId = 0;

// ─── Simplex Noise (3D) ──────────────────────────────────────────
// Based on Stefan Gustavson's implementation.
// 3D noise uses x,y for spatial dimensions, z for time evolution.

const F3 = 1.0 / 3.0;
const G3 = 1.0 / 6.0;

const grad3 = [
    [1,1,0],[-1,1,0],[1,-1,0],[-1,-1,0],
    [1,0,1],[-1,0,1],[1,0,-1],[-1,0,-1],
    [0,1,1],[0,-1,1],[0,1,-1],[0,-1,-1]
];

function buildPerm(seed) {
    const p = new Uint8Array(256);
    for (let i = 0; i < 256; i++) p[i] = i;
    let s = seed | 0;
    for (let i = 255; i > 0; i--) {
        s = (s * 16807 + 0) % 2147483647;
        const j = s % (i + 1);
        [p[i], p[j]] = [p[j], p[i]];
    }
    const perm = new Uint8Array(512);
    const permMod12 = new Uint8Array(512);
    for (let i = 0; i < 512; i++) {
        perm[i] = p[i & 255];
        permMod12[i] = perm[i] % 12;
    }
    return { perm, permMod12 };
}

function simplex3D(permData, x, y, z) {
    const { perm, permMod12 } = permData;

    const s = (x + y + z) * F3;
    const i = Math.floor(x + s);
    const j = Math.floor(y + s);
    const k = Math.floor(z + s);

    const t = (i + j + k) * G3;
    const X0 = i - t;
    const Y0 = j - t;
    const Z0 = k - t;
    const x0 = x - X0;
    const y0 = y - Y0;
    const z0 = z - Z0;

    let i1, j1, k1, i2, j2, k2;
    if (x0 >= y0) {
        if (y0 >= z0) { i1=1; j1=0; k1=0; i2=1; j2=1; k2=0; }
        else if (x0 >= z0) { i1=1; j1=0; k1=0; i2=1; j2=0; k2=1; }
        else { i1=0; j1=0; k1=1; i2=1; j2=0; k2=1; }
    } else {
        if (y0 < z0) { i1=0; j1=0; k1=1; i2=0; j2=1; k2=1; }
        else if (x0 < z0) { i1=0; j1=1; k1=0; i2=0; j2=1; k2=1; }
        else { i1=0; j1=1; k1=0; i2=1; j2=1; k2=0; }
    }

    const x1 = x0 - i1 + G3;
    const y1 = y0 - j1 + G3;
    const z1 = z0 - k1 + G3;
    const x2 = x0 - i2 + 2.0 * G3;
    const y2 = y0 - j2 + 2.0 * G3;
    const z2 = z0 - k2 + 2.0 * G3;
    const x3 = x0 - 1.0 + 3.0 * G3;
    const y3 = y0 - 1.0 + 3.0 * G3;
    const z3 = z0 - 1.0 + 3.0 * G3;

    const ii = i & 255;
    const jj = j & 255;
    const kk = k & 255;

    let n0 = 0, n1 = 0, n2 = 0, n3 = 0;

    let t0 = 0.6 - x0*x0 - y0*y0 - z0*z0;
    if (t0 >= 0) {
        const gi0 = permMod12[ii + perm[jj + perm[kk]]];
        t0 *= t0;
        n0 = t0 * t0 * (grad3[gi0][0]*x0 + grad3[gi0][1]*y0 + grad3[gi0][2]*z0);
    }

    let t1 = 0.6 - x1*x1 - y1*y1 - z1*z1;
    if (t1 >= 0) {
        const gi1 = permMod12[ii + i1 + perm[jj + j1 + perm[kk + k1]]];
        t1 *= t1;
        n1 = t1 * t1 * (grad3[gi1][0]*x1 + grad3[gi1][1]*y1 + grad3[gi1][2]*z1);
    }

    let t2 = 0.6 - x2*x2 - y2*y2 - z2*z2;
    if (t2 >= 0) {
        const gi2 = permMod12[ii + i2 + perm[jj + j2 + perm[kk + k2]]];
        t2 *= t2;
        n2 = t2 * t2 * (grad3[gi2][0]*x2 + grad3[gi2][1]*y2 + grad3[gi2][2]*z2);
    }

    let t3 = 0.6 - x3*x3 - y3*y3 - z3*z3;
    if (t3 >= 0) {
        const gi3 = permMod12[ii + 1 + perm[jj + 1 + perm[kk + 1]]];
        t3 *= t3;
        n3 = t3 * t3 * (grad3[gi3][0]*x3 + grad3[gi3][1]*y3 + grad3[gi3][2]*z3);
    }

    return 32.0 * (n0 + n1 + n2 + n3);
}

// ─── Fractal Brownian Motion ──────────────────────────────────────

function fbm(permData, x, y, z, octaves, persistence, lacunarity) {
    let value = 0;
    let amplitude = 1.0;
    let frequency = 1.0;
    let maxValue = 0;

    for (let i = 0; i < octaves; i++) {
        value += amplitude * simplex3D(permData, x * frequency, y * frequency, z * frequency);
        maxValue += amplitude;
        amplitude *= persistence;
        frequency *= lacunarity;
    }

    return value / maxValue;
}

// ─── Color Palette ────────────────────────────────────────────────

function hexToRgb(hex) {
    const r = parseInt(hex.slice(1, 3), 16);
    const g = parseInt(hex.slice(3, 5), 16);
    const b = parseInt(hex.slice(5, 7), 16);
    return { r, g, b };
}

/**
 * Build a 256-entry color lookup table from an array of color stops.
 * Maps noise value [0, 1] → RGB via linear interpolation between stops.
 */
function buildPaletteLUT(colorStops) {
    const lut = new Uint8Array(256 * 3);
    const numStops = colorStops.length;

    if (numStops === 0) return lut;

    const rgbs = colorStops.map(hexToRgb);

    for (let i = 0; i < 256; i++) {
        const t = i / 255;
        // Position along the gradient [0, numStops-1]
        const pos = t * (numStops - 1);
        const idx = Math.min(Math.floor(pos), numStops - 2);
        const frac = pos - idx;

        const c0 = rgbs[idx];
        const c1 = rgbs[idx + 1];

        lut[i * 3]     = Math.round(c0.r + (c1.r - c0.r) * frac);
        lut[i * 3 + 1] = Math.round(c0.g + (c1.g - c0.g) * frac);
        lut[i * 3 + 2] = Math.round(c0.b + (c1.b - c0.b) * frac);
    }

    return lut;
}

// ─── Canvas Sizing ────────────────────────────────────────────────

function getCanvasSize(canvas) {
    const rect = canvas.parentElement.getBoundingClientRect();
    return {
        width: Math.max(1, Math.floor(rect.width)),
        height: Math.max(1, Math.floor(rect.height))
    };
}

// ─── Init ─────────────────────────────────────────────────────────

export function init(canvas, rawConfig) {
    const config = normalizeConfig(rawConfig);
    const { width, height } = getCanvasSize(canvas);

    // Compute at 0.5x resolution for smooth, organic visuals
    // (0.25x was too aggressive, causing muddy/pixelated output)
    const scale = 0.5;
    const noiseW = Math.max(1, Math.floor(width * scale));
    const noiseH = Math.max(1, Math.floor(height * scale));

    canvas.width = noiseW;
    canvas.height = noiseH;

    // Enable bilinear smoothing for upscale
    const ctx = canvas.getContext('2d');
    ctx.imageSmoothingEnabled = true;
    ctx.imageSmoothingQuality = 'high';

    // Pre-compute palette LUT
    const paletteLUT = buildPaletteLUT(config.colorStops);

    // Off-screen buffer for pixel manipulation
    const imageData = ctx.createImageData(noiseW, noiseH);
    const pixels = imageData.data;

    const permData = buildPerm(42);

    const state = {
        canvas,
        ctx,
        config,
        paletteLUT,
        imageData,
        pixels,
        permData,
        noiseW,
        noiseH,
        scale,
        displayWidth: width,
        displayHeight: height,
        running: true,
        animFrameId: null,
        lastFrameTime: 0,
        lastTimestamp: 0,
        time: 0,
        observer: null,
        resizeHandler: null
    };

    const id = String(nextId++);
    instances.set(id, state);

    // Start animation loop
    startLoop(id);

    // IntersectionObserver for off-screen pause
    if ('IntersectionObserver' in window) {
        state.observer = new IntersectionObserver(
            (entries) => {
                for (const entry of entries) {
                    if (entry.isIntersecting && !state.running) {
                        state.running = true;
                        startLoop(id);
                    } else if (!entry.isIntersecting && state.running) {
                        state.running = false;
                        if (state.animFrameId) {
                            cancelAnimationFrame(state.animFrameId);
                            state.animFrameId = null;
                        }
                    }
                }
            },
            { threshold: 0 }
        );
        state.observer.observe(canvas);
    }

    // Debounced resize
    let resizeTimer = null;
    state.resizeHandler = () => {
        if (resizeTimer) clearTimeout(resizeTimer);
        resizeTimer = setTimeout(() => {
            const { width: newW, height: newH } = getCanvasSize(canvas);
            const nW = Math.max(1, Math.floor(newW * scale));
            const nH = Math.max(1, Math.floor(newH * scale));
            canvas.width = nW;
            canvas.height = nH;
            state.noiseW = nW;
            state.noiseH = nH;
            state.displayWidth = newW;
            state.displayHeight = newH;
            state.imageData = state.ctx.createImageData(nW, nH);
            state.pixels = state.imageData.data;
        }, 100);
    };
    window.addEventListener('resize', state.resizeHandler);

    return id;
}

// ─── Update ───────────────────────────────────────────────────────

export function update(instanceId, rawConfig) {
    const state = instances.get(instanceId);
    if (!state) return;

    state.config = normalizeConfig(rawConfig);
    state.paletteLUT = buildPaletteLUT(state.config.colorStops);
}

// ─── Dispose ──────────────────────────────────────────────────────

export function dispose(instanceId) {
    const state = instances.get(instanceId);
    if (!state) return;

    state.running = false;
    if (state.animFrameId) {
        cancelAnimationFrame(state.animFrameId);
        state.animFrameId = null;
    }
    if (state.observer) {
        state.observer.disconnect();
        state.observer = null;
    }
    if (state.resizeHandler) {
        window.removeEventListener('resize', state.resizeHandler);
        state.resizeHandler = null;
    }

    // Clear canvas and release pixel buffer
    state.ctx.clearRect(0, 0, state.canvas.width, state.canvas.height);
    state.pixels = null;
    state.imageData = null;
    instances.delete(instanceId);
}

// ─── Animation Loop ──────────────────────────────────────────────

function startLoop(id) {
    const state = instances.get(id);
    if (!state || !state.running) return;

    const loop = (timestamp) => {
        if (!state.running) return;

        const interval = 1000 / state.config.targetFps;
        if (timestamp - state.lastFrameTime < interval) {
            state.animFrameId = requestAnimationFrame(loop);
            return;
        }

        const dt = state.lastTimestamp ? (timestamp - state.lastTimestamp) / 1000 : 1 / 60;
        state.lastTimestamp = timestamp;
        state.lastFrameTime = timestamp;
        state.time += dt;

        drawFrame(state);

        state.animFrameId = requestAnimationFrame(loop);
    };

    state.animFrameId = requestAnimationFrame(loop);
}

function drawFrame(state) {
    const { pixels, paletteLUT, permData, noiseW, noiseH, config, time, displayWidth, displayHeight } = state;
    const { noiseScale, speed, octaves, persistence, lacunarity, brightness } = config;

    // Time drives the z-axis evolution of 3D noise
    const z = time * speed * 100;
    // Pre-compute brightness as a 0-255 scale factor
    const bScale = Math.max(0, Math.min(1, brightness));

    // We compute noise coordinates based on the DISPLAY (parent) size,
    // not the downscaled buffer size, so the noise pattern is
    // resolution-independent and looks the same at any scale factor.
    const dW = displayWidth || noiseW / (state.scale || 0.5);
    const dH = displayHeight || noiseH / (state.scale || 0.5);

    for (let y = 0; y < noiseH; y++) {
        for (let x = 0; x < noiseW; x++) {
            // Map buffer pixel → display pixel for consistent noise field
            const dx = (x / noiseW) * dW;
            const dy = (y / noiseH) * dH;

            const nx = dx * noiseScale;
            const ny = dy * noiseScale;

            // Domain warping: use noise to distort the noise coordinates
            // This creates flowing, organic, lava-lamp-like motion
            const warpStrength = 0.6;
            const warpFreq = noiseScale * 1.5;
            const warpX = fbm(permData, dx * warpFreq, dy * warpFreq, z * 0.7, Math.max(1, octaves - 1), persistence, lacunarity);
            const warpY = fbm(permData, dx * warpFreq + 5.2, dy * warpFreq + 1.3, z * 0.7, Math.max(1, octaves - 1), persistence, lacunarity);

            // Fractal Brownian Motion with warped coordinates
            const noiseVal = fbm(
                permData,
                nx + warpX * warpStrength,
                ny + warpY * warpStrength,
                z,
                octaves,
                persistence,
                lacunarity
            );

            // Map from [-1, 1] to [0, 1]
            const normalized = (noiseVal + 1) * 0.5;

            // Smooth contrast curve for richer gradients
            // Smoothstep: 3t² - 2t³ — pushes values away from midgray
            const raw = Math.max(0, Math.min(1, normalized));
            const t = raw * raw * (3 - 2 * raw);

            // Palette lookup
            const lutIdx = Math.min(255, Math.floor(t * 255)) * 3;

            const pixIdx = (y * noiseW + x) * 4;
            pixels[pixIdx]     = Math.min(255, Math.round(paletteLUT[lutIdx]     * bScale));
            pixels[pixIdx + 1] = Math.min(255, Math.round(paletteLUT[lutIdx + 1] * bScale));
            pixels[pixIdx + 2] = Math.min(255, Math.round(paletteLUT[lutIdx + 2] * bScale));
            pixels[pixIdx + 3] = 255;
        }
    }

    state.ctx.putImageData(state.imageData, 0, 0);
}

// ─── Config Normalization ────────────────────────────────────────

function normalizeConfig(raw) {
    // Richer default palette with more stops for smoother gradient transitions
    const defaultStops = ["#0a0a2e", "#1e1b4b", "#6366f1", "#8b5cf6", "#c084fc", "#ec4899", "#f43f5e", "#1e1b4b", "#0a0a2e"];

    let colorStops = defaultStops;
    if (raw?.colorStops && Array.isArray(raw.colorStops) && raw.colorStops.length >= 2) {
        colorStops = raw.colorStops.map(c => String(c));
    }

    return {
        colorStops,
        noiseScale: Math.max(0.0001, Number(raw?.noiseScale) || 0.003),
        speed: Math.max(0, Number(raw?.speed) || 0.005),
        octaves: Math.max(1, Math.min(8, Number(raw?.octaves) || 4)),
        persistence: Math.max(0, Math.min(1, Number(raw?.persistence) || 0.5)),
        lacunarity: Math.max(1, Number(raw?.lacunarity) || 2.0),
        brightness: Math.max(0, Math.min(3, Number(raw?.brightness) || 1.0)),
        opacity: Math.max(0, Math.min(1, Number(raw?.opacity) || 0.85)),
        targetFps: Math.max(1, Math.min(120, Number(raw?.targetFps) || 60))
    };
}
