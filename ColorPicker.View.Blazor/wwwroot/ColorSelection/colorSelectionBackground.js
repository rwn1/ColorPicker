/* ============================
   PUBLIC API
============================ */

export function initColorSelectionBackground(canvas, hue) {
    if (!canvas) return;

    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    canvas._svHue = hue ?? 0;

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

        drawSVGradient(ctx, rect.width, rect.height, canvas._svHue);
    }

    redraw();

    const resizeObserver = new ResizeObserver(() => redraw());
    resizeObserver.observe(canvas);

    canvas._svResizeObserver = resizeObserver;
}

export function updateColorSelectionBackground(canvas, hue) {
    if (!canvas) return;

    canvas._svHue = hue;

    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    const rect = canvas.getBoundingClientRect();
    drawSVGradient(ctx, rect.width, rect.height, hue);
}

/* ============================
   DRAWING
============================ */

function drawSVGradient(ctx, width, height, hue) {
    ctx.clearRect(0, 0, width, height);

    // 1️. Saturation: white → hue
    const satGradient = ctx.createLinearGradient(0, 0, width, 0);
    satGradient.addColorStop(0, "#ffffff");
    satGradient.addColorStop(1, `hsl(${hue}, 100%, 50%)`);

    ctx.fillStyle = satGradient;
    ctx.fillRect(0, 0, width, height);

    // 2️. Value: transparent → black
    const valGradient = ctx.createLinearGradient(0, 0, 0, height);
    valGradient.addColorStop(0, "rgba(0,0,0,0)");
    valGradient.addColorStop(1, "rgba(0,0,0,1)");

    ctx.fillStyle = valGradient;
    ctx.fillRect(0, 0, width, height);
}