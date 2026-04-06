/**
 * BlazorEffects.FireEmbers - Fire and Embers animated background effect
 *
 * Exports: init, update, dispose
 *
 * Rendering technique: Canvas 2D particle system.
 * Two particle types:
 *   - Flame particles: larger, soft-edged circles with radial gradient, fade out as they rise
 *   - Ember particles: smaller, brighter dots with more turbulence, slower fade
 * Particles spawn at the bottom, rise upward with turbulence, and fade out.
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
    return {
        width: Math.max(1, Math.floor(rect.width)),
        height: Math.max(1, Math.floor(rect.height))
    };
}

/**
 * Create a single fire/ember particle.
 */
function createParticle(cfg, canvasWidth, canvasHeight) {
    const isEmber = Math.random() < cfg.emberRatio;
    const config = {
        x: Math.random() * canvasWidth,
        y: canvasHeight + Math.random() * 50, // Start just below canvas
        vx: 0,
        vy: 0,
        isEmber: isEmber,
        life: 0,
        maxLife: 0.5 + Math.random() * 0.5, // 0.5 to 1.0 normalized lifetime
        size: isEmber
            ? (0.5 + Math.random() * 1.5) * (cfg.particleSize / 4)
            : (1.0 + Math.random() * 1.0) * cfg.particleSize,
        wobbleOffset: Math.random() * Math.PI * 2,
        wobbleSpeed: 1 + Math.random() * 2
    };
    return applyReducedMotion(config);
}

/**
 * Create a pool of fire/ember particles.
 */
function createParticlePool(count, config, canvasWidth, canvasHeight) {
    const pool = new Array(count);
    for (let i = 0; i < count; i++) {
        const p = createParticle(config, canvasWidth, canvasHeight);
        // Stagger initial positions so they don't all start at the bottom
        p.life = Math.random() * p.maxLife;
        p.y = canvasHeight - (p.life / p.maxLife) * canvasHeight * 0.8;
        pool[i] = p;
    }
    return pool;
}

/**
 * Update particle positions — rise upward with turbulence and fade.
 */
function updateParticles(particles, config, canvasWidth, canvasHeight, deltaTime) {
    const speed = config.speed;
    const turbulence = config.turbulence;

    for (let i = 0; i < particles.length; i++) {
        const p = particles[i];

        // Advance life
        p.life += deltaTime * speed * 0.001;

        // Recycle if life exceeded
        if (p.life >= p.maxLife) {
            const newP = createParticle(config, canvasWidth, canvasHeight);
            newP.life = 0;
            particles[i] = newP;
            continue;
        }

        // Rise speed (embers rise slower than flames)
        const riseSpeed = p.isEmber ? speed * 0.6 : speed;

        // Position update
        p.y -= riseSpeed * deltaTime * 0.1;
        p.x += Math.sin(p.wobbleOffset + p.life * p.wobbleSpeed) * turbulence * deltaTime * 0.02;

        // Horizontal drift for flames
        if (!p.isEmber) {
            p.x += (Math.random() - 0.5) * turbulence * 0.5;
        }
    }
}

/**
 * Draw all fire and ember particles.
 */
function drawFrame(ctx, particles, config, canvasWidth, canvasHeight) {
    const flameRgb = hexToRgb(config.flameColor);
    const emberRgb = hexToRgb(config.emberColor);
    const opacity = config.opacity;

    // Clear with background
    ctx.fillStyle = config.backgroundColor;
    ctx.fillRect(0, 0, canvasWidth, canvasHeight);

    // Draw glow layer at base
    const glowGradient = ctx.createRadialGradient(
        canvasWidth / 2, canvasHeight, 0,
        canvasWidth / 2, canvasHeight, canvasHeight * 0.4
    );
    glowGradient.addColorStop(0, `rgba(${flameRgb.r}, ${flameRgb.g}, ${flameRgb.b}, ${0.15 * opacity})`);
    glowGradient.addColorStop(1, 'rgba(0, 0, 0, 0)');
    ctx.fillStyle = glowGradient;
    ctx.fillRect(0, 0, canvasWidth, canvasHeight);

    // Sort particles by life (draw newer/brighter ones on top)
    // Skip sorting for performance — draw in order

    for (let i = 0; i < particles.length; i++) {
        const p = particles[i];
        const lifeRatio = p.life / p.maxLife;

        // Skip dead particles
        if (lifeRatio >= 1) continue;

        // Skip particles outside viewport
        if (p.y < -20 || p.y > canvasHeight + 20 || p.x < -20 || p.x > canvasWidth + 20) continue;

        // Alpha fades in then out
        let alpha;
        if (lifeRatio < 0.1) {
            alpha = lifeRatio / 0.1; // Fade in
        } else {
            alpha = 1 - ((lifeRatio - 0.1) / 0.9); // Fade out
        }
        alpha = Math.max(0, Math.min(1, alpha));

        const size = Math.max(0.5, p.size * (1 - lifeRatio * 0.5)); // Shrink as they age

        if (p.isEmber) {
            // Embers: bright dots with glow
            ctx.beginPath();
            ctx.arc(p.x, p.y, size, 0, Math.PI * 2);
            ctx.fillStyle = `rgba(${emberRgb.r}, ${emberRgb.g}, ${emberRgb.b}, ${alpha * opacity})`;
            ctx.fill();

            // Small glow
            if (size > 1) {
                ctx.beginPath();
                ctx.arc(p.x, p.y, size * 2, 0, Math.PI * 2);
                ctx.fillStyle = `rgba(${emberRgb.r}, ${emberRgb.g}, ${emberRgb.b}, ${alpha * 0.15 * opacity})`;
                ctx.fill();
            }
        } else {
            // Flame particles: soft-edged circles with radial gradient
            const gradient = ctx.createRadialGradient(
                p.x, p.y, 0,
                p.x, p.y, Math.max(1, size * 2)
            );
            gradient.addColorStop(0, `rgba(${flameRgb.r}, ${flameRgb.g}, ${flameRgb.b}, ${alpha * 0.8 * opacity})`);
            gradient.addColorStop(0.4, `rgba(${flameRgb.r}, ${flameRgb.g}, ${flameRgb.b}, ${alpha * 0.3 * opacity})`);
            gradient.addColorStop(1, `rgba(${flameRgb.r}, ${flameRgb.g}, ${flameRgb.b}, 0)`);

            ctx.beginPath();
            ctx.arc(p.x, p.y, Math.max(1, size * 2), 0, Math.PI * 2);
            ctx.fillStyle = gradient;
            ctx.fill();
        }
    }
}

// ─── Init ─────────────────────────────────────────────────────────

export function init(canvas, rawConfig) {
    const config = normalizeConfig(rawConfig);
    const { width, height } = getCanvasSize(canvas);

    canvas.width = Math.max(1, width);
    canvas.height = Math.max(1, height);

    const ctx = canvas.getContext('2d');
    const particles = createParticlePool(config.particleCount, config, width, height);

    const state = {
        canvas,
        ctx,
        config,
        particles,
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
            state.particles = createParticlePool(state.config.particleCount, state.config, newW, newH);
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

    // Rebuild particles if count changed
    const { width, height } = getCanvasSize(state.canvas);
    state.particles = createParticlePool(state.config.particleCount, state.config, width, height);
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

        const delta = timestamp - state.lastFrameTime;
        state.lastFrameTime = timestamp;

        const { width, height } = getCanvasSize(state.canvas);

        updateParticles(state.particles, state.config, width, height, delta);
        drawFrame(state.ctx, state.particles, state.config, width, height);

        state.animFrameId = requestAnimationFrame(loop);
    };

    state.animFrameId = requestAnimationFrame(loop);
}

// ─── Config Normalization ────────────────────────────────────────

function normalizeConfig(raw) {
    const config = {
        particleCount: Math.max(1, Math.min(600, Number(raw?.particleCount) || 200)),
        flameColor: String(raw?.flameColor || '#ff6600'),
        emberColor: String(raw?.emberColor || '#ffcc00'),
        speed: Math.max(0.1, Number(raw?.speed) || 1.5),
        particleSize: Math.max(1, Number(raw?.particleSize) || 4),
        turbulence: Math.max(0, Number(raw?.turbulence) || 1),
        emberRatio: Math.max(0, Math.min(1.0, Number(raw?.emberRatio) || 0.3)),
        backgroundColor: String(raw?.backgroundColor || '#0a0a0a'),
        opacity: Math.max(0, Math.min(1.0, Number(raw?.opacity) || 0.9)),
        targetFps: Math.max(1, Math.min(120, Number(raw?.targetFps) || 60))
    };
    return applyReducedMotion(config);
}
