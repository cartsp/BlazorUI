/**
 * BlazorEffects.VortexTunnel - Vortex/Tunnel spiraling animated background effect
 *
 * Exports: init, update, dispose
 *
 * Rendering technique: Canvas 2D with rotating concentric shapes that scale
 * toward the viewer. Each ring has a rotation offset that increases with depth,
 * creating a spiraling tunnel effect. Uses ctx.save()/restore() with transforms.
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
    const rect = canvas.parentElement.getBoundingClientRect();
    return {
        width: Math.max(1, Math.floor(rect.width)),
        height: Math.max(1, Math.floor(rect.height))
    };
}

/**
 * Get the color for a ring at the given index, supporting multi-color arrays.
 */
function getRingColor(config, index) {
    if (config.colors && config.colors.length > 0) {
        return config.colors[index % config.colors.length];
    }
    return config.color;
}

/**
 * Draw a polygon path (regular polygon with n sides).
 */
function drawPolygon(ctx, x, y, radius, sides, rotation) {
    ctx.beginPath();
    for (let i = 0; i <= sides; i++) {
        const angle = (i * 2 * Math.PI / sides) + rotation;
        const px = x + Math.max(0.1, radius) * Math.cos(angle);
        const py = y + Math.max(0.1, radius) * Math.sin(angle);
        if (i === 0) {
            ctx.moveTo(px, py);
        } else {
            ctx.lineTo(px, py);
        }
    }
    ctx.closePath();
}

/**
 * Draw a square path with rotation.
 */
function drawSquare(ctx, x, y, size, rotation) {
    const halfSize = Math.max(0.1, size) / 2;
    ctx.save();
    ctx.translate(x, y);
    ctx.rotate(rotation);
    ctx.beginPath();
    ctx.rect(-halfSize, -halfSize, halfSize * 2, halfSize * 2);
    ctx.restore();
}

/**
 * Draw the full tunnel frame.
 */
function drawFrame(ctx, config, time, canvasWidth, canvasHeight) {
    const centerX = canvasWidth / 2;
    const centerY = canvasHeight / 2;
    const maxRadius = Math.max(canvasWidth, canvasHeight) * 0.7;

    // Clear with background
    ctx.fillStyle = config.backgroundColor;
    ctx.fillRect(0, 0, canvasWidth, canvasHeight);

    // Draw rings from outer to inner
    for (let i = config.ringCount - 1; i >= 0; i--) {
        const t = i / config.ringCount;
        const radius = maxRadius * Math.pow(config.scaleFactor, i);
        const rotation = time * config.rotationSpeed * (1 + t * 2);
        const alpha = Math.max(0.1, (1 - t * 0.7)) * config.opacity;
        const lw = Math.max(0.3, config.lineWidth * (1 - t * 0.6));

        const ringColor = getRingColor(config, i);
        const rgb = hexToRgb(ringColor);

        ctx.save();
        ctx.globalAlpha = alpha;
        ctx.strokeStyle = `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, ${alpha})`;
        ctx.lineWidth = lw;

        switch (config.shape) {
            case 'square':
                drawSquare(ctx, centerX, centerY, radius * 2, rotation);
                break;
            case 'polygon':
                drawPolygon(ctx, centerX, centerY, radius, config.polygonSides, rotation);
                break;
            case 'circle':
            default:
                ctx.beginPath();
                ctx.arc(centerX, centerY, Math.max(0.1, radius), 0, Math.PI * 2);
                break;
        }

        ctx.stroke();
        ctx.restore();
    }

    // Optional: draw a bright center point
    const centerAlpha = 0.3 * config.opacity;
    const centerRgb = hexToRgb(getRingColor(config, 0));
    ctx.beginPath();
    ctx.arc(centerX, centerY, 3, 0, Math.PI * 2);
    ctx.fillStyle = `rgba(${centerRgb.r}, ${centerRgb.g}, ${centerRgb.b}, ${centerAlpha})`;
    ctx.fill();
}

// ─── Init ─────────────────────────────────────────────────────────

export function init(canvas, rawConfig) {
    const config = normalizeConfig(rawConfig);
    const { width, height } = getCanvasSize(canvas);

    canvas.width = Math.max(1, width);
    canvas.height = Math.max(1, height);

    const ctx = canvas.getContext('2d');

    const state = {
        canvas,
        ctx,
        config,
        time: 0,
        running: true,
        animFrameId: null,
        lastFrameTime: 0,
        observer: null,
        resizeHandler: null
    };

    const id = String(nextId++);
    instances.set(id, state);

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
            canvas.width = Math.max(1, newW);
            canvas.height = Math.max(1, newH);
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

        state.time++;
        const { width, height } = getCanvasSize(state.canvas);

        drawFrame(state.ctx, state.config, state.time, width, height);

        state.animFrameId = requestAnimationFrame(loop);
    };

    state.animFrameId = requestAnimationFrame(loop);
}

// ─── Config Normalization ────────────────────────────────────────

function normalizeConfig(raw) {
    return {
        ringCount: Math.max(5, Math.min(30, Number(raw?.ringCount) || 20)),
        rotationSpeed: Math.max(0.005, Number(raw?.rotationSpeed) || 0.02),
        color: String(raw?.color || '#8b5cf6'),
        colors: Array.isArray(raw?.colors) ? raw.colors : [],
        scaleFactor: Math.max(0.75, Math.min(0.98, Number(raw?.scaleFactor) || 0.92)),
        shape: (['circle', 'polygon', 'square'].includes(raw?.shape)) ? raw.shape : 'circle',
        polygonSides: Math.max(3, Math.min(12, Number(raw?.polygonSides) || 6)),
        lineWidth: Math.max(0.5, Number(raw?.lineWidth) || 2),
        backgroundColor: String(raw?.backgroundColor || '#030712'),
        opacity: Math.max(0, Math.min(1.0, Number(raw?.opacity) || 1.0)),
        targetFps: Math.max(1, Math.min(120, Number(raw?.targetFps) || 60))
    };
}
