/**
 * BlazorEffects.Particles - Particle Constellation (network graph) animated background effect
 *
 * Exports: init, update, dispose, setMousePosition, clearMousePosition
 *
 * Rendering technique: Canvas 2D particles with proximity-based connection lines.
 * Features spatial hashing for O(n) connection detection, object pooling,
 * batched line rendering, and interactive mouse force.
 */

// Instance management
const instances = new Map();
let nextId = 0;

// ─── Spatial Hash Grid ────────────────────────────────────────────
// Grid-based spatial partitioning for efficient neighbor lookups.

class SpatialHash {
    constructor(cellSize) {
        this.cellSize = Math.max(1, cellSize);
        this.grid = new Map();
    }

    clear() {
        this.grid.clear();
    }

    _key(cx, cy) {
        return `${cx},${cy}`;
    }

    insert(particle) {
        const cx = Math.floor(particle.x / this.cellSize);
        const cy = Math.floor(particle.y / this.cellSize);
        const key = this._key(cx, cy);
        let cell = this.grid.get(key);
        if (!cell) {
            cell = [];
            this.grid.set(key, cell);
        }
        cell.push(particle);
    }

    query(particle, radius) {
        const results = [];
        const minCx = Math.floor((particle.x - radius) / this.cellSize);
        const maxCx = Math.floor((particle.x + radius) / this.cellSize);
        const minCy = Math.floor((particle.y - radius) / this.cellSize);
        const maxCy = Math.floor((particle.y + radius) / this.cellSize);

        for (let cx = minCx; cx <= maxCx; cx++) {
            for (let cy = minCy; cy <= maxCy; cy++) {
                const cell = this.grid.get(this._key(cx, cy));
                if (cell) {
                    for (const p of cell) {
                        if (p !== particle) {
                            results.push(p);
                        }
                    }
                }
            }
        }
        return results;
    }
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
 * Create a pre-allocated particle pool.
 */
function createParticlePool(count, canvasWidth, canvasHeight, speed) {
    const pool = new Array(count);
    for (let i = 0; i < count; i++) {
        const angle = Math.random() * Math.PI * 2;
        const vel = (0.3 + Math.random() * 0.7) * speed;
        pool[i] = {
            idx: i,
            x: Math.random() * canvasWidth,
            y: Math.random() * canvasHeight,
            vx: Math.cos(angle) * vel,
            vy: Math.sin(angle) * vel,
            baseSpeed: vel
        };
    }
    return pool;
}

/**
 * Update particle positions with boundary wrapping and optional mouse interaction.
 */
function updateParticles(particles, config, canvasWidth, canvasHeight, mouseX, mouseY) {
    const speed = config.speed;
    const hasMouse = config.mouseInteraction && mouseX !== null && mouseY !== null;
    const mouseRadius = config.mouseRadius;
    const mouseRadiusSq = mouseRadius * mouseRadius;
    const isAttract = config.mouseForce === 'attract';

    for (let i = 0; i < particles.length; i++) {
        const p = particles[i];

        // Mouse interaction force
        if (hasMouse) {
            const dx = mouseX - p.x;
            const dy = mouseY - p.y;
            const distSq = dx * dx + dy * dy;

            if (distSq < mouseRadiusSq && distSq > 1) {
                const dist = Math.sqrt(distSq);
                // Smooth force falloff: stronger near cursor, weaker at edge
                const force = (1 - dist / mouseRadius) * 0.03 * speed;
                const nx = dx / dist;
                const ny = dy / dist;

                if (isAttract) {
                    p.vx += nx * force;
                    p.vy += ny * force;
                } else {
                    p.vx -= nx * force;
                    p.vy -= ny * force;
                }
            }
        }

        // Apply velocity
        p.x += p.vx * speed;
        p.y += p.vy * speed;

        // Boundary wrapping
        if (p.x < 0) p.x += canvasWidth;
        else if (p.x > canvasWidth) p.x -= canvasWidth;
        if (p.y < 0) p.y += canvasHeight;
        else if (p.y > canvasHeight) p.y -= canvasHeight;

        // Dampen velocity back toward base speed (prevents runaway from mouse)
        const currentSpeed = Math.sqrt(p.vx * p.vx + p.vy * p.vy);
        if (currentSpeed > p.baseSpeed * 3) {
            const dampen = (p.baseSpeed * 3) / currentSpeed;
            p.vx *= dampen;
            p.vy *= dampen;
        }
    }
}

/**
 * Draw all particles and connections.
 */
function drawFrame(ctx, particles, spatialHash, config, canvasWidth, canvasHeight) {
    ctx.clearRect(0, 0, canvasWidth, canvasHeight);

    const connectionDist = config.connectionDistance;
    const connectionDistSq = connectionDist * connectionDist;
    const particleSize = config.particleSize;
    const particleRgb = hexToRgb(config.particleColor);
    const connectionRgb = hexToRgb(config.connectionColor);

    // Rebuild spatial hash
    spatialHash.clear();
    for (let i = 0; i < particles.length; i++) {
        spatialHash.insert(particles[i]);
    }

    // Collect connections grouped by opacity bucket for batched rendering
    // Use index-based dedup: only draw (i, j) where i < j
    const opacityBuckets = new Map();

    for (let i = 0; i < particles.length; i++) {
        const p = particles[i];
        const neighbors = spatialHash.query(p, connectionDist);

        for (const q of neighbors) {
            // Only draw once: i < j
            if (q.idx <= i) continue;

            const dx = p.x - q.x;
            const dy = p.y - q.y;
            const distSq = dx * dx + dy * dy;

            if (distSq < connectionDistSq) {
                const dist = Math.sqrt(distSq);
                // Bucket opacity to nearest 0.05 for batching
                const alpha = Math.round((1 - dist / connectionDist) * 20) / 20;
                const bucketKey = alpha;
                let bucket = opacityBuckets.get(bucketKey);
                if (!bucket) {
                    bucket = [];
                    opacityBuckets.set(bucketKey, bucket);
                }
                bucket.push(p.x, p.y, q.x, q.y);
            }
        }
    }

    // Draw batched connections per opacity level
    for (const [alpha, coords] of opacityBuckets) {
        ctx.strokeStyle = `rgba(${connectionRgb.r}, ${connectionRgb.g}, ${connectionRgb.b}, ${alpha * config.opacity})`;
        ctx.beginPath();
        for (let k = 0; k < coords.length; k += 4) {
            ctx.moveTo(coords[k], coords[k + 1]);
            ctx.lineTo(coords[k + 2], coords[k + 3]);
        }
        ctx.stroke();
    }

    // Batch all particles
    ctx.beginPath();
    for (let i = 0; i < particles.length; i++) {
        const p = particles[i];
        ctx.moveTo(p.x + particleSize, p.y);
        ctx.arc(p.x, p.y, particleSize, 0, Math.PI * 2);
    }
    ctx.fillStyle = `rgba(${particleRgb.r}, ${particleRgb.g}, ${particleRgb.b}, ${config.opacity})`;
    ctx.fill();
}

// ─── Init ─────────────────────────────────────────────────────────

export function init(canvas, rawConfig) {
    const config = normalizeConfig(rawConfig);
    const { width, height } = getCanvasSize(canvas);

    // Full resolution — particles are small, precise elements
    canvas.width = Math.max(1, width);
    canvas.height = Math.max(1, height);

    const ctx = canvas.getContext('2d');
    ctx.lineWidth = 0.5;

    // Create particle pool and spatial hash
    const particles = createParticlePool(config.particleCount, width, height, 1.0);
    const spatialHash = new SpatialHash(config.connectionDistance);

    const state = {
        canvas,
        ctx,
        config,
        particles,
        spatialHash,
        mouseX: null,
        mouseY: null,
        running: true,
        animFrameId: null,
        lastFrameTime: 0,
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
            canvas.width = Math.max(1, newW);
            canvas.height = Math.max(1, newH);
            state.ctx.lineWidth = 0.5;

            // Rebuild particles for new dimensions
            state.particles = createParticlePool(
                state.config.particleCount, newW, newH, 1.0
            );
            state.spatialHash = new SpatialHash(state.config.connectionDistance);
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
    state.particles = createParticlePool(
        state.config.particleCount, width, height, 1.0
    );
    state.spatialHash = new SpatialHash(state.config.connectionDistance);
}

// ─── Mouse Interaction ────────────────────────────────────────────

export function setMousePosition(instanceId, x, y) {
    const state = instances.get(instanceId);
    if (!state) return;
    state.mouseX = x;
    state.mouseY = y;
}

export function clearMousePosition(instanceId) {
    const state = instances.get(instanceId);
    if (!state) return;
    state.mouseX = null;
    state.mouseY = null;
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

        const { width, height } = getCanvasSize(state.canvas);

        // Update particle positions
        updateParticles(
            state.particles,
            state.config,
            width, height,
            state.mouseX, state.mouseY
        );

        // Draw frame
        drawFrame(
            state.ctx,
            state.particles,
            state.spatialHash,
            state.config,
            width, height
        );

        state.animFrameId = requestAnimationFrame(loop);
    };

    state.animFrameId = requestAnimationFrame(loop);
}

// ─── Config Normalization ────────────────────────────────────────

function normalizeConfig(raw) {
    return {
        particleCount: Math.max(1, Math.min(500, Number(raw?.particleCount) || 150)),
        particleColor: String(raw?.particleColor || '#6366f1'),
        particleSize: Math.max(0.5, Number(raw?.particleSize) || 2),
        connectionDistance: Math.max(10, Number(raw?.connectionDistance) || 120),
        connectionColor: String(raw?.connectionColor || '#6366f1'),
        speed: Math.max(0.05, Number(raw?.speed) || 0.5),
        mouseInteraction: raw?.mouseInteraction !== false,
        mouseRadius: Math.max(10, Number(raw?.mouseRadius) || 150),
        mouseForce: raw?.mouseForce === 'repel' ? 'repel' : 'attract',
        opacity: Math.max(0, Math.min(1.0, Number(raw?.opacity) || 0.6)),
        targetFps: Math.max(1, Math.min(120, Number(raw?.targetFps) || 60))
    };
}
