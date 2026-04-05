/**
 * BlazorEffects.Core - Shared JavaScript runtime
 *
 * Provides reusable utilities for all effect modules:
 * - FPS throttling via requestAnimationFrame
 * - IntersectionObserver for off-screen pause
 * - Debounced resize handling
 * - Instance manager (Map-based)
 */

/**
 * Create an FPS-throttled frame callback wrapper.
 * @param {number} targetFps - Desired frames per second (1-120)
 * @returns {{ shouldRender: (timestamp: number) => boolean, setFps: (fps: number) => void }}
 */
export function createFpsThrottle(targetFps) {
    let fps = Math.max(1, Math.min(120, targetFps));
    let lastFrameTime = 0;

    return {
        shouldRender(timestamp) {
            const interval = 1000 / fps;
            if (timestamp - lastFrameTime < interval) return false;
            lastFrameTime = timestamp;
            return true;
        },
        setFps(newFps) {
            fps = Math.max(1, Math.min(120, newFps));
        },
        getFps() {
            return fps;
        }
    };
}

/**
 * Create an IntersectionObserver that pauses/resumes animation when canvas scrolls off-screen.
 * @param {HTMLElement} canvas - The canvas element to observe
 * @param {Function} onPause - Called when element scrolls off-screen
 * @param {Function} onResume - Called when element scrolls back on-screen
 * @returns {{ disconnect: () => void }}
 */
export function createIntersectionPause(canvas, onPause, onResume) {
    const observer = new IntersectionObserver(
        (entries) => {
            for (const entry of entries) {
                if (entry.isIntersecting) {
                    onResume();
                } else {
                    onPause();
                }
            }
        },
        { threshold: 0 }
    );
    observer.observe(canvas);

    return {
        disconnect() {
            observer.disconnect();
        }
    };
}

/**
 * Create a debounced version of a function.
 * @param {Function} fn - Function to debounce
 * @param {number} ms - Debounce delay in milliseconds
 * @returns {Function} Debounced function with .cancel() method
 */
export function debounce(fn, ms) {
    let timer = null;
    const debounced = (...args) => {
        if (timer) clearTimeout(timer);
        timer = setTimeout(() => {
            timer = null;
            fn(...args);
        }, ms);
    };
    debounced.cancel = () => {
        if (timer) clearTimeout(timer);
        timer = null;
    };
    return debounced;
}

/**
 * Create a simple instance manager (Map-based) for tracking multiple effect instances.
 * @returns {{ register: (state: object) => string, get: (id: string) => object|undefined, remove: (id: string) => boolean, clear: () => void }}
 */
export function createInstanceManager() {
    const instances = new Map();
    let nextId = 0;

    return {
        register(state) {
            const id = String(nextId++);
            instances.set(id, state);
            return id;
        },
        get(id) {
            return instances.get(id);
        },
        remove(id) {
            return instances.delete(id);
        },
        clear() {
            instances.clear();
        },
        get size() {
            return instances.size;
        }
    };
}
