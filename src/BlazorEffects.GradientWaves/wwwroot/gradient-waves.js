/**
 * BlazorEffects.GradientWaves - Gradient Waves (Mesh Gradient) animated background effect
 *
 * Exports: init, update, dispose (standard EffectComponentBase contract)
 *
 * Rendering technique: Color control points rendered as filled circles on an offscreen
 * canvas, composited via CSS filter blur for smooth, continuous gradient blending.
 * This is the "Stripe mesh gradient" technique — blurred color blobs create a premium
 * mesh gradient effect with minimal computational cost.
 */

// Instance management
const instances = new Map();
let nextId = 0;

// ─── Helpers ──────────────────────────────────────────────────────

function hexToRgb(hex) {
    const r = parseInt(hex.slice(1, 3), 16);
    const g = parseInt(hex.slice(3, 5), 16);
    const b = parseInt(hex.slice(5, 7), 16);
    return { r, g, b };
}

function getCanvasSize(canvas) {
    const parent = canvas.parentElement;
    if (!parent) return { width: 1, height: 1 };
    const rect = parent.getBoundingClientRect();
    return {
        width: Math.max(1, Math.floor(rect.width)),
        height: Math.max(1, Math.floor(rect.height))
    };
}

/**
 * Create a drifting color control point with organic motion.
 * Each point has a center it orbits around, with noise-like wander.
 */
function createColorPoint(index, count, canvasWidth, canvasHeight, config) {
    // Distribute points in a relaxed grid-like pattern with jitter
    const cols = Math.ceil(Math.sqrt(count));
    const rows = Math.ceil(count / cols);
    const col = index % cols;
    const row = Math.floor(index / cols);

    const cellW = canvasWidth / cols;
    const cellH = canvasHeight / rows;

    // Center of cell with random jitter
    const cx = cellW * (col + 0.5) + (Math.random() - 0.5) * cellW * 0.4;
    const cy = cellH * (row + 0.5) + (Math.random() - 0.5) * cellH * 0.4;

    // Drift parameters — each point has unique phase and speed
    const phase = Math.random() * Math.PI * 2;
    const driftSpeed = 0.3 + Math.random() * 0.7;
    const driftRadius = Math.min(canvasWidth, canvasHeight) * (0.1 + Math.random() * 0.15);

    // Secondary drift axis for more organic motion
    const phase2 = Math.random() * Math.PI * 2;
    const driftSpeed2 = 0.2 + Math.random() * 0.5;
    const driftRadius2 = Math.min(canvasWidth, canvasHeight) * (0.05 + Math.random() * 0.1);

    return {
        baseX: cx,
        baseY: cy,
        phase,
        driftSpeed,
        driftRadius,
        phase2,
        driftSpeed2,
        driftRadius2,
        color: config.colors[index % config.colors.length]
    };
}

// ─── Init ─────────────────────────────────────────────────────────

export function init(canvas, rawConfig) {
    const config = normalizeConfig(rawConfig);

    // Respect prefers-reduced-motion: freeze animation, render a single static frame
    const prefersReducedMotion = window.matchMedia?.('(prefers-reduced-motion: reduce)')?.matches ?? false;
    if (prefersReducedMotion) {
        config.speed = 0;
        config.targetFps = 1;
    }

    const { width, height } = getCanvasSize(canvas);

    // Use a moderate resolution — the blur makes pixel density less critical
    const scale = 0.5;
    canvas.width = Math.max(1, Math.floor(width * scale));
    canvas.height = Math.max(1, Math.floor(height * scale));

    const ctx = canvas.getContext('2d');

    // Create color control points
    const points = [];
    for (let i = 0; i < config.pointCount; i++) {
        points.push(createColorPoint(i, config.pointCount, width, height, config));
    }

    const state = {
        canvas,
        ctx,
        config,
        points,
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
            // Rebuild points for new dimensions
            state.points = [];
            for (let i = 0; i < state.config.pointCount; i++) {
                state.points.push(createColorPoint(i, state.config.pointCount, newW, newH, state.config));
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

    // Rebuild points with new config
    state.points = [];
    for (let i = 0; i < state.config.pointCount; i++) {
        state.points.push(createColorPoint(i, state.config.pointCount, state.displayWidth, state.displayHeight, state.config));
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
        state.lastFrameTime = timestamp;

        state.time += state.config.speed;
        drawFrame(state);

        state.animFrameId = requestAnimationFrame(loop);
    };

    state.animFrameId = requestAnimationFrame(loop);
}

function drawFrame(state) {
    const { ctx, canvas, points, time, scale, config } = state;
    const w = canvas.width;
    const h = canvas.height;
    const displayW = state.displayWidth;
    const displayH = state.displayHeight;

    // Clear canvas
    ctx.globalCompositeOperation = 'source-over';
    ctx.clearRect(0, 0, w, h);

    // Apply blur filter for the mesh gradient effect
    // Scale blur by the same factor as the canvas for consistent visual
    const scaledBlur = Math.max(1, config.blurAmount * scale);
    ctx.filter = `blur(${scaledBlur}px)`;

    // Calculate blob radius based on diagonal and config
    const diagonal = Math.sqrt(displayW * displayW + displayH * displayH);
    const blobRadius = Math.max(10, diagonal * config.blobSize * 0.25 * scale);

    // Draw each color control point as a filled circle
    ctx.globalCompositeOperation = config.blendMode === 'screen' ? 'screen' : 'source-over';

    for (const point of points) {
        // Calculate current position based on Lissajous-like drift
        const x = point.baseX * scale + Math.sin(time * point.driftSpeed * 250 + point.phase) * point.driftRadius * scale;
        const y = point.baseY * scale + Math.cos(time * point.driftSpeed2 * 250 + point.phase2) * point.driftRadius2 * scale
            + Math.sin(time * point.driftSpeed * 180 + point.phase + 1.5) * point.driftRadius * scale * 0.5;

        const rgb = hexToRgb(point.color);

        // Radial gradient from solid center to transparent edge
        const gradient = ctx.createRadialGradient(x, y, 0, x, y, blobRadius);
        gradient.addColorStop(0, `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, 0.85)`);
        gradient.addColorStop(0.4, `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, 0.6)`);
        gradient.addColorStop(0.7, `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, 0.3)`);
        gradient.addColorStop(1, `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, 0)`);

        ctx.fillStyle = gradient;
        ctx.beginPath();
        ctx.arc(x, y, blobRadius, 0, Math.PI * 2);
        ctx.fill();
    }

    // Reset filter and composite operation
    ctx.filter = 'none';
    ctx.globalCompositeOperation = 'source-over';
}

// ─── Config Normalization ────────────────────────────────────────

function normalizeConfig(raw) {
    const defaultColors = ['#635bff', '#00d4ff', '#ff6b9d', '#a855f7', '#06b6d4', '#f472b6'];

    let colors = defaultColors;
    if (raw?.colors && Array.isArray(raw.colors) && raw.colors.length >= 2) {
        colors = raw.colors.map(c => String(c));
    }

    return {
        colors,
        pointCount: Math.max(2, Math.min(12, Number(raw?.pointCount) || 6)),
        blobSize: Math.max(0.1, Math.min(1.0, Number(raw?.blobSize) || 0.5)),
        speed: Math.max(0.001, Number(raw?.speed) || 0.004),
        blurAmount: Math.max(10, Math.min(200, Number(raw?.blurAmount) || 80)),
        blendMode: String(raw?.blendMode || 'normal'),
        opacity: Math.max(0, Math.min(1, Number(raw?.opacity) || 1.0)),
        targetFps: Math.max(1, Math.min(120, Number(raw?.targetFps) || 60))
    };
}
