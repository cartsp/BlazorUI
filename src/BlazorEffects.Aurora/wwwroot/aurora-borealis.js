/**
 * BlazorEffects.Aurora - Aurora Borealis (Northern Lights) animated background effect
 *
 * Exports: init, update, dispose (standard EffectComponentBase contract)
 *
 * Rendering technique: multiple translucent gradient ribbons driven by simplex noise,
 * composited with screen blending for realistic light mixing.
 * Uses 0.25x resolution scaling (decorative background) and layered alpha compositing.
 */

// ─── Instance Management ────────────────────────────────────────

const instances = new Map();
let nextId = 0;

// ─── Simplex Noise (2D) ─────────────────────────────────────────

// Permutation table for noise generation
const perm = new Uint8Array(512);
const grad3 = [
    [1, 1, 0], [-1, 1, 0], [1, -1, 0], [-1, -1, 0],
    [1, 0, 1], [-1, 0, 1], [1, 0, -1], [-1, 0, -1],
    [0, 1, 1], [0, -1, 1], [0, 1, -1], [0, -1, -1]
];

function initNoise(seed) {
    const p = new Uint8Array(256);
    for (let i = 0; i < 256; i++) p[i] = i;
    // Seeded shuffle
    let s = seed || 42;
    for (let i = 255; i > 0; i--) {
        s = (s * 16807 + 0) % 2147483647;
        const j = s % (i + 1);
        [p[i], p[j]] = [p[j], p[i]];
    }
    for (let i = 0; i < 512; i++) perm[i] = p[i & 255];
}

function dot2(g, x, y) { return g[0] * x + g[1] * y; }

const F2 = 0.5 * (Math.sqrt(3) - 1);
const G2 = (3 - Math.sqrt(3)) / 6;

function noise2D(xin, yin) {
    const s = (xin + yin) * F2;
    const i = Math.floor(xin + s);
    const j = Math.floor(yin + s);
    const t = (i + j) * G2;
    const X0 = i - t;
    const Y0 = j - t;
    const x0 = xin - X0;
    const y0 = yin - Y0;

    let i1, j1;
    if (x0 > y0) { i1 = 1; j1 = 0; }
    else { i1 = 0; j1 = 1; }

    const x1 = x0 - i1 + G2;
    const y1 = y0 - j1 + G2;
    const x2 = x0 - 1.0 + 2.0 * G2;
    const y2 = y0 - 1.0 + 2.0 * G2;

    const ii = i & 255;
    const jj = j & 255;
    const gi0 = perm[ii + perm[jj]] % 12;
    const gi1 = perm[ii + i1 + perm[jj + j1]] % 12;
    const gi2 = perm[ii + 1 + perm[jj + 1]] % 12;

    let n0 = 0, n1 = 0, n2 = 0;

    let t0 = 0.5 - x0 * x0 - y0 * y0;
    if (t0 >= 0) { t0 *= t0; n0 = t0 * t0 * dot2(grad3[gi0], x0, y0); }

    let t1 = 0.5 - x1 * x1 - y1 * y1;
    if (t1 >= 0) { t1 *= t1; n1 = t1 * t1 * dot2(grad3[gi1], x1, y1); }

    let t2 = 0.5 - x2 * x2 - y2 * y2;
    if (t2 >= 0) { t2 *= t2; n2 = t2 * t2 * dot2(grad3[gi2], x2, y2); }

    return 70.0 * (n0 + n1 + n2); // Returns -1..1
}

// Fractal Brownian Motion for richer noise
function fbm(x, y, octaves) {
    let value = 0;
    let amplitude = 1;
    let frequency = 1;
    let maxValue = 0;
    for (let i = 0; i < octaves; i++) {
        value += amplitude * noise2D(x * frequency, y * frequency);
        maxValue += amplitude;
        amplitude *= 0.5;
        frequency *= 2.0;
    }
    return value / maxValue;
}

// ─── Color Helpers ───────────────────────────────────────────────

function hexToRgb(hex) {
    const result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    if (!result) return { r: 0, g: 255, b: 135 };
    return {
        r: parseInt(result[1], 16),
        g: parseInt(result[2], 16),
        b: parseInt(result[3], 16)
    };
}

// ─── Canvas Helpers ──────────────────────────────────────────────

function getCanvasSize(canvas) {
    const rect = canvas.parentElement.getBoundingClientRect();
    return {
        width: Math.max(1, Math.floor(rect.width)),
        height: Math.max(1, Math.floor(rect.height))
    };
}

// ─── Reduced Motion ─────────────────────────────────────────────

function applyReducedMotion(config) {
    const behavior = config.reducedMotionBehavior || "Minimal";
    if (behavior === "Ignore") return config;
    const prefersReduced = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
    if (!prefersReduced) return config;
    if (behavior === "Pause") {
        config.speed = 0;
        config.targetFps = 1;
    } else {
        config.speed = Math.max(0.1, (config.speed || 1) * 0.1);
        config.targetFps = Math.min(config.targetFps, 12);
    }
    return config;
}

// ─── Ribbon Construction ────────────────────────────────────────

function buildRibbons(config, canvasWidth, canvasHeight) {
    const ribbons = [];
    const count = Math.max(1, config.ribbonCount);
    const colors = config.colors && config.colors.length > 0
        ? config.colors
        : ['#00ff87', '#7b2ff7', '#00b4d8'];

    for (let i = 0; i < count; i++) {
        const color = colors[i % colors.length];
        const rgb = hexToRgb(color);

        ribbons.push({
            color: rgb,
            colorHex: color,
            // Noise offset so each ribbon is unique
            noiseOffsetX: i * 73.17 + 10,
            noiseOffsetY: i * 31.43 + 5,
            // Each ribbon has slightly different parameters
            amplitudeMultiplier: 0.6 + (i / count) * 0.8,
            speedMultiplier: 0.7 + (i / count) * 0.6,
            // Vertical center position (spread across canvas)
            baseY: canvasHeight * (0.2 + (i / count) * 0.5),
            // Ribbon thickness
            thickness: canvasHeight * (0.08 + Math.random() * 0.06),
            // Alpha per ribbon
            alpha: 0.3 + (1 - i / count) * 0.4
        });
    }

    return ribbons;
}

// ─── Init ────────────────────────────────────────────────────────

export function init(canvas, rawConfig) {
    const config = normalizeConfig(rawConfig);
    const { width, height } = getCanvasSize(canvas);

    // 0.25x resolution scaling (decorative background)
    const scale = 0.25;
    canvas.width = Math.max(1, Math.floor(width * scale));
    canvas.height = Math.max(1, Math.floor(height * scale));

    const ctx = canvas.getContext('2d');

    // Initialize noise with a random seed for variety
    initNoise(Math.floor(Math.random() * 65536));

    const ribbons = buildRibbons(config, width, height);

    const state = {
        canvas,
        ctx,
        config,
        ribbons,
        running: true,
        animFrameId: null,
        lastFrameTime: 0,
        time: 0,
        observer: null,
        resizeHandler: null,
        scale,
        displayWidth: width,
        displayHeight: height
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
            canvas.width = Math.max(1, Math.floor(newW * scale));
            canvas.height = Math.max(1, Math.floor(newH * scale));
            state.displayWidth = newW;
            state.displayHeight = newH;
            state.ribbons = buildRibbons(state.config, newW, newH);
        }, 100);
    };
    window.addEventListener('resize', state.resizeHandler);

    return id;
}

// ─── Update ──────────────────────────────────────────────────────

export function update(instanceId, rawConfig) {
    const state = instances.get(instanceId);
    if (!state) return;

    state.config = normalizeConfig(rawConfig);
    state.ribbons = buildRibbons(state.config, state.displayWidth, state.displayHeight);
}

// ─── Dispose ─────────────────────────────────────────────────────

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

    state.ctx.clearRect(0, 0, state.canvas.width, state.canvas.height);
    instances.delete(instanceId);
}

// ─── Animation Loop ─────────────────────────────────────────────

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
        state.lastFrameTime = timestamp;

        state.time += state.config.speed;
        drawFrame(state);

        state.animFrameId = requestAnimationFrame(loop);
    };

    state.animFrameId = requestAnimationFrame(loop);
}

// ─── Frame Drawing ───────────────────────────────────────────────

function drawFrame(state) {
    const { ctx, canvas, ribbons, time, scale } = state;
    const w = canvas.width;  // Already scaled (low-res)
    const h = canvas.height;

    // Clear to black
    ctx.globalCompositeOperation = 'source-over';
    ctx.fillStyle = '#000000';
    ctx.fillRect(0, 0, w, h);

    // Use screen blending for light mixing
    ctx.globalCompositeOperation = 'screen';

    for (const ribbon of ribbons) {
        drawRibbon(ctx, ribbon, time, w, h, scale);
    }

    // Reset composite operation
    ctx.globalCompositeOperation = 'source-over';
}

function drawRibbon(ctx, ribbon, time, canvasW, canvasH, scale) {
    const { color, noiseOffsetX, noiseOffsetY, amplitudeMultiplier, speedMultiplier, alpha } = ribbon;

    // Ribbon vertical center in scaled coords
    const baseYScaled = ribbon.baseY * scale;
    const thicknessScaled = ribbon.thickness * scale;
    const ampScaled = ribbon.amplitudeMultiplier * 150 * scale;

    // Number of horizontal segments
    const segments = Math.max(8, Math.floor(canvasW / 4));

    // Build top and bottom edges of the ribbon
    const topPoints = [];
    const bottomPoints = [];

    for (let i = 0; i <= segments; i++) {
        const x = (i / segments) * canvasW;
        const xNorm = i / segments;

        // Multi-octave noise for organic motion
        const n = fbm(
            xNorm * 3.0 + noiseOffsetX + time * speedMultiplier * 2.5,
            noiseOffsetY + time * speedMultiplier * 0.8,
            3
        );

        const yOffset = n * ampScaled;

        // Ribbon width varies with a second noise channel
        const widthNoise = fbm(
            xNorm * 2.0 + noiseOffsetX * 0.5 + time * speedMultiplier * 1.2 + 100,
            noiseOffsetY * 1.3 + time * speedMultiplier * 0.4 + 50,
            2
        );
        const halfWidth = Math.max(1, thicknessScaled * (0.3 + (widthNoise + 1) * 0.5));

        topPoints.push({ x, y: baseYScaled + yOffset - halfWidth });
        bottomPoints.push({ x, y: baseYScaled + yOffset + halfWidth });
    }

    // Draw the ribbon shape
    ctx.beginPath();
    ctx.moveTo(topPoints[0].x, topPoints[0].y);

    // Top edge (left to right) - smooth curve
    for (let i = 1; i < topPoints.length; i++) {
        const prev = topPoints[i - 1];
        const curr = topPoints[i];
        const cpx = (prev.x + curr.x) / 2;
        ctx.quadraticCurveTo(prev.x, prev.y, cpx, (prev.y + curr.y) / 2);
    }
    ctx.lineTo(topPoints[topPoints.length - 1].x, topPoints[topPoints.length - 1].y);

    // Bottom edge (right to left) - smooth curve
    for (let i = bottomPoints.length - 1; i >= 1; i--) {
        const prev = bottomPoints[i];
        const curr = bottomPoints[i - 1];
        const cpx = (prev.x + curr.x) / 2;
        ctx.quadraticCurveTo(prev.x, prev.y, cpx, (prev.y + curr.y) / 2);
    }
    ctx.lineTo(bottomPoints[0].x, bottomPoints[0].y);
    ctx.closePath();

    // Create gradient along the ribbon for a glow effect
    const grad = ctx.createLinearGradient(0, baseYScaled - thicknessScaled * 2, 0, baseYScaled + thicknessScaled * 2);
    grad.addColorStop(0, `rgba(${color.r}, ${color.g}, ${color.b}, 0)`);
    grad.addColorStop(0.3, `rgba(${color.r}, ${color.g}, ${color.b}, ${alpha * 0.7})`);
    grad.addColorStop(0.5, `rgba(${color.r}, ${color.g}, ${color.b}, ${alpha})`);
    grad.addColorStop(0.7, `rgba(${color.r}, ${color.g}, ${color.b}, ${alpha * 0.7})`);
    grad.addColorStop(1, `rgba(${color.r}, ${color.g}, ${color.b}, 0)`);

    ctx.fillStyle = grad;
    ctx.fill();
}

// ─── Config Normalization ───────────────────────────────────────

function normalizeConfig(raw) {
    const colors = Array.isArray(raw?.colors) && raw.colors.length > 0
        ? raw.colors.map(c => String(c))
        : ['#00ff87', '#7b2ff7', '#00b4d8'];

    const config = {
        colors,
        ribbonCount: Math.max(1, Math.min(10, Number(raw?.ribbonCount) || 4)),
        amplitude: Math.max(10, Number(raw?.amplitude) || 120),
        speed: Math.max(0.001, Number(raw?.speed) || 0.008),
        opacity: Math.max(0, Math.min(1, Number(raw?.opacity) || 0.5)),
        blendMode: String(raw?.blendMode || 'screen'),
        targetFps: Math.max(1, Math.min(120, Number(raw?.targetFps) || 60))
    };
    return applyReducedMotion(config);
}
