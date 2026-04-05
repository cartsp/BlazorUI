/**
 * BlazorEffects.Ripple - Ripple animated background effect
 *
 * Exports: init, update, dispose
 *
 * Rendering technique: Canvas 2D with concentric circles expanding from
 * click/touch or auto-generated center points. Each ripple has radius,
 * opacity, and lineWidth that change over time. Overlapping ripples use
 * 'lighter' composite operation for a glow effect.
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

function hexToRgb(hex) {
    const r = parseInt(hex.slice(1, 3), 16);
    const g = parseInt(hex.slice(3, 5), 16);
    const b = parseInt(hex.slice(5, 7), 16);
    return { r, g, b };
}

function getCanvasSize(canvas) {
    const rect = canvas.parentElement.getBoundingClientRect();
    const config = {
        width: Math.max(1, Math.floor(rect.width)),
        height: Math.max(1, Math.floor(rect.height))
    };
    return applyReducedMotion(config);
}

/**
 * Create a single ripple at a given center point.
 */
function createRipple(x, y, config) {
    const config = {
        x: x,
        y: y,
        radius: 0,
        opacity: 1.0,
        lineWidth: config.lineWidth,
        speed: config.speed,
        maxRadius: config.maxRadius,
        decay: config.decay
    };
    return applyReducedMotion(config);
}

/**
 * Update all ripples — expand radius, decay opacity, remove dead ones.
 */
function updateRipples(ripples, config) {
    for (let i = ripples.length - 1; i >= 0; i--) {
        const r = ripples[i];
        r.radius += r.speed;
        r.opacity -= r.decay;

        if (r.opacity <= 0 || r.radius >= r.maxRadius) {
            ripples.splice(i, 1);
        }
    }
}

/**
 * Draw all active ripples with glow composite.
 */
function drawFrame(ctx, ripples, config, canvasWidth, canvasHeight) {
    const rgb = hexToRgb(config.color);

    // Clear with background
    ctx.fillStyle = config.backgroundColor;
    ctx.fillRect(0, 0, canvasWidth, canvasHeight);

    // Use 'lighter' for glow effect on overlapping ripples
    ctx.globalCompositeOperation = 'lighter';

    for (let i = 0; i < ripples.length; i++) {
        const r = ripples[i];
        const alpha = Math.max(0, r.opacity) * config.opacity;
        if (alpha <= 0) continue;

        // Outer ring
        ctx.beginPath();
        ctx.arc(r.x, r.y, Math.max(0.1, r.radius), 0, Math.PI * 2);
        ctx.strokeStyle = `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, ${alpha})`;
        ctx.lineWidth = Math.max(0.1, r.lineWidth * (1 - r.radius / r.maxRadius * 0.5));
        ctx.stroke();

        // Inner glow ring (smaller, brighter)
        if (r.radius > 10) {
            ctx.beginPath();
            ctx.arc(r.x, r.y, Math.max(0.1, r.radius * 0.6), 0, Math.PI * 2);
            ctx.strokeStyle = `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, ${alpha * 0.4})`;
            ctx.lineWidth = Math.max(0.1, r.lineWidth * 0.5);
            ctx.stroke();
        }
    }

    ctx.globalCompositeOperation = 'source-over';
}

/**
 * Auto-generate ripples at random positions.
 */
function autoGenerateRipple(state) {
    if (state.ripples.length >= state.config.maxRipples) return;
    if (state.config.autoRippleCount <= 0) return;

    const { width, height } = getCanvasSize(state.canvas);
    const x = Math.random() * width;
    const y = Math.random() * height;
    state.ripples.push(createRipple(x, y, state.config));
}

// ─── Init ─────────────────────────────────────────────────────────

export function init(canvas, rawConfig) {
    const config = normalizeConfig(rawConfig);
    const { width, height } = getCanvasSize(canvas);

    canvas.width = Math.max(1, width);
    canvas.height = Math.max(1, height);

    const ctx = canvas.getContext('2d');
    const ripples = [];

    const state = {
        canvas,
        ctx,
        config,
        ripples,
        running: true,
        animFrameId: null,
        lastFrameTime: 0,
        observer: null,
        resizeHandler: null,
        clickHandler: null,
        autoIntervalId: null
    };

    const id = String(nextId++);
    instances.set(id, state);

    // Start auto ripple generation if in auto mode
    if (config.trigger === 'auto') {
        startAutoGeneration(id);
    }

    // Click/touch handler for click mode
    if (config.trigger === 'click') {
        state.clickHandler = (e) => {
            const rect = canvas.getBoundingClientRect();
            const x = e.clientX - rect.left;
            const y = e.clientY - rect.top;
            if (state.ripples.length < state.config.maxRipples) {
                state.ripples.push(createRipple(x, y, state.config));
            }
        };
        canvas.addEventListener('click', state.clickHandler);

        // Touch support
        state.touchHandler = (e) => {
            e.preventDefault();
            const rect = canvas.getBoundingClientRect();
            for (const touch of e.changedTouches) {
                const x = touch.clientX - rect.left;
                const y = touch.clientY - rect.top;
                if (state.ripples.length < state.config.maxRipples) {
                    state.ripples.push(createRipple(x, y, state.config));
                }
            }
        };
        canvas.addEventListener('touchstart', state.touchHandler, { passive: false });
    }

    startLoop(id);

    // IntersectionObserver for off-screen pause
    if ('IntersectionObserver' in window) {
        state.observer = new IntersectionObserver(
            (entries) => {
                for (const entry of entries) {
                    if (entry.isIntersecting && !state.running) {
                        state.running = true;
                        startLoop(id);
                        if (state.config.trigger === 'auto') {
                            startAutoGeneration(id);
                        }
                    } else if (!entry.isIntersecting && state.running) {
                        state.running = false;
                        if (state.animFrameId) {
                            cancelAnimationFrame(state.animFrameId);
                            state.animFrameId = null;
                        }
                        if (state.autoIntervalId) {
                            clearInterval(state.autoIntervalId);
                            state.autoIntervalId = null;
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

    // Restart auto generation if needed
    if (state.autoIntervalId) {
        clearInterval(state.autoIntervalId);
        state.autoIntervalId = null;
    }
    if (state.config.trigger === 'auto' && state.running) {
        startAutoGeneration(instanceId);
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
    if (state.autoIntervalId) {
        clearInterval(state.autoIntervalId);
        state.autoIntervalId = null;
    }
    if (state.observer) {
        state.observer.disconnect();
        state.observer = null;
    }
    if (state.resizeHandler) {
        window.removeEventListener('resize', state.resizeHandler);
        state.resizeHandler = null;
    }
    if (state.clickHandler) {
        state.canvas.removeEventListener('click', state.clickHandler);
        state.clickHandler = null;
    }
    if (state.touchHandler) {
        state.canvas.removeEventListener('touchstart', state.touchHandler);
        state.touchHandler = null;
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

        const { width, height } = getCanvasSize(state.canvas);

        updateRipples(state.ripples, state.config);
        drawFrame(state.ctx, state.ripples, state.config, width, height);

        state.animFrameId = requestAnimationFrame(loop);
    };

    state.animFrameId = requestAnimationFrame(loop);
}

// ─── Auto Generation ──────────────────────────────────────────────

function startAutoGeneration(id) {
    const state = instances.get(id);
    if (!state) return;

    // Generate initial batch
    for (let i = 0; i < state.config.autoRippleCount; i++) {
        setTimeout(() => autoGenerateRipple(state), i * (state.config.autoInterval / state.config.autoRippleCount));
    }

    // Periodic generation
    state.autoIntervalId = setInterval(() => {
        autoGenerateRipple(state);
    }, state.config.autoInterval);
}

// ─── Config Normalization ────────────────────────────────────────

function normalizeConfig(raw) {
    const config = {
        maxRipples: Math.max(1, Math.min(40, Number(raw?.maxRipples) || 20)),
        maxRadius: Math.max(50, Number(raw?.maxRadius) || 300),
        speed: Math.max(0.5, Number(raw?.speed) || 3),
        color: String(raw?.color || '#60a5fa'),
        lineWidth: Math.max(0.5, Number(raw?.lineWidth) || 2),
        decay: Math.max(0.005, Math.min(0.1, Number(raw?.decay) || 0.02)),
        trigger: (raw?.trigger === 'click') ? 'click' : 'auto',
        autoRippleCount: Math.max(0, Number(raw?.autoRippleCount) || 5),
        autoInterval: Math.max(100, Number(raw?.autoInterval) || 800),
        backgroundColor: String(raw?.backgroundColor || '#0f172a'),
        opacity: Math.max(0, Math.min(1.0, Number(raw?.opacity) || 1.0)),
        targetFps: Math.max(1, Math.min(120, Number(raw?.targetFps) || 60))
    };
    return applyReducedMotion(config);
}
