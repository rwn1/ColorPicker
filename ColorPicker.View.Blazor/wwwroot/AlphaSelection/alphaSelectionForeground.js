/* ============================
   PUBLIC API
============================ */

export function initAlphaSelectionForeground(canvas, color) {
    if (!canvas) return;

    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    canvas._alphaColor = color;

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

        drawAlphaGradient(ctx, rect.width, rect.height, canvas._alphaColor);
    }

    redraw();

    const resizeObserver = new ResizeObserver(() => redraw());
    resizeObserver.observe(canvas);

    canvas._alphaResizeObserver = resizeObserver;
}

export function updateAlphaColor(canvas, color) {
    if (!canvas) return;
    canvas._alphaColor = color;

    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    const rect = canvas.getBoundingClientRect();
    drawAlphaGradient(ctx, rect.width, rect.height, color);
}

/* ============================
   DRAWING
============================ */

function drawAlphaGradient(ctx, width, height, color) {
    ctx.clearRect(0, 0, width, height);

    const rgb = normalizeColor(color);
    const horizontal = width >= height;

    const gradient = horizontal
        ? ctx.createLinearGradient(0, 0, width, 0)
        : ctx.createLinearGradient(0, 0, 0, height);

    gradient.addColorStop(0, `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, 1)`);
    gradient.addColorStop(1, `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, 0)`);

    ctx.fillStyle = gradient;
    ctx.fillRect(0, 0, width, height);
}

/* ============================
   HELPERS
============================ */

function normalizeColor(color) {
    if (!color) return { r: 0, g: 0, b: 0 };

    if (color.startsWith("#")) {
        if (color.length === 7) {
            return {
                r: parseInt(color.slice(1, 3), 16),
                g: parseInt(color.slice(3, 5), 16),
                b: parseInt(color.slice(5, 7), 16)
            };
        }

        if (color.length === 9) {
            return {
                r: parseInt(color.slice(3, 5), 16),
                g: parseInt(color.slice(5, 7), 16),
                b: parseInt(color.slice(7, 9), 16)
            };
        }
    }

    return { r: 0, g: 0, b: 0 };
}