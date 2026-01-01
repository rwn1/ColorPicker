/* ============================
   PUBLIC API
============================ */

export function initHueSelectionBackground(canvas) {
    if (!canvas) return;

    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    function redraw() {
        const rect = canvas.getBoundingClientRect();
        const dpr = window.devicePixelRatio || 1;

        const width = Math.round(rect.width * dpr);
        const height = Math.round(rect.height * dpr);

        if (canvas.width !== width || canvas.height !== height) {
            canvas.width = width;
            canvas.height = height;
        }

        ctx.setTransform(dpr, 0, 0, dpr, 0, 0);

        drawHueGradient(ctx, rect.width, rect.height);
    }

    redraw();

    const resizeObserver = new ResizeObserver(() => redraw());
    resizeObserver.observe(canvas);

    canvas._hueResizeObserver = resizeObserver;
}

/* ============================
   DRAWING
============================ */

function drawHueGradient(ctx, width, height) {
    ctx.clearRect(0, 0, width, height);

    const horizontal = width >= height;

    const gradient = horizontal
        ? ctx.createLinearGradient(0, 0, width, 0)
        : ctx.createLinearGradient(0, 0, 0, height);

    // Hue spectrum
    gradient.addColorStop(0.0, "rgb(255,   0,   0)"); // red
    gradient.addColorStop(1 / 6, "rgb(255, 255,   0)"); // yellow
    gradient.addColorStop(2 / 6, "rgb(0,   255,   0)"); // green
    gradient.addColorStop(3 / 6, "rgb(0,   255, 255)"); // cyan
    gradient.addColorStop(4 / 6, "rgb(0,     0, 255)"); // blue
    gradient.addColorStop(5 / 6, "rgb(255,   0, 255)"); // magenta
    gradient.addColorStop(1.0, "rgb(255,   0,   0)"); // red

    ctx.fillStyle = gradient;
    ctx.fillRect(0, 0, width, height);
}