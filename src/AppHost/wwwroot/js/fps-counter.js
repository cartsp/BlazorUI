// fps-counter.js — JS interop utility for real FPS measurement
// Updates every 500ms via DotNetObjectReference callback.

let running = false;
let frames = 0;
let lastTime = 0;
let rafId = 0;

function tick(now, dotNetRef) {
    if (!running) return;
    frames++;
    const elapsed = now - lastTime;
    if (elapsed >= 500) {
        const fps = Math.round((frames * 1000) / elapsed);
        try {
            dotNetRef.invokeMethodAsync('UpdateFps', fps);
        } catch {
            // Component disposed
            running = false;
            return;
        }
        frames = 0;
        lastTime = now;
    }
    rafId = requestAnimationFrame((t) => tick(t, dotNetRef));
}

export function start(dotNetRef) {
    running = true;
    frames = 0;
    lastTime = performance.now();
    rafId = requestAnimationFrame((t) => tick(t, dotNetRef));
}

export function stop() {
    running = false;
    if (rafId) {
        cancelAnimationFrame(rafId);
        rafId = 0;
    }
}
