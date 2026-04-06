/**
 * BlazorEffects.MatrixRain - Matrix Rain (falling characters) animated background effect
 *
 * Exports: init, update, dispose (standard EffectComponentBase contract)
 *
 * Rendering technique: classic column-based character cascade with trail fading
 * via semi-transparent background overlay each frame.
 */

// Instance management
const instances = new Map();
let nextId = 0;

// ─── Helpers ──────────────────────────────────────────────────────

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

function randomChar(chars) {
    return chars[Math.floor(Math.random() * chars.length)];
}

function getCanvasSize(canvas) {
    const rect = canvas.parentElement.getBoundingClientRect();
    return {
        width: Math.max(1, Math.floor(rect.width)),
        height: Math.max(1, Math.floor(rect.height))
    };
}

function buildColumns(config, canvasWidth) {
    const colCount = Math.max(1, Math.floor(canvasWidth / config.fontSize));
    const activeCount = Math.max(1, Math.floor(colCount * config.density));

    // Shuffle and pick active columns
    const indices = Array.from({ length: colCount }, (_, i) => i);
    for (let i = indices.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [indices[i], indices[j]] = [indices[j], indices[i]];
    }
    const activeSet = new Set(indices.slice(0, activeCount));

    const columns = [];
    for (let i = 0; i < colCount; i++) {
        if (activeSet.has(i)) {
            columns.push({
                x: i * config.fontSize,
                y: Math.random() * -canvasWidth * 0.5, // stagger start positions above viewport
                speed: (0.5 + Math.random() * 0.8) * config.speed,
                chars: [] // trail of characters
            });
        }
    }
    return columns;
}

// ─── Init ─────────────────────────────────────────────────────────

export function init(canvas, rawConfig) {
    const config = normalizeConfig(rawConfig);
    const { width, height } = getCanvasSize(canvas);

    // Use 0.5x resolution for performance (characters are meant to be slightly mysterious)
    const scale = 0.5;
    canvas.width = Math.max(1, Math.floor(width * scale));
    canvas.height = Math.max(1, Math.floor(height * scale));

    const ctx = canvas.getContext('2d');
    ctx.scale(scale, scale);
    ctx.font = `${config.fontSize}px ${config.fontFamily}`;

    const columns = buildColumns(config, width);

    const state = {
        canvas,
        ctx,
        config,
        columns,
        running: true,
        animFrameId: null,
        lastFrameTime: 0,
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
            state.ctx.font = `${state.config.fontSize}px ${state.config.fontFamily}`;
            state.columns = buildColumns(state.config, newW);
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
    state.ctx.font = `${state.config.fontSize}px ${state.config.fontFamily}`;

    const { width } = getCanvasSize(state.canvas);
    state.columns = buildColumns(state.config, width);
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
        state.lastFrameTime = timestamp;

        drawFrame(state);

        state.animFrameId = requestAnimationFrame(loop);
    };

    state.animFrameId = requestAnimationFrame(loop);
}

function drawFrame(state) {
    const { ctx, canvas, config, columns, scale } = state;
    const displayWidth = canvas.width / scale;
    const displayHeight = canvas.height / scale;

    // Draw semi-transparent fade overlay (trail fading technique)
    ctx.fillStyle = config.fadeColor;
    ctx.globalAlpha = 0.05;
    ctx.fillRect(0, 0, displayWidth, displayHeight);
    ctx.globalAlpha = 1.0;

    // Draw each column
    for (const col of columns) {
        col.y += col.speed * config.fontSize * 0.5;

        // Draw lead character (brighter)
        ctx.fillStyle = config.color;
        ctx.globalAlpha = 1.0;
        const char = randomChar(config.characters);
        ctx.fillText(char, col.x, col.y);

        // Occasionally draw a brighter "head" effect
        if (Math.random() < 0.1) {
            ctx.globalAlpha = 1.0;
            ctx.fillStyle = '#ffffff';
            ctx.fillText(randomChar(config.characters), col.x, col.y);
        }

        // Reset column when it falls below the canvas
        if (col.y > displayHeight + config.fontSize * 5) {
            col.y = Math.random() * -displayHeight * 0.3;
            col.speed = (0.5 + Math.random() * 0.8) * config.speed;
        }
    }

    ctx.globalAlpha = 1.0;
}

// ─── Config Normalization ────────────────────────────────────────

function normalizeConfig(raw) {
    const config = {
        characters: String(raw?.characters || 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#$%&'),
        fontSize: Math.max(4, Number(raw?.fontSize) || 16),
        fontFamily: String(raw?.fontFamily || 'monospace'),
        color: String(raw?.color || '#00ff41'),
        fadeColor: String(raw?.fadeColor || '#003b00'),
        speed: Math.max(0.1, Number(raw?.speed) || 1.0),
        density: Math.max(0.01, Math.min(1.0, Number(raw?.density) || 1.0)),
        opacity: Math.max(0, Math.min(1.0, Number(raw?.opacity) || 0.8)),
        targetFps: Math.max(1, Math.min(120, Number(raw?.targetFps) || 30))
    };
    return applyReducedMotion(config);
}
