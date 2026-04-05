/**
 * BlazorEffects.Starfield - Starfield warp-speed animated background effect
 *
 * Exports: init, update, dispose
 *
 * Rendering technique: Canvas 2D with 3D-to-2D star projection.
 * Stars spawn at random x,y,z positions and fly toward the camera (z=0).
 * As z decreases, stars appear larger and faster — creating the hyperspace warp effect.
 * Optional trails create streaking lines behind each star.
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
 * Create a star at a random position in the 3D field.
 */
function createStar(maxDepth, canvasWidth, canvasHeight) {
    return {
        x: (Math.random() - 0.5) * canvasWidth * 2,
        y: (Math.random() - 0.5) * canvasHeight * 2,
        z: Math.random() * maxDepth,
        prevX: 0,
        prevY: 0
    };
}

/**
 * Create a pool of stars distributed throughout the depth field.
 */
function createStarPool(count, maxDepth, canvasWidth, canvasHeight) {
    const pool = new Array(count);
    for (let i = 0; i < count; i++) {
        pool[i] = createStar(maxDepth, canvasWidth, canvasHeight);
    }
    return pool;
}

/**
 * Update star positions — move each star closer to camera (decrease z).
 * Stars that pass the camera are recycled to the far end.
 */
function updateStars(stars, config, canvasWidth, canvasHeight, speed) {
    const halfW = canvasWidth / 2;
    const halfH = canvasHeight / 2;
    const maxDepth = config.depth;

    for (let i = 0; i < stars.length; i++) {
        const star = stars[i];

        // Project current position to 2D for trail drawing
        const k = Math.max(0.001, 128 / star.z);
        star.prevX = star.x * k + halfW;
        star.prevY = star.y * k + halfH;

        // Move star toward camera
        star.z -= speed;

        // Recycle star if it passes the camera
        if (star.z <= 0) {
            star.x = (Math.random() - 0.5) * canvasWidth * 2;
            star.y = (Math.random() - 0.5) * canvasHeight * 2;
            star.z = maxDepth;
            star.prevX = star.x * (128 / star.z) + halfW;
            star.prevY = star.y * (128 / star.z) + halfH;
        }
    }
}

/**
 * Draw all stars with optional trails.
 */
function drawFrame(ctx, stars, config, canvasWidth, canvasHeight) {
    const halfW = canvasWidth / 2;
    const halfH = canvasHeight / 2;
    const rgb = hexToRgb(config.starColor);
    const maxStarSize = config.starSize;
    const trailLength = config.trailLength;
    const opacity = config.opacity;

    // Clear with background
    ctx.fillStyle = config.backgroundColor;
    ctx.fillRect(0, 0, canvasWidth, canvasHeight);

    for (let i = 0; i < stars.length; i++) {
        const star = stars[i];
        const k = Math.max(0.001, 128 / star.z);
        const sx = star.x * k + halfW;
        const sy = star.y * k + halfH;

        // Skip stars outside viewport
        if (sx < -50 || sx > canvasWidth + 50 || sy < -50 || sy > canvasHeight + 50) continue;

        // Size based on proximity (closer = bigger)
        const size = Math.max(0.5, (1 - star.z / config.depth) * maxStarSize);

        // Brightness based on proximity
        const brightness = Math.min(1, (1 - star.z / config.depth) * 1.5);

        // Draw trail if enabled
        if (trailLength > 0 && star.z < config.depth * 0.9) {
            ctx.strokeStyle = `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, ${brightness * trailLength * opacity})`;
            ctx.lineWidth = Math.max(0.5, size * 0.5);
            ctx.beginPath();
            ctx.moveTo(star.prevX, star.prevY);
            ctx.lineTo(sx, sy);
            ctx.stroke();
        }

        // Draw star point
        ctx.beginPath();
        ctx.arc(sx, sy, size, 0, Math.PI * 2);
        ctx.fillStyle = `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, ${brightness * opacity})`;
        ctx.fill();
    }
}

// ─── Init ─────────────────────────────────────────────────────────

export function init(canvas, rawConfig) {
    const config = normalizeConfig(rawConfig);
    const { width, height } = getCanvasSize(canvas);

    canvas.width = Math.max(1, width);
    canvas.height = Math.max(1, height);

    const ctx = canvas.getContext('2d');
    const stars = createStarPool(config.starCount, config.depth, width, height);

    const state = {
        canvas,
        ctx,
        config,
        stars,
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
            state.stars = createStarPool(state.config.starCount, state.config.depth, newW, newH);
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

    // Rebuild stars if count or depth changed
    const { width, height } = getCanvasSize(state.canvas);
    state.stars = createStarPool(state.config.starCount, state.config.depth, width, height);
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

        const { width, height } = getCanvasSize(state.canvas);

        updateStars(state.stars, state.config, width, height, state.config.speed);
        drawFrame(state.ctx, state.stars, state.config, width, height);

        state.animFrameId = requestAnimationFrame(loop);
    };

    state.animFrameId = requestAnimationFrame(loop);
}

// ─── Config Normalization ────────────────────────────────────────

function normalizeConfig(raw) {
    return {
        starCount: Math.max(1, Math.min(3000, Number(raw?.starCount) || 800)),
        starColor: String(raw?.starColor || '#ffffff'),
        starSize: Math.max(0.5, Number(raw?.starSize) || 2),
        speed: Math.max(0.1, Number(raw?.speed) || 2),
        trailLength: Math.max(0, Math.min(1.0, Number(raw?.trailLength) || 0.6)),
        depth: Math.max(100, Number(raw?.depth) || 1000),
        backgroundColor: String(raw?.backgroundColor || '#000000'),
        opacity: Math.max(0, Math.min(1.0, Number(raw?.opacity) || 1.0)),
        targetFps: Math.max(1, Math.min(120, Number(raw?.targetFps) || 60))
    };
}
