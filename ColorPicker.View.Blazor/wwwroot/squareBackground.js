/* ============================
   PUBLIC API
============================ */

export function initSquareBackground(canvas) {
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

        drawBackground(ctx, rect.width, rect.height);
    }

    redraw();

    const resizeObserver = new ResizeObserver(() => {
        redraw();
    });

    resizeObserver.observe(canvas);

    canvas._squareObserver = resizeObserver;
}


/* ============================
   DRAWING
============================ */

function drawBackground(ctx, width, height) {

    const light = "#f0f0f0";
    const dark = "#d0d0d0";

    const tile = Math.min(width, height);

    for (let y = 0; y < height; y += tile) {
        for (let x = 0; x < width; x += tile) {
            ctx.fillStyle = light;
            ctx.fillRect(x, y, tile, tile);

            ctx.fillStyle = dark;
            ctx.fillRect(x, y, tile / 2, tile / 2);
            ctx.fillRect(x + tile / 2, y + tile / 2, tile / 2, tile / 2);
        }
    }
}