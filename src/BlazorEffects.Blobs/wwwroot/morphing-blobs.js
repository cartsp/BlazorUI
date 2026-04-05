/**
 * BlazorEffects.Blobs - Morphing Gradient Blobs animated background effect
 *
 * Exports: init, update, dispose (standard EffectComponentBase contract)
 *
 * Rendering technique: organic blobs drawn as closed bezier curves with
 * simplex-noise-perturbed control points, filled with radial gradients,
 * blended with globalCompositeOperation for luminous color mixing.
 */

// Instance management
const instances = new Map();
let nextId = 0;

// ─── Simplex Noise (2D) ──────────────────────────────────────────
// Minimal implementation for morphing blob shape perturbation.
// Based on Stefan Gustavson's simplified noise algorithm.

const F2 = 0.5 * (Math.sqrt(3.0) - 1.0);
const G2 = (3.0 - Math.sqrt(3.0)) / 6.0;

const grad3 = [
    [1, 1], [-1, 1], [1, -1], [-1, -1],
    [1, 0], [-1, 0], [0, 1], [0, -1]
];

// Build permutation table from a seed
function buildPerm(seed) {
    const p = new Uint8Array(256);
    for (let i = 0; i < 256; i++) p[i] = i;
    // Fisher-Yates shuffle with seed
    let s = seed;
    for (let i = 255; i > 0; i--) {
        s = (s * 16807 + 0) % 2147483647;
        const j = s % (i + 1);
        [p[i], p[j]] = [p[j], p[i]];
    }
    const perm = new Uint8Array(512);
    for (let i = 0; i < 512; i++) perm[i] = p[i & 255];
    return perm;
}

function simplex2D(perm, x, y) {
    const s = (x + y) * F2;
    const i = Math.floor(x + s);
    const j = Math.floor(y + s);
    const t = (i + j) * G2;
    const X0 = i - t;
    const Y0 = j - t;
    const x0 = x - X0;
    const y0 = y - Y0;

    let i1, j1;
    if (x0 > y0) { i1 = 1; j1 = 0; }
    else { i1 = 0; j1 = 1; }

    const x1 = x0 - i1 + G2;
    const y1 = y0 - j1 + G2;
    const x2 = x0 - 1.0 + 2.0 * G2;
    const y2 = y0 - 1.0 + 2.0 * G2;

    const ii = i & 255;
    const jj = j & 255;

    let n0 = 0, n1 = 0, n2 = 0;

    let t0 = 0.5 - x0 * x0 - y0 * y0;
    if (t0 >= 0) {
        const gi0 = perm[ii + perm[jj]] % 8;
        t0 *= t0;
        n0 = t0 * t0 * (grad3[gi0][0] * x0 + grad3[gi0][1] * y0);
    }

    let t1 = 0.5 - x1 * x1 - y1 * y1;
    if (t1 >= 0) {
        const gi1 = perm[ii + i1 + perm[jj + j1]] % 8;
        t1 *= t1;
        n1 = t1 * t1 * (grad3[gi1][0] * x1 + grad3[gi1][1] * y1);
    }

    let t2 = 0.5 - x2 * x2 - y2 * y2;
    if (t2 >= 0) {
        const gi2 = perm[ii + 1 + perm[jj + 1]] % 8;
        t2 *= t2;
        n2 = t2 * t2 * (grad3[gi2][0] * x2 + grad3[gi2][1] * y2);
    }

    return 70.0 * (n0 + n1 + n2);
}

// ─── Helpers ──────────────────────────────────────────────────────

function hexToRgb(hex) {
    const r = parseInt(hex.slice(1, 3), 16);
    const g = parseInt(hex.slice(3, 5), 16);
    const b = parseInt(hex.slice(5, 7), 16);
    return { r, g, b };
}

function getCanvasSize(canvas) {
    const rect = canvas.parentElement.getBoundingClientRect();
    return {
        width: Math.max(1, Math.floor(rect.width)),
        height: Math.max(1, Math.floor(rect.height))
    };
}

/**
 * Create a single blob object with its own state.
 */
function createBlob(index, config, canvasWidth, canvasHeight) {
    // Distribute blobs across the canvas with some randomness
    const angle = (index / config.blobCount) * Math.PI * 2;
    const spread = Math.min(canvasWidth, canvasHeight) * 0.25;
    const cx = canvasWidth / 2 + Math.cos(angle) * spread * (0.5 + Math.random() * 0.5);
    const cy = canvasHeight / 2 + Math.sin(angle) * spread * (0.5 + Math.random() * 0.5);

    // Drift velocity (slow, random direction)
    const driftAngle = Math.random() * Math.PI * 2;
    const driftSpeed = (0.2 + Math.random() * 0.3) * config.speed * 200;

    return {
        x: cx,
        y: cy,
        vx: Math.cos(driftAngle) * driftSpeed,
        vy: Math.sin(driftAngle) * driftSpeed,
        baseRadius: config.blobSize * (0.7 + Math.random() * 0.6),
        color: config.colors[index % config.colors.length],
        phaseOffset: index * 1.7, // unique noise phase per blob
        controlPoints: 8 + Math.floor(Math.random() * 4), // 8-11 control points
        noiseSeed: Math.floor(Math.random() * 10000),
        perm: buildPerm(Math.floor(Math.random() * 2147483647))
    };
}

/**
 * Draw a single morphing blob as a closed bezier curve.
 */
function drawBlob(ctx, blob, time, config) {
    const { x, y, baseRadius, controlPoints, phaseOffset, perm } = blob;
    const morphIntensity = config.morphIntensity;
    const noiseScale = 0.3;

    // Compute perturbed control points around the circle
    const points = [];
    for (let i = 0; i < controlPoints; i++) {
        const angle = (i / controlPoints) * Math.PI * 2;
        const nx = Math.cos(angle) * noiseScale + phaseOffset;
        const ny = Math.sin(angle) * noiseScale + time * config.speed * 100;

        // Simplex noise perturbation
        const noiseVal = simplex2D(perm, nx, ny);
        const radiusOffset = noiseVal * morphIntensity;
        const r = baseRadius + radiusOffset;

        points.push({
            x: x + Math.cos(angle) * r,
            y: y + Math.sin(angle) * r
        });
    }

    // Draw smooth closed bezier curve through the points
    ctx.beginPath();

    // Move to midpoint between last and first point
    const last = points[controlPoints - 1];
    const first = points[0];
    const startX = (last.x + first.x) / 2;
    const startY = (last.y + first.y) / 2;
    ctx.moveTo(startX, startY);

    // Draw quadratic bezier curves through midpoints
    for (let i = 0; i < controlPoints; i++) {
        const curr = points[i];
        const next = points[(i + 1) % controlPoints];
        const midX = (curr.x + next.x) / 2;
        const midY = (curr.y + next.y) / 2;
        ctx.quadraticCurveTo(curr.x, curr.y, midX, midY);
    }

    ctx.closePath();

    // Fill with radial gradient (solid center → transparent edge)
    const gradient = ctx.createRadialGradient(x, y, 0, x, y, baseRadius * 1.3);
    const rgb = hexToRgb(blob.color);
    gradient.addColorStop(0, `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, 0.8)`);
    gradient.addColorStop(0.4, `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, 0.5)`);
    gradient.addColorStop(0.7, `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, 0.2)`);
    gradient.addColorStop(1, `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, 0)`);

    ctx.fillStyle = gradient;
    ctx.fill();
}

/**
 * Update blob position with soft boundary bouncing.
 */
function updateBlobPosition(blob, canvasWidth, canvasHeight, dt) {
    blob.x += blob.vx * dt;
    blob.y += blob.vy * dt;

    // Soft boundary bounce with margin
    const margin = blob.baseRadius * 0.3;
    if (blob.x < -margin) { blob.vx = Math.abs(blob.vx); blob.x = -margin; }
    if (blob.x > canvasWidth + margin) { blob.vx = -Math.abs(blob.vx); blob.x = canvasWidth + margin; }
    if (blob.y < -margin) { blob.vy = Math.abs(blob.vy); blob.y = -margin; }
    if (blob.y > canvasHeight + margin) { blob.vy = -Math.abs(blob.vy); blob.y = canvasHeight + margin; }
}

// ─── Init ─────────────────────────────────────────────────────────

export function init(canvas, rawConfig) {
    const config = normalizeConfig(rawConfig);
    const { width, height } = getCanvasSize(canvas);

    // Use 0.5x resolution for performance (blobs are large and soft)
    const scale = 0.5;
    canvas.width = Math.max(1, Math.floor(width * scale));
    canvas.height = Math.max(1, Math.floor(height * scale));

    const ctx = canvas.getContext('2d');
    ctx.scale(scale, scale);

    // Create blob entities
    const blobs = [];
    for (let i = 0; i < config.blobCount; i++) {
        blobs.push(createBlob(i, config, width, height));
    }

    const state = {
        canvas,
        ctx,
        config,
        blobs,
        running: true,
        animFrameId: null,
        lastFrameTime: 0,
        lastTimestamp: 0,
        time: 0,
        observer: null,
        resizeHandler: null,
        scale
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
            state.ctx.scale(scale, scale);

            // Rebuild blobs with new canvas dimensions
            state.blobs = [];
            for (let i = 0; i < state.config.blobCount; i++) {
                state.blobs.push(createBlob(i, state.config, newW, newH));
            }
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

    // Rebuild blobs with new config
    const { width, height } = getCanvasSize(state.canvas);
    state.blobs = [];
    for (let i = 0; i < state.config.blobCount; i++) {
        state.blobs.push(createBlob(i, state.config, width, height));
    }
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

    // Clear canvas
    state.ctx.clearRect(0, 0, state.canvas.width, state.canvas.height);
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

        drawFrame(state, dt);

        state.animFrameId = requestAnimationFrame(loop);
    };

    state.animFrameId = requestAnimationFrame(loop);
}

function drawFrame(state, dt) {
    const { ctx, canvas, config, blobs, scale } = state;
    const displayWidth = canvas.width / scale;
    const displayHeight = canvas.height / scale;

    // Clear frame
    ctx.clearRect(0, 0, displayWidth, displayHeight);

    // Set blend mode for luminous color mixing
    ctx.globalCompositeOperation = config.blendMode;

    // Update and draw each blob
    for (const blob of blobs) {
        updateBlobPosition(blob, displayWidth, displayHeight, dt);
        drawBlob(ctx, blob, state.time, config);
    }

    // Reset composite operation
    ctx.globalCompositeOperation = 'source-over';
}

// ─── Config Normalization ────────────────────────────────────────

function normalizeConfig(raw) {
    const defaultColors = ['#6366f1', '#ec4899', '#f97316', '#06b6d4'];

    let colors = defaultColors;
    if (raw?.colors && Array.isArray(raw.colors) && raw.colors.length > 0) {
        colors = raw.colors.map(c => String(c));
    }

    return {
        blobCount: Math.max(1, Math.min(20, Number(raw?.blobCount) || 4)),
        colors: colors,
        blobSize: Math.max(50, Number(raw?.blobSize) || 300),
        speed: Math.max(0.001, Number(raw?.speed) || 0.005),
        morphIntensity: Math.max(0, Math.min(200, Number(raw?.morphIntensity) || 80)),
        blendMode: String(raw?.blendMode || 'screen'),
        opacity: Math.max(0, Math.min(1.0, Number(raw?.opacity) || 0.7)),
        targetFps: Math.max(1, Math.min(120, Number(raw?.targetFps) || 60))
    };
}
